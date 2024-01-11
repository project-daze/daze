using System;
using UnityEngine;

namespace Daze.Player
{
    public class CharacterGroundState : CharacterState
    {
        private Vector3 _moveDirection = Vector3.zero;
        private Vector3 _lookDirection = Vector3.zero;

        private bool _jumpedThisFrame = false;
        private bool _jumpConsumed = false;
        private float _timeSinceLastAbleToJump = 0f;

        public event Action<float> TimeSinceJumpRequestedUpdated;
        public event Action JumpConsumed;
        public event Action RejectJumpRequest;

        /// <summary>
        /// On update, calcurate the move and look direction based on player
        /// inputs and the current camera rotation so that we can use them
        /// when actually moving the character.
        ///
        /// Note that "forward" direction is different between the ground
        /// state and fall state. On ground, when pressing "up" key should
        /// move the character forward, but in fall state, it should move
        /// the character "up".
        /// </summary>
        public override void Update()
        {
            Vector3 right = Controller.Cam.Main.right;
            Vector3 forward = Vector3.Cross(right, -Character.Gravity).normalized;

            Vector3 forwardRelativeVInput = forward * Controller.Input.MoveComposite.y;
            Vector3 rightRelativeHInput = right * Controller.Input.MoveComposite.x;

            _moveDirection = forwardRelativeVInput + rightRelativeHInput;
            _lookDirection = _moveDirection;
        }

        /// <summary>
        /// Handle character movement when on ground.
        /// </summary>
        public override void UpdateVelocity(ref Vector3 velocity, float deltaTime)
        {
            // If the character is on the ground and stable, add normal
            // velocity to move the character.
            //
            // If not, that means the character is off ground, like falling
            // Off from the object, then add air movement velocity such as
            // adding gravity to pull the character down.
            if (Motor.GroundingStatus.IsStableOnGround)
            {
                SetStableGroundMovementVelociry(ref velocity, deltaTime);
            }
            else
            {
                SetAirMovementVelocity(ref velocity, deltaTime);
            }

            // Next see if the character is trying to jump. All the conditions
            // are handles inside this function.
            HandleJumpMovement(ref velocity, deltaTime);
        }

        private void SetStableGroundMovementVelociry(ref Vector3 velocity, float deltaTime)
        {
            // Reorient source velocity on current ground slope. This is
            // because we do not want our smoothing to cause any velocity
            // losses in slope changes.
            velocity = Motor.GetDirectionTangentToSurface(velocity, Motor.GroundingStatus.GroundNormal) * velocity.magnitude;

            // Calculate target velocity.
            Vector3 inputRight = Vector3.Cross(_moveDirection, Motor.CharacterUp);
            Vector3 reorientedInput = Vector3.Cross(Motor.GroundingStatus.GroundNormal, inputRight).normalized * _moveDirection.magnitude;
            Vector3 targetVelocity = reorientedInput * Character.MaxGroundMoveSpeed;

            // Smooth movement velocity and set to the velocity.
            float smoothFactor = GetExpSmoothFactor(Character.GroundMovementSharpness, deltaTime);

            velocity = Vector3.Lerp(velocity, targetVelocity, smoothFactor);
        }

        private void SetAirMovementVelocity(ref Vector3 velocity, float deltaTime)
        {
            // Add players air movement input to the velocity.
            if (_moveDirection.sqrMagnitude > 0f)
            {
                Vector3 targetVelocity = _moveDirection * Character.MaxAirMoveSpeed;

                // Prevent climbing on un-stable slopes with air movement.
                if (Motor.GroundingStatus.FoundAnyGround)
                {
                    Vector3 perpenticularObstructionNormal = Vector3.Cross(Vector3.Cross(Motor.CharacterUp, Motor.GroundingStatus.GroundNormal), Motor.CharacterUp).normalized;
                    targetVelocity = Vector3.ProjectOnPlane(targetVelocity, perpenticularObstructionNormal);
                }

                Vector3 velocityDiff = Vector3.ProjectOnPlane(targetVelocity - velocity, Character.Gravity);

                velocity += Character.AirAccelerationSpeed * deltaTime * velocityDiff;
            }

            // Add gravity force and air drag.
            velocity += Character.Gravity * deltaTime;
            velocity *= 1f / (1f + (Character.AirDrag * deltaTime));
        }

