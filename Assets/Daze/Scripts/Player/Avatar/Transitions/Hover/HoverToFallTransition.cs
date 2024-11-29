namespace Daze.Player.Avatar
{
    public class HoverToFallTransition : Transition
    {
        public override StateType From { get => StateType.Hover; }
        public override StateType To { get => StateType.Fall; }

        public HoverToFallTransition(Context ctx) : base(ctx)
        { }

        public override void OnTransition()
        {
            Ctx.Animator.TriggerFall();
            Ctx.LeaveHovering();
            Ctx.EnterFalling();
        }
    }
}
