using System;
using KinematicCharacterController;
using UnityEngine;
using Daze.Player.Avatar.Rigs;

namespace Daze.Player.Avatar
{
    [Serializable]
    public class Context
    {
        public PlayerSettings Settings;
        public PlayerInput Input;
        public Transform Camera;
        public KinematicCharacterMotor Motor;
        public Animator Animator;
        public FallRig FallRig;

        public event Action EnterFallingState;
        public event Action LeaveFallingState;

        public void EnterFalling()
        {
            EnterFallingState?.Invoke();
        }

        public void LeaveFalling()
        {
            LeaveFallingState?.Invoke();
        }
    }
}
