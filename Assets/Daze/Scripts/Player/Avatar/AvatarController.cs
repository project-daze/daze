using System;
using UnityEngine;
using KinematicCharacterController;
using UnityHFSM;
using Daze.Player.Avatar.Rigs;

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
        public AvatarAnimatorMoveHook AnimatorMoveHook;
        public FallRig FallRig;
        public KinematicCharacterMotor Motor;

        private Context _ctx;
        private StateMachine<StateType, StateEvent> _fsm;

        public event Action OnLanded;
        public event Action OnJumped;

        public event Action EnterFloatingState;
        public event Action LeaveFloatingState;
        public event Action EnterFallingState;
        public event Action LeaveFallingState;
        public event Action EnterHoveringState;
        public event Action LeaveHoveringState;

        public event Action<float> FallSpeedUpdated;

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
            SetupAnimatorMoveHook();
        }

        private void SetupInput()
        {
            Input.GravityOn += () => _fsm.Trigger(StateEvent.GravityOn);
            Input.GravityOff += () => _fsm.Trigger(StateEvent.GravityOff);
        }

        private void SetupContext()
        {
            _ctx = new Context
            {
                Settings = Settings,
                Input = Input,
                Camera = Camera,
                Motor = Motor,
                Animator = new AvatarAnimator(Animator),
                FallRig = FallRig
            };

            _ctx.OnLanded += () => OnLanded?.Invoke();
            _ctx.OnJumped += () => OnJumped?.Invoke();

            _ctx.EnterFloatingState += () => EnterFloatingState?.Invoke();
            _ctx.LeaveFloatingState += () => LeaveFloatingState?.Invoke();
            _ctx.EnterFallingState += () => EnterFallingState?.Invoke();
            _ctx.LeaveFallingState += () => LeaveFallingState?.Invoke();
            _ctx.EnterHoveringState += () => EnterHoveringState?.Invoke();
            _ctx.LeaveHoveringState += () => LeaveHoveringState?.Invoke();

            _ctx.FallSpeedUpdated += (speed) => FallSpeedUpdated?.Invoke(speed);
        }

        private void SetupFms()
        {
            _fsm = new FsmBuilder(_ctx).Make();

            _fsm.Init();
        }

        private void SetupAnimatorMoveHook()
        {
            AnimatorMoveHook.OnMove += OnAvatarAnimatorMove;
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

            // Reset animation root motion deltas.
            _ctx.Animator.RootMotionPositionDelta = Vector3.zero;
            _ctx.Animator.RootMotionRotationDelta = Quaternion.identity;
        }

        public void PostGroundingUpdate(float deltaTime) {
            _fsm.OnAction(StateEvent.PostGroundingUpdate, deltaTime);
        }

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

        /// <summary>
        /// The animation root motion callback from the attached `Animator`
        /// component. We'll pass the data to the context so that the state
        /// classes can use the value.
        ///
        /// The value gets reset every frame in `AfterCharacterUpdate` once the
        /// the character updated its motion.
        /// </summary>
        public void OnAvatarAnimatorMove(Vector3 deltaPosition)
        {
            // Accumulate rootMotion deltas between character updates
            Debug.Log("Animator:" + Animator.deltaPosition);
            _ctx.Animator.RootMotionPositionDelta += deltaPosition;
            // _ctx.Animator.RootMotionRotationDelta = Animator.deltaRotation * _ctx.Animator.RootMotionRotationDelta;
        }
    }
}
