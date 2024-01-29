using UnityEngine;

namespace Daze.Player.Avatar
{
    public class HoverToFallTransition : Transition
    {
        public override StateType From { get => StateType.Hover; }
        public override StateType To { get => StateType.Fall; }

        private readonly int _fallHash = Animator.StringToHash("Fall");

        public HoverToFallTransition(Context ctx) : base(ctx)
        { }

        public override void OnTransition()
        {
            Ctx.Animator.SetTrigger(_fallHash);
            Ctx.LeaveHovering();
            Ctx.EnterFalling();
        }
    }
}
