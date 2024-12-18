using UnityEngine;

namespace Daze.Player.Avatar
{
    public class LiftState : State
    {
        public override bool NeedsExitTime { get => true; }

        private float _speed = 0f;
        private float _timer = 0f;

        public LiftState(Context ctx) : base(ctx)
        { }

        public override void OnEnter()
        {
            _speed = 0f;
            _timer = 0f;
            Ctx.Settings.Gravity = -Ctx.Settings.Gravity;
            Ctx.Motor.SetGroundSolvingActivation(false);
        }

        /// <summary>
        /// While on lift state, list the player "up" for a certain amount of
        /// time defined by `Daze.PlayerSettings.LiftTime` by accelerating
        /// rate of `Daze.PlayerSettings.LiftAcceleration`.
        /// </summary>
        public override void UpdateVelocity(ref Vector3 velocity, float deltaTime)
        {
            if (_timer < Ctx.Settings.LiftTime)
                Lift(ref velocity, deltaTime);
            else
                State.fsm.StateCanExit();
        }

        private void Lift(ref Vector3 velocity, float deltaTime)
        {
            _timer += deltaTime;
            _speed += Ctx.Settings.LiftAcceleration;
            velocity = Ctx.Settings.Gravity * (_speed * deltaTime);
        }

        public override bool CanExit()
        {
            return false;
        }
    }
}
