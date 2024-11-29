using UnityEngine;

namespace Daze.Player.Avatar
{
    public class LiftToGroundTransition : Transition
    {
        public override StateType From { get => StateType.Lift; }
        public override StateType To { get => StateType.Ground; }
        public override bool ForceInstantly { get => true; }

        public LiftToGroundTransition(Context ctx) : base(ctx)
        { }

        public override void OnTransition()
        {
            Ctx.Animator.TriggerLand();
            Ctx.FallRig.Disable();
            Ctx.LeaveFloating();
            Ctx.LeaveHovering();
        }
    }
}
