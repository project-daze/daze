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

            if (velocity.magnitude > 10f)
            {
                Ctx.FallRig.Break();
            }

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
            if (velocity.magnitude > 0.05f)
            {
                velocity = Vector3.Lerp(
                    velocity,
                    Vector3.zero,
                    Ctx.Settings.FallBrakeSpeed * deltaTime
                );

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

            // For vertial movement, we use the current gravity direction.
            Vector3 vOffset = -Ctx.Settings.Gravity.normalized * v;

            // For horizontal movement, we use the character's current "right",
            // but if that angle is too close to the gravity direction, we will
            // use the "forward" instead.
            float angle = Vector3.Angle(
                Ctx.Settings.Gravity.normalized,
                Ctx.Motor.CharacterRight.normalized
            );

            Vector3 hOffset = Mathf.Abs(angle) < 20f
                ? Ctx.Motor.CharacterForward.normalized * h
                : Ctx.Motor.CharacterRight.normalized * h;

            velocity = vOffset + hOffset;
        }
    }
}
