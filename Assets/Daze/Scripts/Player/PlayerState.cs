using UnityEngine;

namespace Daze.Player
{
    /// <summary>
    /// `PlayerState` is the state shared between multiple components of the
    /// player. For example, the `AvatarController` and the `CameraController`
    /// both need to know the player's gravity, movement speed, etc.
    ///
    /// The states that are scoped to specific component, should be defined in
    /// that component instead of listed here.
    /// </summary>
    public class PlayerState : MonoBehaviour {
        public bool IsFloating = false;
        public bool IsHovering = false;
        public bool IsFalling = false;

        public float FallSpeed = 0f;
    }
}
