using System;
using UnityEngine;
using KinematicCharacterController;

namespace Daze.Player
{
    public class Character : MonoBehaviour, ICharacterController
    {
        [NonSerialized] public Controller Controller;

        public KinematicCharacterMotor Motor;

        // ---------------------------------------------------------------------
        // Gravity
        // ---------------------------------------------------------------------

        [Header("Gravity")]
        public Vector3 Gravity = new(0, -15f, 0);

        // ---------------------------------------------------------------------
        // Ground Movement
        // ---------------------------------------------------------------------

        [Header("Ground Movement")]
        public float MaxGroundMoveSpeed = 10f;
        public float GroundMovementSharpness = 15f;
        public float GroundOrientationSharpness = 15f;

        // ---------------------------------------------------------------------
        // Air Movement
        // ---------------------------------------------------------------------

        [Header("Air Movement")]
        public float MaxAirMoveSpeed = 5f;
        public float AirAccelerationSpeed = 5f;
        public float AirDrag = 0.5f;

        // ---------------------------------------------------------------------
        // Jump Movement
        // ---------------------------------------------------------------------

        [Header("Jump Movement")]
        public float JumpSpeed = 8f;

        /// <summary>
        /// The extra time before landing where you can press jump and still
        /// jump once you land.
        /// </summary>
        public float JumpPreGroundingGraceTime = 0f;

        /// <summary>
        /// The extra time after leaving stable ground where you will still
        /// be allowed to jump.
        /// </summary>
        public float JumpPostGroundingGraceTime = 0f;

        public bool AllowJumpingWhenSliding = false;

        public event Action<float> TimeSinceJumpRequestedUpdated;
        public event Action JumpConsumed;
        public event Action RejectJumpRequest;

        // ---------------------------------------------------------------------
        // States
        // ---------------------------------------------------------------------

        private CharacterState _state;
        private readonly CharacterGroundState _groundState = new();

        public void OnAwake(Controller controller)
        {
            Controller = controller;
            Motor.CharacterController = this;

            _groundState.OnAwake(Controller);

            _groundState.TimeSinceJumpRequestedUpdated += (float v) => TimeSinceJumpRequestedUpdated?.Invoke(v);
            _groundState.JumpConsumed += () => JumpConsumed?.Invoke();
            _groundState.RejectJumpRequest += () => RejectJumpRequest?.Invoke();

            _state = _groundState;
        }

        public void ChangeState(CharacterState state)
        {
            _state = state;
        }

        public void Update()
        {
            _state.Update();
        }

        /// <summary>
        /// This is called when the motor wants to know what its velocity
        /// should be right now. This is the ONLY place where you can
        /// set the character's velocity.
        /// </summary>
        public void UpdateVelocity(ref Vector3 velocity, float deltaTime)
        {
            _state.UpdateVelocity(ref velocity, deltaTime);
        }

        /// <summary>
        /// This is called by KinematicCharacterMotor during its update cycle.
        /// This is where you tell your character what its rotation should be
        /// right now. This is the ONLY place where you should set the
        /// character's rotation.
        /// </summary>
        public void UpdateRotation(ref Quaternion rotation, float deltaTime)
        {
            _state.UpdateRotation(ref rotation, deltaTime);
        }

        public void BeforeCharacterUpdate(float deltaTime) { }

        /// <summary>
        /// Called by KinematicCharacterMotor during its update cycle. This is
        /// called after the character has finished its movement update.
        /// </summary>
        public void AfterCharacterUpdate(float deltaTime)
        {
            _state.AfterCharacterUpdate(deltaTime);
        }

        public void PostGroundingUpdate(float deltaTime) { }

        public void OnGroundHit(Collider hitCollider, Vector3 hitNormal, Vector3 hitPoint, ref HitStabilityReport hitStabilityReport) { }

        public void OnMovementHit(Collider hitCollider, Vector3 hitNormal, Vector3 hitPoint, ref HitStabilityReport hitStabilityReport) { }

        public void ProcessHitStabilityReport(Collider hitCollider, Vector3 hitNormal, Vector3 hitPoint, Vector3 atCharacterPosition, Quaternion atCharacterRotation, ref HitStabilityReport hitStabilityReport) { }

        /// <summary>
        /// This is called after when the motor wants to know if the collider
        /// can be collided with (or if we just go through it).
        /// </summary>
        public bool IsColliderValidForCollisions(Collider coll)
        {
            return true;
        }

        public void OnDiscreteCollisionDetected(Collider hitCollider) { }
    }
}
