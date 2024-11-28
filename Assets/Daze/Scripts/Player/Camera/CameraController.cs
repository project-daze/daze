using System;
using UnityEngine;
using Unity.Cinemachine;

namespace Daze.Player.Camera
{
    public class CameraController : MonoBehaviour
    {
        [NonSerialized] public PlayerSettings Settings;
        [NonSerialized] public PlayerInput Input;
        [NonSerialized] public PlayerState State;

        public Transform MainCamera;
        public CinemachineCamera GroundCamera;
        public GroundCameraTargetController GroundCameraTargetController;
        public CinemachineCamera AirCamera;
        public AirCameraTargetController AirCameraTargetController;

        public CameraState CameraState;

        /// <summary>
        /// Awake hook to set the player cofigs from the parent.
        /// </summary>
        public void OnAwake(PlayerSettings settings, PlayerInput input, PlayerState state)
        {
            Settings = settings;
            Input = input;
            State = state;

            CameraState.IsGroundCameraActive = IsGroundCameraActive();

            GroundCameraTargetController.OnAwake(State);
            AirCameraTargetController.OnAwake(Input, MainCamera, CameraState);
        }

        /// <summary>
        /// Update the camera states.
        /// </summary>
        public void LateUpdate()
        {
            HandleTransition();
        }

        /// <summary>
        /// Check if the ground camera is active.
        /// </summary>
        private bool IsGroundCameraActive()
        {
            return GroundCamera.isActiveAndEnabled;
        }

        /// <summary>
        /// Check if the camera should be transitioning.
        /// </summary>
        private bool ShouldTransition()
        {
            return (State.IsFloating && IsGroundCameraActive())
                || (!State.IsFloating && !IsGroundCameraActive());
        }

        /// <summary>
        /// Handle the camera transition between the ground and air camera.
        /// </summary>
        private void HandleTransition()
        {
            if (ShouldTransition())
            {
                GroundCamera.gameObject.SetActive(!GroundCamera.gameObject.activeSelf);
                CameraState.IsGroundCameraActive = !CameraState.IsGroundCameraActive;
            }
        }
    }
}
