using System;
using UnityEngine;
using Daze.Player.Avatar;
using Daze.Player.Camera;

namespace Daze.Player
{
    public class PlayerController : MonoBehaviour
    {
        public PlayerSettings Settings;
        public PlayerInput Input;
        public PlayerState State;

        public CameraController Camera;
        public AvatarController Avatar;

        private void Awake()
        {
            Avatar.OnAwake(Settings, Input, Camera.Main);
            Camera.OnAwake(Settings, Input, State);

            Avatar.EnterFallingState += () => State.IsFalling = true;
            Avatar.LeaveFallingState += () => State.IsFalling = false;
        }
    }
}