        public void HandleJumpMovement(ref Vector3 velocity, float deltaTime)
        {
            // Reset the jump checking state on every frame.
            _jumpedThisFrame = false;

            // Tell the input to update its time since jump requested.
            TimeSinceJumpRequestedUpdated?.Invoke(Controller.TimeSinceJumpRequested += deltaTime);

            // If the jump is not requested, we can just stop here.
            if (!Controller.JumpRequested) return;

            // OK, so jump is requested. But, lets check if we are really
            // allowed to jump. If not, again, abort here.
            if (!CanJump()) return;

            // Now we gonna actually jump. At first, calculate jump direction
            // before ungrounding.
            Vector3 jumpDirection = Motor.CharacterUp;

            // Kia: I am not sure what this line is for. Copied from the lib
            // example. Seems like it is adjusting the jump direction based
            // on the ground normal if `IsStableOnGround` is false, and finding
            // any ground. So..., maybe it is for wall jumping?
            if (Motor.GroundingStatus.FoundAnyGround && !Motor.GroundingStatus.IsStableOnGround)
            {
                jumpDirection = Motor.GroundingStatus.GroundNormal;
            }

            // Makes the character skip ground probing/snapping on its next
            // update. If this line were not here, the character would remain
            // snapped to the ground when trying to jump.
            Motor.ForceUnground(0.1f);

            // Add the jumping velocity and reset jump states.
            velocity += (jumpDirection * Character.JumpSpeed) - Vector3.Project(velocity, Motor.CharacterUp);

            _jumpConsumed = true;
            _jumpedThisFrame = true;

            JumpConsumed?.Invoke();
        }

        private bool CanJump()
        {
            // If the jump is already consumed, the character should not jump again.
            if (_jumpConsumed) return false;

            // Check if the character is on the ground and stable enough to
            // jump. This depends on `AllowJumpingWhenSliding` settings.
            // If so, we can return true here.
            bool isOnJumpableGround = Character.AllowJumpingWhenSliding
                ? Motor.GroundingStatus.FoundAnyGround
                : Motor.GroundingStatus.IsStableOnGround;

            if (!isOnJumpableGround) return true;

            // Finally, check if the character is in the grace period for
            // jumping after leaving the ground.
            return _timeSinceLastAbleToJump <= Character.JumpPostGroundingGraceTime;
        }

        /// <summary>
        /// Rotate character toward the taget movement direction. So, if the
        /// character is moving left, it should rotate toward left, while
        /// keeping the up direction stable.
        /// </summary>
        public override void UpdateRotation(ref Quaternion rotation, float deltaTime)
        {
            if (_lookDirection == Vector3.zero) return;

            // Smoothly interpolate from current to target look direction.
            float smoothFactor = GetExpSmoothFactor(Character.GroundOrientationSharpness, deltaTime);
            Vector3 smoothedLookDirection = Vector3.Slerp(Motor.CharacterForward, _lookDirection, smoothFactor).normalized;

            // Set the current rotation.
            rotation = Quaternion.LookRotation(smoothedLookDirection, Motor.CharacterUp);
        }

        public override void AfterCharacterUpdate(float deltaTime)
        {
            // Handle jumping pre-ground grace period.
            if (Controller.JumpRequested && Controller.TimeSinceJumpRequested > Character.JumpPreGroundingGraceTime)
            {
                RejectJumpRequest?.Invoke();
            }

            // Handle jumping while sliding. If the character is on a ground
            // surface, reset jumping values and return here.
            if (Character.AllowJumpingWhenSliding ? Motor.GroundingStatus.FoundAnyGround : Motor.GroundingStatus.IsStableOnGround)
            {
                if (!_jumpedThisFrame)
                {
                    _jumpConsumed = false;
                }
                _timeSinceLastAbleToJump = 0f;
                return;
            }

            // If not, we are still jumping. Keep track of time since we were
            // last able to jump (for grace period).
            _timeSinceLastAbleToJump += deltaTime;
        }

        private float GetExpSmoothFactor(float power, float deltaTime)
        {
            return 1 - Mathf.Exp(-power * deltaTime);
        }
    }
}
