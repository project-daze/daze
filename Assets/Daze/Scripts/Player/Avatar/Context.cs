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

        public Transform FkUpperArmL;
        public Transform FkUpperArmR;

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
