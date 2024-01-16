using UnityHFSM;

namespace Daze.Fsm
{
    /// <summary>
    /// `TransitionBuilder` builds `UnityHFSM.Transition` by composing
    /// attaching the various actions such as OnLogic and Update. See
    /// `Daze.Fsm.StateBuilder` for why we are using this builder pattern.
    /// </summary>
    public class TransitionBuilder<TStateId>
    {
        public Transition<TStateId> Transition;

        public virtual TStateId From { get; }
        public virtual TStateId To { get; }
        public virtual bool ForceInstantly { get => false; }

        public virtual Transition<TStateId> Make()
        {
            Transition<TStateId> transition = new(
                from: From,
                to: To,
                condition: (transition) => Condition(),
                onTransition: (transition) => OnTransition(),
                afterTransition: (transition) => AfterTransition(),
                forceInstantly: ForceInstantly
            );

            Transition = transition;

            Boot();

            return transition;
        }

        public virtual void Boot()
        { }

        public virtual bool Condition()
        {
            return true;
        }

        public virtual void OnTransition()
        { }

        public virtual void AfterTransition()
        { }
    }
}
