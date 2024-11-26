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

        public event Action OnLanded;
        public event Action OnJumped;

        public event Action EnterFloatingState;
        public event Action LeaveFloatingState;
        public event Action EnterHoveringState;
        public event Action LeaveHoveringState;
        public event Action EnterFallingState;
        public event Action LeaveFallingState;

        public event Action<float> FallSpeedUpdated;

        public void EmitLanded()
        {
            OnLanded?.Invoke();
        }

        public void EmitJumped()
        {
            OnJumped?.Invoke();
        }

        public void EnterFloating()
        {
            EnterFloatingState?.Invoke();
        }

        public void LeaveFloating()
        {
            LeaveFloatingState?.Invoke();
        }

        public void EnterFalling()
        {
            EnterFallingState?.Invoke();
        }

        public void LeaveFalling()
        {
            LeaveFallingState?.Invoke();
        }

        public void EnterHovering()
        {
            EnterHoveringState?.Invoke();
        }

        public void LeaveHovering()
        {
            LeaveHoveringState?.Invoke();
        }

        public void UpdateFallSpeed(float speed)
        {
            FallSpeedUpdated?.Invoke(speed);
        }
    }
}
