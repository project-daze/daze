using Daze.Fsm;
using UnityEngine;

namespace Daze.Player.Avatar
{
    public class UpdateVelocityData
    {
        public Vector3 Velocity;
        public float DeltaTime;
    }

    public class UpdateRotationData
    {
        public Quaternion Rotation;
        public float DeltaTime;
    }

    public class State : StateBuilder<StateType, StateEvent>
    {
        public Context Ctx;

        public State(Context ctx)
        {
            Ctx = ctx;
        }

        public override void Boot()
        {
            State.AddAction(StateEvent.Update, Update);
            State.AddAction(StateEvent.FixedUpdate, FixedUpdate);

            State.AddAction(
                StateEvent.UpdateVelocity,
                (UpdateVelocityData data) =>
                {
                    Vector3 velocity = data.Velocity;
                    UpdateVelocity(ref velocity, data.DeltaTime);
                    data.Velocity = velocity;
                }
            );

            State.AddAction(
                StateEvent.UpdateRotation,
                (UpdateRotationData data) =>
                {
                    Quaternion rotation = data.Rotation;
                    UpdateRotation(ref rotation, data.DeltaTime);
                    data.Rotation = rotation;
                }
            );

            State.AddAction<float>(StateEvent.AfterCharacterUpdate, AfterCharacterUpdate);
        }

        public virtual void Update()
        { }

        public virtual void FixedUpdate()
        { }

        public virtual void UpdateVelocity(ref Vector3 velocity, float deltaTime)
        { }

        public virtual void UpdateRotation(ref Quaternion rotation, float deltaTime)
        { }

        public virtual void AfterCharacterUpdate(float deltaTime)
        { }

        public virtual void OnAnimatorIK()
        { }
    }
}
