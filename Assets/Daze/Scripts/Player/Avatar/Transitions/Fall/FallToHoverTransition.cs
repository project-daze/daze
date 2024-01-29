using UnityEngine;

namespace Daze.Player.Avatar
{
    public class FallToHoverTransition : Transition
    {
        public override StateType From { get => StateType.Fall; }
        public override StateType To { get => StateType.Hover; }

        private readonly int _HoverHash = Animator.StringToHash("Hover");

        public FallToHoverTransition(Context ctx) : base(ctx)
        { }

        public override void OnTransition()
        {
            Ctx.Animator.SetTrigger(_HoverHash);
            Ctx.LeaveFalling();
        }
    }
}
