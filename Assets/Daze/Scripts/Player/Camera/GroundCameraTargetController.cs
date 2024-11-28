using System;
using UnityEngine;

namespace Daze.Player.Camera
{
    /// <summary>
    /// This class is responsible for controlling the object that ground camera
    /// targets. It will follow specified target (usually the player's avatar)
    /// but with damping on Y axis (relative to the current gravity direction)
    /// to make the camera movement match GR.
    /// </summary>
    public class GroundCameraTargetController : MonoBehaviour
    {
        /// <summary>
        /// The playes state.
        /// </summary>
        [NonSerialized] public PlayerState State;

        /// <summary>
        /// The main camera that is used to check if the target is going out
        /// of the screen.
        /// </summary>
        public UnityEngine.Camera Camera;

        /// <summary>
        /// The target (usually the avatar) to follow.
        /// </summary>
        public Transform Target;

        /// <summary>
        /// The target's head position. This is used to check if the target is
        /// going out of the screen.
        /// </summary>
        public Transform TargetHead;

        /// <summary>
        /// The target's feet position. This is used to check if the target is
        /// going out of the screen.
        /// </summary>
        public Transform TargetFeet;

        /// <summary>
        /// The ground state damping factor for the camera movement. The higher
        /// the value, the faster the camera will follow the target.
        /// </summary>
        public float GroundStateDamping = 0.6f;

        /// <summary>
        /// The jump state damping factor for the camera movement. The lower
        /// the value, the faster the camera will follow the target since it
        /// uses the `SmoothDamp` function.
        /// </summary>
        public float JumpStateDamping = 0.6f;

        /// <summary>
        /// The velocity multiplier for the target's current velocity.
        /// </summary>
        public float JumpStateVelocityMultiplier = 0.1f;

        /// <summary>
        /// The padding on top of the screen on that is treated as hard limit.
        /// If the target goes out of this limit, the camera target will
        /// accelerate to follow the target.
        /// </summary>
        public float HardLimitTop = 32;

        /// <summary>
        /// The padding on bottom of the screen on that is treated as hard
        /// limit. If the target goes out of this limit, the camera target
        /// will accelerate to follow the target.
        /// </summary>
        public float HardLimitBottom = 32;

        /// <summary>
        /// The target's position at last frame. This is used to calculate the
        /// target's current velocity.
        /// </summary>
        private Vector3 _lastTargetPosition;

        /// <summary>
        /// The current velocity used in `SmoothDamp` function during the
        /// jump state camera movement.
        /// </summary>
        private Vector3 _jumpStateVelocity = Vector3.zero;

        /// <summary>
        /// Awake hook to set the player state from the parent.
        /// </summary>
        public void OnAwake(PlayerState state)
        {
            State = state;
        }

        /// <summary>
        /// Initialize the target position on start.
        /// </summary>
        public void Start()
        {
            _lastTargetPosition = Target.position;
        }

        /// <summary>
        /// Update the camera target's position and rotation. Doing this in
        /// `LateUpdate` to make sure the camera target because otherwise the
        /// camera movement becomes jittery. I don't know why.
        /// </summary>
        public void LateUpdate()
        {
            UpdateRotation();
            UpdatePosition();
        }

        /// <summary>
        /// Update the rotation of the camera. The camera rotation will always
        /// be the same as the target's rotation only on "roll" axis.
        /// </summary>
        private void UpdateRotation()
        {
            Quaternion targetRotation = Target.rotation;

            // Extract only the X and Z components of the rotation
            Vector3 targetEulerAngles = targetRotation.eulerAngles;

            // Keep Object A's current Y rotation
            Vector3 newEulerAngles = new(targetEulerAngles.x, transform.rotation.eulerAngles.y, targetEulerAngles.z);

            // Apply the new rotation to Object A
            transform.rotation = Quaternion.Euler(newEulerAngles);
        }

