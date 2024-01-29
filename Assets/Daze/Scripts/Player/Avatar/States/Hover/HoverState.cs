using UnityEngine;

namespace Daze.Player.Avatar
{
    public class HoverState : State
    {
        private bool _isStable = false;

        private float _driftTimeV = 0f;
        private float _driftTimeH = 0f;

        public HoverState(Context ctx) : base(ctx)
        { }

        public override void OnEnter()
        {
            _isStable = false;
            _driftTimeV = 0f;
            _driftTimeH = 0f;
        }

        public override void UpdateVelocity(ref Vector3 velocity, float deltaTime)
        {
            Ctx.UpdateFallSpeed(velocity.magnitude);

            // When entering this state, the player might be moving. So, at
            // first we will stabilize player to slowdown until certain
            // velocity treshold, then move to drifting state.
            if (!_isStable)
            {
                Stabilize(ref velocity, deltaTime);
                return;
            }

            Drift(ref velocity, deltaTime);
        }

        private void Stabilize(ref Vector3 velocity, float deltaTime)
        {
            if (velocity.magnitude > 0.1f)
            {
                velocity += -velocity.normalized * (Ctx.Settings.FallBrakeSpeed * deltaTime);
                return;
            }

            Ctx.EnterHovering();
            _isStable = true;
        }

        private void Drift(ref Vector3 velocity, float deltaTime)
        {
            _driftTimeV += Ctx.Settings.DriftVFrequency * deltaTime;
            _driftTimeH += Ctx.Settings.DriftHFrequency * deltaTime;

            float v = Mathf.Sin(-_driftTimeV * Mathf.PI) * Ctx.Settings.DriftVAmplitude;
            float h = Mathf.Cos(-_driftTimeH * Mathf.PI) * Ctx.Settings.DriftHAmplitude;

            Vector3 vOffset = Ctx.Motor.CharacterUp.normalized * v;
            Vector3 hOffset = Ctx.Motor.CharacterRight.normalized * h;

            velocity = vOffset + hOffset;
        }
    }
}
