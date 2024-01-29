using UnityEngine;

namespace Daze.Player.Avatar
{
    public class FallState : State
    {
        public FallState(Context ctx) : base(ctx)
        { }

        public override void OnEnter()
        {
            Ctx.Settings.Gravity = Ctx.Camera.forward;
        }

        public override void UpdateVelocity(ref Vector3 velocity, float deltaTime)
        {
            if (velocity.magnitude < Ctx.Settings.MaxFallSpeed)
            {
                velocity += Ctx.Settings.Gravity * (Ctx.Settings.FallAcceleration * deltaTime);
            }

            Ctx.UpdateFallSpeed(velocity.magnitude);
        }
    }
}
