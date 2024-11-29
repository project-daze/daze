using UnityEngine;

namespace Daze.Player.Avatar
{
    public class FallToHoverTransition : Transition
    {
        public override StateType From { get => StateType.Fall; }
        public override StateType To { get => StateType.Hover; }

        public FallToHoverTransition(Context ctx) : base(ctx)
        { }

        public override void OnTransition()
        {
            Ctx.Animator.TriggerHover();
            Ctx.LeaveFalling();
        }
    }
}
