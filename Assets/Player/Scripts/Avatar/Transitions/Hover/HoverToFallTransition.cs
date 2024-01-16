using UnityEngine;

namespace Daze.Player.Avatar
{
    public class HoverToFallTransition : Transition
    {
        public override StateType From { get => StateType.Hover; }
        public override StateType To { get => StateType.Fall; }

        public HoverToFallTransition(Context ctx) : base(ctx)
        { }
    }
}
