using System;
using Cinemachine;
using UnityEngine;

namespace Daze.Camera
{
    public class CameraControl : MonoBehaviour
    {
        public global::Player Player;
        [NonSerialized] public CinemachineBrain Brain;
        public Transform MainCamera;
        public CinemachineFreeLook LookCamera;
        public CinemachineVirtualCamera FreeCamera;
        [NonSerialized] public CinemachineBasicMultiChannelPerlin FreeCameraNoise;
        public FreeCameraControl FreeCameraControl;

        public float PlayerMagnitude = 0f;
        public float PlayerMaxMagnitude = 24f;

        public float NoiseAmplitude = 0f;
        public float NoiseMaxAmplitude = 0.25f;
        public float NoiseFrequency = 0f;
        public float NoiseMaxFrequency = 0.1f;

        private void Awake()
        {
            Brain = GetComponentInChildren<CinemachineBrain>();
            FreeCameraNoise = FreeCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();

            Player.OnGrounded += StartTransitionToLookCamera;
            Player.OnDiving += SetPlayerMagnitude;
            Player.OnHover += StartTransitionToFreeCamera;
            Player.OnDiveMove += SetDiveMoveDirection;

            FreeCameraControl.OnTransitionedFrom += TransitionToFreeCamera;
            FreeCameraControl.OnTransitionedTo += TransitionToLookCamera;
        }

        private void FixedUpdate()
        {
            if (!FreeCamera.gameObject.activeSelf) return;

            NoiseAmplitude = MapRange(
                PlayerMagnitude,
                0f, PlayerMaxMagnitude,
                0f, NoiseMaxAmplitude
            );

            NoiseFrequency = MapRange(
                PlayerMagnitude,
                0f, PlayerMaxMagnitude,
                0f, NoiseMaxFrequency
            );

            FreeCameraNoise.m_AmplitudeGain = NoiseAmplitude;
            FreeCameraNoise.m_FrequencyGain = NoiseFrequency;
        }

        private void StartTransitionToLookCamera()
        {
            FreeCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
            FreeCameraControl.Deactivate();
            FreeCameraControl.TransitionTo(Player.transform);
        }

        private void TransitionToLookCamera()
        {
            FreeCameraNoise.m_AmplitudeGain = 0f;
            FreeCameraNoise.m_FrequencyGain = 0f;
            NoiseAmplitude = 0f;
            NoiseFrequency = 0f;
            LookCamera.gameObject.SetActive(true);
        }

        private void StartTransitionToFreeCamera()
        {
            FreeCameraControl.Activate();
            FreeCameraControl.TransitionFrom(MainCamera.transform);
        }

        private void TransitionToFreeCamera()
        {
            LookCamera.gameObject.SetActive(false);
        }

        private void SetPlayerMagnitude(float magnitude)
        {
            PlayerMagnitude = magnitude;
        }

        private float MapRange(float valueA, float minA, float maxA, float minB, float maxB)
        {
            // Ensure valueA is within the range of minA and maxA.
            valueA = Math.Max(minA, Math.Min(maxA, valueA));

            // Calculate the corresponding value in the range of minB and maxB.
            float ratio = (valueA - minA) / (maxA - minA);
            float valueB = minB + (maxB - minB) * ratio;

            return valueB;
        }

        private void SetDiveMoveDirection(float direction)
        {
            FreeCameraControl.SetDiveMoveDirection(direction);
        }
    }
}
