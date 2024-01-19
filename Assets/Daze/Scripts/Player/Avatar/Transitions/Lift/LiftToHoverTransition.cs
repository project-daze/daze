using UnityEngine;

namespace Daze.Player.Avatar
{
    public class LiftToHoverTransition : Transition
    {
        public override StateType From { get => StateType.Lift; }
        public override StateType To { get => StateType.Hover; }

        public LiftToHoverTransition(Context ctx) : base(ctx)
        { }

        public override bool Condition()
        {
            return true;
        }
    }
}
