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

        public AvatarController Avatar;
        public CameraController Camera;

        public void Awake()
        {
            Avatar.OnAwake(Settings, Input, Camera.MainCamera);
            Camera.OnAwake(Settings, Input, State);

            Avatar.OnLanded += () => State.IsJumping = false;
            Avatar.OnJumped += () => State.IsJumping = true;

            Avatar.EnterFloatingState += () => State.IsFloating = true;
            Avatar.LeaveFloatingState += () => State.IsFloating = false;
            Avatar.EnterFallingState += () => State.IsFalling = true;
            Avatar.LeaveFallingState += () => State.IsFalling = false;
            Avatar.EnterHoveringState += () => State.IsHovering = true;
            Avatar.LeaveHoveringState += () => State.IsHovering = false;

            Avatar.FallSpeedUpdated += (speed) => State.FallSpeed = speed;
        }
    }
}
