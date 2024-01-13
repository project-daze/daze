using UnityHFSM;

namespace Daze.Fsm
{
    /// <summary>
    /// `StateBuilder` builds `UnityHFSM.State` by composing attaching the
    /// various actions such as OnLogic and Update. This is used to create
    /// the states we use in this appalication.
    ///
    /// We are using this builder pattern instead of inheriting the base
    /// `UnityHFSM.State` here because of the following reasons.
    ///
    /// At first, `UnityHFSM.State` contains predefined logics inside each
    /// actions hence if we inherit the base state, we must keep calling things
    /// like `base.OnLogic` on every override. Which is pretty annoying.
    /// </summary>
    public class StateBuilder<TStateId, TEvent>
    {
        public TStateId Name;
        public State<TStateId, TEvent> State;
        public bool NeedsExitTime;
        public bool IsGhostState;
        public ITimer Timer;

        public virtual State<TStateId, TEvent> Make()
        {
            State<TStateId, TEvent> state = new(
                onEnter: (state) => OnEnter(),
                onLogic: (state) => OnLogic(),
                onExit: (state) => OnExit(),
                canExit: (state) => CanExit(),
                needsExitTime: NeedsExitTime,
                isGhostState: IsGhostState
            );

            Name = state.name;
            State = state;
            Timer = state.timer;

            Boot();

            return State;
        }

        public virtual void Boot()
        { }

        public virtual void OnEnter()
        { }

        public virtual void OnLogic()
        { }

        public virtual void OnExit()
        { }

        public virtual bool CanExit()
        {
            return true;
        }
    }
}
