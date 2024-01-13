using Daze.Fsm;

namespace Daze.Player.Avatar
{
    public class Transition : TransitionBuilder<StateType>
    {
        public Context Ctx;

        public Transition(Context ctx)
        {
            Ctx = ctx;
        }
    }
}
