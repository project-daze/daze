using System;
using KinematicCharacterController;
using UnityEngine;

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

        public event Action EnterFallingState;
        public event Action LeaveFallingState;

        public Context(
            PlayerSettings settings,
            PlayerInput input,
            Transform camera,
            KinematicCharacterMotor motor,
            Animator animator
        )
        {
            Settings = settings;
            Input = input;
            Camera = camera;
            Motor = motor;
            Animator = animator;
        }

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
