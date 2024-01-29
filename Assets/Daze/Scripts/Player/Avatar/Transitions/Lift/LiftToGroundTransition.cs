using UnityEngine;

namespace Daze.Player.Avatar
{
    public class LiftToGroundTransition : Transition
    {
        public override StateType From { get => StateType.Lift; }
        public override StateType To { get => StateType.Ground; }
        public override bool ForceInstantly { get => true; }

        private readonly int _LandHash = Animator.StringToHash("Land");

        public LiftToGroundTransition(Context ctx) : base(ctx)
        { }

        public override void OnTransition()
        {
            Ctx.Animator.SetTrigger(_LandHash);
            Ctx.FallRig.Disable();
            Ctx.LeaveFloating();
            Ctx.LeaveHovering();
        }
    }
}
