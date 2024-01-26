using UnityEngine;

namespace Daze.Player.Avatar
{
    public class HoverToGroundTransition : Transition
    {
        public override StateType From { get => StateType.Hover; }
        public override StateType To { get => StateType.Ground; }

        private readonly int _LandHash = Animator.StringToHash("Land");

        public HoverToGroundTransition(Context ctx) : base(ctx)
        { }

        public override void OnTransition()
        {
            Ctx.Animator.SetTrigger(_LandHash);
            Ctx.FallRig.Disable();
            Ctx.LeaveFalling();
        }
    }
}
