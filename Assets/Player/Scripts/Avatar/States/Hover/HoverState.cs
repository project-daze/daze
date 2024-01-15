using UnityEngine;

namespace Daze.Player.Avatar
{
    public class HoverState : State
    {
        private bool _isStable = false;

        private float _driftTimeV = 0f;
        private float _driftTimeH = 0f;

        public float _driftVAmplitude1 = 0.4f;
        public float _driftVAmplitude2 = 0.25f;
        public float _driftVFrequency = 1f;

        public float _driftHAmplitude = 0.1f;
        public float _driftHFrequency = 0.1f;

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
            if (velocity.magnitude > 0.5f)
            {
                velocity *= 0.1f * deltaTime;
                return;
            }

            _isStable = true;
        }

        private void Drift(ref Vector3 velocity, float deltaTime)
        {
            _driftTimeV += _driftVFrequency * deltaTime;
            _driftTimeH += _driftHFrequency * deltaTime;

            float v = _driftTimeV % (2 * Mathf.PI) > Mathf.PI
                ? Mathf.Sin(-_driftTimeV * Mathf.PI) * _driftVAmplitude2
                : Mathf.Sin(-_driftTimeV * Mathf.PI) * _driftVAmplitude1;

            float h = Mathf.Sin(-_driftTimeH * Mathf.PI) * _driftHAmplitude;

            Vector3 offset = (Ctx.Settings.Gravity * v) + (Ctx.Camera.right * h);

            velocity = offset;
        }
    }
}
