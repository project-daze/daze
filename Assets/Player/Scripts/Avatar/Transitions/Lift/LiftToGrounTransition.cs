using UnityEngine;

namespace Daze.Player.Avatar
{
    public class LiftToGroundTransition : Transition
    {
        public override StateType From { get => StateType.Lift; }
        public override StateType To { get => StateType.Ground; }

        public LiftToGroundTransition(Context ctx) : base(ctx)
        { }
    }
}
