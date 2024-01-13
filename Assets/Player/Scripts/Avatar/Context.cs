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

        public Context(
            PlayerSettings settings,
            PlayerInput input,
            Transform camera,
            KinematicCharacterMotor motor
        )
        {
            Settings = settings;
            Input = input;
            Camera = camera;
            Motor = motor;
        }
    }
}
