using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using KinematicCharacterController;

namespace Daze.Player
{
    public abstract class CharacterState
    {
        public Controller Controller;
        public Character Character;
        public KinematicCharacterMotor Motor;

        public void OnAwake(Controller controller)
        {
            Controller = controller;
            Character = controller.Character;
            Motor = controller.Character.Motor;
        }

        public virtual void Update() { }

        /// <summary>
        /// This is called when the motor wants to know what its velocity
        /// should be right now. This is the ONLY place where you can
        /// set the character's velocity.
        /// </summary>
        public virtual void UpdateVelocity(ref Vector3 velocity, float deltaTime) { }

        /// <summary>
        /// This is called by KinematicCharacterMotor during its update cycle.
        /// This is where you tell your character what its rotation should be
        /// right now. This is the ONLY place where you should set the
        /// character's rotation.
        /// </summary>
        public virtual void UpdateRotation(ref Quaternion rotation, float deltaTime) { }


        /// <summary>
        /// Called by KinematicCharacterMotor during its update cycle. This is
        /// called after the character has finished its movement update.
        /// </summary>
        public virtual void AfterCharacterUpdate(float deltaTime) { }
    }
}
