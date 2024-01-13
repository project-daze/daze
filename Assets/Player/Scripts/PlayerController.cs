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

        public CameraController Camera;
        public AvatarController Avatar;

        public bool IsFalling = false;

        private void Awake()
        {
            Avatar.OnAwake(Settings, Input, Camera.Main);
            Camera.OnAwake(this);
        }
    }
}
