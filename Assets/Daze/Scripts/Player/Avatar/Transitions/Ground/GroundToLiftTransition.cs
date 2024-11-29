using UnityEngine;

namespace Daze.Player.Avatar
{
    public class GroundToLiftTransition : Transition
    {
        public override StateType From { get => StateType.Ground; }
        public override StateType To { get => StateType.Lift; }

        public GroundToLiftTransition(Context ctx) : base(ctx)
        { }

        public override void OnTransition()
        {
            Ctx.Animator.TriggerHover();
            Ctx.FallRig.Enable();
            Ctx.EnterFloating();
        }
    }
}
