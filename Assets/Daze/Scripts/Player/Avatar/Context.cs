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

        public Transform IKLeftHand;
        public Transform IKRightHand;
        public Transform IkLeftFoot;
        public Transform IkRightFoot;

        public event Action EnterFallingState;
        public event Action LeaveFallingState;

        public Context(
            PlayerSettings settings,
            PlayerInput input,
            Transform camera,
            KinematicCharacterMotor motor,
            Animator animator,
            Transform ikLeftHand,
            Transform ikRightHand,
            Transform ikLeftFoot,
            Transform ikRightFoot
        )
        {
            Settings = settings;
            Input = input;
            Camera = camera;
            Motor = motor;
            Animator = animator;
            IKLeftHand = ikLeftHand;
            IKRightHand = ikRightHand;
            IkLeftFoot = ikLeftFoot;
            IkRightFoot = ikRightFoot;
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
