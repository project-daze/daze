using System;
using UnityEngine;
using KinematicCharacterController;
using UnityHFSM;

namespace Daze.Player.Avatar
{
    [RequireComponent(typeof(KinematicCharacterMotor))]
    public class AvatarController : MonoBehaviour, ICharacterController
    {
        [NonSerialized] public PlayerSettings Settings;
        [NonSerialized] public PlayerInput Input;
        [NonSerialized] public Transform Camera;

        public Transform Body;
        public Animator Animator;
        public IkController IkController;
        public KinematicCharacterMotor Motor;

        public Transform IkLeftHand;
        public Transform IkRightHand;
        public Transform IkLeftFoot;
        public Transform IkRightFoot;

        private Context _ctx;
        private StateMachine<StateType, StateEvent> _fsm;

        public event Action EnterFallingState;
        public event Action LeaveFallingState;

        public void OnAwake(PlayerSettings settings, PlayerInput input, Transform camera)
        {
            Settings = settings;
            Input = input;
            Camera = camera;
        }

        public void Start()
        {
            Motor.CharacterController = this;
            SetupInput();
            SetupContext();
            SetupFms();
            SetupEvents();
        }

        private void SetupInput()
        {
            Input.GravityOn += () => _fsm.Trigger(StateEvent.GravityOn);
            Input.GravityOff += () => _fsm.Trigger(StateEvent.GravityOff);
        }

        private void SetupContext()
        {
            _ctx = new(
                Settings,
                Input,
                Camera,
                Motor,
                Animator,
                IkLeftHand,
                IkRightHand,
                IkLeftFoot,
                IkRightFoot
            );

            _ctx.EnterFallingState += () => EnterFallingState?.Invoke();
            _ctx.LeaveFallingState += () => LeaveFallingState?.Invoke();
        }

        private void SetupFms()
        {
            _fsm = new FsmBuilder(_ctx).Make();

            _fsm.Init();
        }

        private void SetupEvents()
        {
            IkController.OnAnimatorIk += () => _fsm.OnAction(StateEvent.OnAnimatorIK);
        }

        public void Update()
        {
            _fsm.OnLogic();
            _fsm.OnAction(StateEvent.Update);
        }

        public void FixedUpdate()
        {
            _fsm.OnAction(StateEvent.FixedUpdate);
        }

        /// <summary>
        /// This is called when the motor wants to know what its velocity
        /// should be right now. This is the ONLY place where you can
        /// set the character's velocity.
        /// </summary>
        public void UpdateVelocity(ref Vector3 velocity, float deltaTime)
        {
            UpdateVelocityData data = new()
            {
                Velocity = velocity,
                DeltaTime = deltaTime
            };

            _fsm.OnAction(StateEvent.UpdateVelocity, data);

            velocity = data.Velocity;
        }

        /// <summary>
        /// This is called by KinematicCharacterMotor during its update cycle.
        /// This is where you tell your character what its rotation should be
        /// right now. This is the ONLY place where you should set the
        /// character's rotation.
        /// </summary>
        public void UpdateRotation(ref Quaternion rotation, float deltaTime)
        {
            UpdateRotationData data = new()
            {
                Rotation = rotation,
                DeltaTime = deltaTime
            };

            _fsm.OnAction(StateEvent.UpdateRotation, data);

            rotation = data.Rotation;
        }

        public void BeforeCharacterUpdate(float deltaTime) { }

        /// <summary>
        /// Called by KinematicCharacterMotor during its update cycle. This is
        /// called after the character has finished its movement update.
        /// </summary>
        public void AfterCharacterUpdate(float deltaTime)
        {
            // _state.AfterCharacterUpdate(deltaTime);
            _fsm.OnAction(StateEvent.AfterCharacterUpdate, deltaTime);
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
