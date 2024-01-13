using UnityEngine;

namespace Daze.Player.Avatar
{
    public class LiftState : State
    {
        private float _speed = 0f;
        private float _timer = 0f;
        private bool _stop = false;

        public LiftState(Context ctx) : base(ctx)
        { }

        public override void OnEnter()
        {
            _speed = 0f;
            _timer = 0f;
            _stop = false;
        }

        /// <summary>
        /// While on lift state, list the player "up" for a certain amount of
        /// time defined by `Daze.PlayerSettings.LiftTime` by accelerating
        /// rate of `Daze.PlayerSettings.LiftAcceleration`.
        /// </summary>
        public override void UpdateVelocity(ref Vector3 velocity, float deltaTime)
        {
            if (_stop) {
                return;
            }

            Ctx.Motor.ForceUnground();

            _timer += deltaTime;

            if (_timer >= Ctx.Settings.LiftTime) {
                _stop = true;
                _timer = 0f;
                velocity = Vector3.zero;
                return;
            }

            _speed += Ctx.Settings.LiftAcceleration;

            velocity = -Ctx.Settings.Gravity * (_speed * deltaTime);
        }
    }
}
