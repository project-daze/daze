namespace Daze.Player.Avatar
{
    public class HoverToGroundTransition : Transition
    {
        public override StateType From { get => StateType.Hover; }
        public override StateType To { get => StateType.Ground; }

        public HoverToGroundTransition(Context ctx) : base(ctx)
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
