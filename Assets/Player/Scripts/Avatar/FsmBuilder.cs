using UnityHFSM;

namespace Daze.Player.Avatar
{
    public class FsmBuilder
    {
        public Context Ctx;

        public FsmBuilder(Context ctx)
        {
            Ctx = ctx;
        }

        public StateMachine<StateType, StateEvent> Make()
        {
            StateMachine<StateType, StateEvent>fsm = new();

            fsm.AddState(StateType.Ground, new GroundState(Ctx).Make());
            fsm.AddState(StateType.Lift, new LiftState(Ctx).Make());

            fsm.SetStartState(StateType.Ground);

            // Ground -> Lift: On Input GravityOn.
            fsm.AddTriggerTransition(
                StateEvent.GravityOn,
                new GroundToLiftTransition(Ctx).Make()
            );

            // Lift -> Ground: On Input GravityOff.
            fsm.AddTriggerTransition(
                StateEvent.GravityOff,
                new LiftToGroundTransition(Ctx).Make()
            );

            return fsm;
        }
    }
}
