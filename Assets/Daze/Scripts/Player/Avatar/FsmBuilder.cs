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
            fsm.AddState(StateType.Hover, new HoverState(Ctx).Make());
            fsm.AddState(StateType.Fall, new FallState(Ctx).Make());

            fsm.SetStartState(StateType.Ground);

            // -----------------------------------------------------------------
            // Ground Transitions
            // -----------------------------------------------------------------

            // Ground -> Lift: On Input GravityOn.
            fsm.AddTriggerTransition(
                StateEvent.GravityOn,
                new GroundToLiftTransition(Ctx).Make()
            );

            // -----------------------------------------------------------------
            // Lift Transitions
            // -----------------------------------------------------------------

            // Lift -> Hover: On End.
            fsm.AddTransition(new LiftToHoverTransition(Ctx).Make());

            // Lift -> Ground: On Input GravityOff.
            fsm.AddTriggerTransition(
                StateEvent.GravityOff,
                new LiftToGroundTransition(Ctx).Make()
            );

            // -----------------------------------------------------------------
            // Hover Transitions
            // -----------------------------------------------------------------

            // Hover -> Ground: On Input GravityOff.
            fsm.AddTriggerTransition(
                StateEvent.GravityOff,
                new HoverToGroundTransition(Ctx).Make()
            );

            // Hover -> Fall: On Input GravityOn.
            fsm.AddTriggerTransition(
                StateEvent.GravityOn,
                new HoverToFallTransition(Ctx).Make()
            );

            // -----------------------------------------------------------------
            // Fall Transitions
            // -----------------------------------------------------------------

            // Fall -> Hover: On Input GravityOn.
            fsm.AddTriggerTransition(
                StateEvent.GravityOn,
                new FallToHoverTransition(Ctx).Make()
            );

            return fsm;
        }
    }
}
