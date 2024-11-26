using System;
using UnityEngine;

namespace Daze.Player.Camera
{
    /// <summary>
    /// The `AirCameraTargetController` is responsible for updating the air
    /// camera target to achieve full 360 degree camera rotation.
    /// </summary>
    public class AirCameraTargetController : MonoBehaviour
    {
        [NonSerialized] public PlayerInput Input;

        [NonSerialized] public Transform MainCamera;
        [NonSerialized] public CameraState CameraState;

        /// <summary>
        /// The target (usually the avatar) to follow.
        /// </summary>
        public Transform Target;

        /// <summary>
        /// the rotation speed of the camera.
        /// </summary>
        public float RotationSpeed = 200f;

        /// <summary>
        /// Awake hook to set the player state from the parent.
        /// </summary>
        public void OnAwake(PlayerInput input, Transform mainCamera, CameraState cameraState)
        {
            Input = input;
            MainCamera = mainCamera;
            CameraState = cameraState;
        }

        /// <summary>
        /// Update the camera target's position and rotation. Doing this in
        /// `LateUpdate` to make sure the camera target because otherwise the
        /// camera movement becomes jittery. I don't know why.
        /// </summary>
        public void LateUpdate()
        {
            UpdateRotation();
            UpdatePosition();
        }

        /// <summary>
        /// Update the camera target's rotation. The air camera will be locked
        /// to this target and will rotate around when the object rotates.
        /// </summary>
        private void UpdateRotation()
        {
            // When ground camera is active, sync camera target rotation with
            // the main camera rotation to get smooth camera transition.
            if (CameraState.IsGroundCameraActive)
            {
                transform.rotation = MainCamera.rotation;
                return;
            }

            // Else, that means the air camera is active. So, rotate the camera
            // based on the player's input.
            if (Input.LookComposite == Vector2.zero)
            {
                return;
            }

            float x = -Input.LookComposite.y * RotationSpeed * Time.deltaTime;
            float y = Input.LookComposite.x * RotationSpeed * Time.deltaTime;

            transform.Rotate(x, y, 0f);
        }

        /// <summary>
        /// Update the camera target's position to sync with the target so that
        /// the camera target is always at the target's position.
        private void UpdatePosition()
        {
            transform.position = Target.position;
        }
    }
}
