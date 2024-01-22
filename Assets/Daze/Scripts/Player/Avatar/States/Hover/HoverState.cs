using UnityEngine;

namespace Daze.Player.Avatar
{
    public class HoverState : State
    {
        private bool _isStable = false;

        private float _driftTimeV = 0f;
        private float _driftTimeH = 0f;

        private readonly int _hoverHash = Animator.StringToHash("Hover");

        public HoverState(Context ctx) : base(ctx)
        { }

        public override void OnEnter()
        {
            _isStable = false;
            _driftTimeV = 0f;
            _driftTimeH = 0f;
            Ctx.Animator.SetTrigger(_hoverHash);
        }

        public override void UpdateVelocity(ref Vector3 velocity, float deltaTime)
        {
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
            // If the velocity is bigger than 0.5, we will slowdown the player
            // until it reaches 0.5.
            if (velocity.magnitude > 0.1f)
            {
                velocity += -velocity.normalized * deltaTime;
                return;
            }

            _isStable = true;
        }

        private void Drift(ref Vector3 velocity, float deltaTime)
        {
            _driftTimeV += Ctx.Settings.DriftVFrequency * deltaTime;
            _driftTimeH += Ctx.Settings.DriftHFrequency * deltaTime;

            float v = _driftTimeV % (2 * Mathf.PI) > Mathf.PI
                ? Mathf.Sin(-_driftTimeV * Mathf.PI) * Ctx.Settings.DriftVAmplitude1
                : Mathf.Sin(-_driftTimeV * Mathf.PI) * Ctx.Settings.DriftVAmplitude2;

            float h = Mathf.Cos(-_driftTimeH * Mathf.PI) * Ctx.Settings.DriftHAmplitude;

            Vector3 vOffset = -Ctx.Motor.CharacterUp.normalized * v;
            Vector3 hOffset = Ctx.Motor.CharacterRight.normalized * h;

            velocity = vOffset + hOffset;
        }
    }
}