        /// <summary>
        /// Update the position of the camera.
        /// </summary>
        private void UpdatePosition()
        {
            // Get target's velocity by comparing to the last frame position.
            Vector3 targetVelocity = (Target.position - _lastTargetPosition) / Time.deltaTime;

            // Get the current surface normal. This can be current "up" because
            // the camera target will always be rotated in the correct
            // direction to match the gravity.
            Vector3 surfaceNormal = transform.up;

            // Get the vertical position of the camera target.
            Vector3 verticalPosition = Vector3.Project(transform.position, surfaceNormal);

            // Get target's ground position (x and z axis). This value will be
            // applied to the camera target's position directly since we don't
            // add any damping on x and z axis movement.
            Vector3 targetGroundPosition = Vector3.ProjectOnPlane(Target.position, surfaceNormal);

            // Get the vertical position, and the velocity of the vertical
            // movement of the target.
            Vector3 targetVerticalPosition = Vector3.Project(Target.position, surfaceNormal);
            Vector3 targetVerticalVelocity = Vector3.Project(targetVelocity, surfaceNormal);

            // Gte the new vertical position based on the current state.
            Vector3 newVerticalPosition = GetVerticalPosition(
                verticalPosition,
                targetVerticalPosition,
                targetVerticalVelocity
            );

            // Combine ground and calculated vertical position then apply to
            // the object position.
            transform.position = targetGroundPosition + newVerticalPosition;

            // Record target's position for the next frame.
            _lastTargetPosition = Target.position;
        }

        /// <summary>
        /// Get the vertical position based on the current state.
        /// </summary>
        private Vector3 GetVerticalPosition(Vector3 position, Vector3 targetPosition, Vector3 targetVelocity)
        {
            // If the target is going out of the screen, we need move the
            // camera faster to catch up with the target so we just apply
            // the target velocity to the momvement.
            //
            // But if the target is not moving, or moving slowly, we will let
            // ordinary damping to move the camera target so that the camera
            // won't stuck in the same position.
            if (!IsTargetInSafeZone() && Mathf.Abs(targetVelocity.sqrMagnitude) > 0.01f)
            {
                return position + targetVelocity * Time.deltaTime;
            }

            return State.IsJumping
                ? GetJumpStateVerticalPosition(position, targetVelocity)
                : GetGroundStateVerticalPosition(position, targetPosition);
        }

        /// <summary>
        /// Get the next vertical position when the player is on the ground.
        /// </summary>
        private Vector3 GetGroundStateVerticalPosition(Vector3 position, Vector3 targetPosition)
        {
            // Reset JumpStateVelocity when the player is on the ground so that
            // the jump state velocity can always start fresh.
            _jumpStateVelocity = Vector3.zero;

            return Vector3.Lerp(position, targetPosition, GroundStateDamping * Time.deltaTime);
        }

        /// <summary>
        /// Get the next vertical position when the player is jumping.
        /// </summary>
        private Vector3 GetJumpStateVerticalPosition(Vector3 position, Vector3 targetVelocity)
        {
            return Vector3.SmoothDamp(
                position,
                position + targetVelocity * JumpStateVelocityMultiplier,
                ref _jumpStateVelocity,
                JumpStateDamping
            );
        }

        /// <summary>
        /// Check if the target is inside the hard limit area (safe zone).
        /// </summary>
        private bool IsTargetInSafeZone()
        {
            Vector3 headScreenPosition = Camera.WorldToScreenPoint(TargetHead.position);
            Vector3 feetScreenPosition = Camera.WorldToScreenPoint(TargetFeet.position);

            float hardLimitTop = Screen.height - HardLimitTop;
            float hardLimitBottom = HardLimitBottom;

            if (
                headScreenPosition.y > hardLimitTop
                || headScreenPosition.y < hardLimitBottom
                || feetScreenPosition.y > hardLimitTop
                || feetScreenPosition.y < hardLimitBottom
            )
            {
                return false;
            }

            return true;
        }
    }
}
