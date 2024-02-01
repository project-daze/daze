using System;
using UnityEngine;
using Cinemachine;
using Daze.Player.Support;

namespace Daze.Player.Camera
{
    public class CameraController : MonoBehaviour
    {
        [NonSerialized] public PlayerSettings Settings;
        [NonSerialized] public PlayerInput Input;
        [NonSerialized] public PlayerState State;

        public Transform Main;
        public CinemachineFreeLook Look;
        public CinemachineVirtualCamera Free;
        [NonSerialized] public CinemachineTransposer FreeTransposer;

        public Transform LookFollowTarget;
        public Transform FreeFollowTarget;
        public Transform Avatar;

        private float _freeDamping = 0f;
        private float _freeFollowOffset = 0f;

        public void OnAwake(PlayerSettings settings, PlayerInput input, PlayerState state)
        {
            Settings = settings;
            Input = input;
            State = state;

            FreeTransposer = Free.GetCinemachineComponent<CinemachineTransposer>();
        }

        public void LateUpdate()
        {
            HandleTransition();
            SyncFollowTargetPosition();
            UpdateRotation();
            UpdateDampings();
            UpdateFollowOffset();
        }

        /// <summary>
        /// If player falling state and active camera do not match, start
        /// transitioning to the correct camera.
        /// </summary>
        private void HandleTransition()
        {
            if (ShouldTransition())
            {
                ResetDampingValue();
                Look.gameObject.SetActive(!Look.gameObject.activeSelf);
            }
        }

        private bool IsLookActive()
        {
            return Look.gameObject.activeSelf;
        }

        private bool ShouldTransition()
        {
            if (State.IsFloating && IsLookActive()) return true;
            if (!State.IsFloating && !IsLookActive()) return true;
            return false;
        }

        private void SyncFollowTargetPosition()
        {
            LookFollowTarget.transform.position = Avatar.position;
            FreeFollowTarget.transform.position = Avatar.position;
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
            if (Input.LookComposite == Vector2.zero) return;

            float x = -Input.LookComposite.y * Settings.FreeCameraRotationSpeed * Time.deltaTime;
            float y = Input.LookComposite.x * Settings.FreeCameraRotationSpeed * Time.deltaTime;

            FreeFollowTarget.Rotate(x, y, 0f);
        }

        private void UpdateDampings()
        {
            UpdateDampingValue();
            FreeTransposer.m_XDamping = _freeDamping;
            FreeTransposer.m_YDamping = _freeDamping;
            FreeTransposer.m_ZDamping = _freeDamping;
        }

        private void UpdateDampingValue()
        {
            if (ShouldDamp())
            {
                AddDampingValue();
            }
            else
            {
                RemoveDampingValue();
            }
        }

        private void AddDampingValue()
        {
            _freeDamping = _freeDamping < 4.9f
                ? Mathf.Lerp(_freeDamping, 5f, 1f * Time.deltaTime)
                : 5f;
        }

        private void RemoveDampingValue()
        {
            _freeDamping = _freeDamping > 0.01f
                ? Mathf.Lerp(_freeDamping, 0f, 5f * Time.deltaTime)
                : 0f;
        }

        private void ResetDampingValue()
        {
            _freeDamping = 0f;
        }

        /// <summary>
        /// Check if the free camera should add dampings. The damping should
        /// be added when the character is on hover state.
        /// </summary>
        private bool ShouldDamp()
        {
            return State.IsFloating && State.IsHovering;
        }

        private void UpdateFollowOffset()
        {
            UpdateFollowOffsetValue();
            FreeTransposer.m_FollowOffset.z = _freeFollowOffset;
        }

        private void UpdateFollowOffsetValue()
        {
            // If the camera should not offset, reduce the offset to its
            // default value.
            if (!ShouldOffset())
            {
                ReduceFollowOffset();
                return;
            }

            // Otherwise, offset camera based on the fall speed.
            _freeFollowOffset = Utils.LinearMap(
                State.FallSpeed,
                Settings.FreeCameraOffsetMinFallSpeed,
                Settings.MaxFallSpeed,
                Settings.FreeCameraMinOffset,
                Settings.FreeCameraMaxOffset
            );

            _freeFollowOffset = Mathf.Lerp(
                _freeFollowOffset,
                Settings.FreeCameraMaxOffset,
                Settings.FreeCameraOffsetTransitionSpeed * Time.deltaTime
            );
        }

        private void ReduceFollowOffset()
        {
            _freeFollowOffset = _freeFollowOffset < (Settings.FreeCameraMinOffset + -0.01f)
                ? Mathf.Lerp(_freeFollowOffset, Settings.FreeCameraMinOffset, Settings.FreeCameraOffsetReduceSpeed * Time.deltaTime)
                : Settings.FreeCameraMinOffset;
        }

        private bool ShouldOffset()
        {
            return State.IsFloating
                && (State.FallSpeed > Settings.FreeCameraOffsetMinFallSpeed);
        }
    }
}
