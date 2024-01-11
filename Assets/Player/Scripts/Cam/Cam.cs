using System;
using UnityEngine;
using Cinemachine;

namespace Daze.Player
{
    public class Cam : MonoBehaviour
    {
        [NonSerialized] public Controller Controller;

        public Transform Main;
        public CinemachineFreeLook Look;
        public CinemachineVirtualCamera Free;

        public Transform LookFollowTarget;
        public Transform FreeFollowTarget;

        public float FreeCameraRotationSpeed = 200f;

        public void OnAwake(Controller controller)
        {
            Controller = controller;
        }

        public void LateUpdate()
        {
            // If player falling state and active camera do not match, start
            // transitioning to the correct camera.
            if (ShouldTransition())
            {
                Look.gameObject.SetActive(!Look.gameObject.activeSelf);
            }

            // Sync follow target position with the player position.
            LookFollowTarget.transform.position = Controller.Character.transform.position;
            FreeFollowTarget.transform.position = Controller.Character.transform.position;

            // Update camera rotation.
            UpdateRotation();
        }

        private bool IsLookActive()
        {
            return Look.gameObject.activeSelf;
        }

        private bool ShouldTransition()
        {
            if (Controller.IsFalling && IsLookActive()) return true;
            if (!Controller.IsFalling && !IsLookActive()) return true;
            return false;
        }

        /// <summary>
        /// Update free camera rotation by rotating follow target. This
        /// rotation is only required for the free camera.
        /// </summary>
        private void UpdateRotation()
        {
            // When look camera is active, sync follow target for Free camera
            // rotation with the main camera (which is Look camera view).
            // This will make transitioning between cameras look smooth.
            if (IsLookActive())
            {
                FreeFollowTarget.rotation = Main.transform.rotation;
                return;
            }

            // Else, it means the Free camera is active. Update its rotation
            // using the player's input.
            if (Controller.Input.LookComposite == Vector2.zero) return;

            float x = -Controller.Input.LookComposite.y * FreeCameraRotationSpeed * Time.deltaTime;
            float y = Controller.Input.LookComposite.x * FreeCameraRotationSpeed * Time.deltaTime;

            FreeFollowTarget.Rotate(x, y, 0f);
        }
    }
}
