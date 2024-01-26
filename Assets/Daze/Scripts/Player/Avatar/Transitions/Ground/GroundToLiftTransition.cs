using UnityEngine;

namespace Daze.Player.Avatar
{
    public class GroundToLiftTransition : Transition
    {
        public override StateType From { get => StateType.Ground; }
        public override StateType To { get => StateType.Lift; }

        private readonly int _HoverHash = Animator.StringToHash("Hover");

        public GroundToLiftTransition(Context ctx) : base(ctx)
        { }

        public override void OnTransition()
        {
            Ctx.Animator.SetTrigger(_HoverHash);
            Ctx.EnterFalling();
        }
    }
}
