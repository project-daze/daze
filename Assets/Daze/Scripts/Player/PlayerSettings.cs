using UnityEngine;

namespace Daze.Player
{
    public class PlayerSettings : MonoBehaviour {
        // ---------------------------------------------------------------------
        // Gravity
        // ---------------------------------------------------------------------

        [Header("Gravity")]

        public Vector3 Gravity = new(0, -15f, 0);

        // ---------------------------------------------------------------------
        // Ground Movement
        // ---------------------------------------------------------------------

        [Header("Ground Movement")]

        public float MaxGroundMoveSpeed = 10f;
        public float GroundMovementSharpness = 15f;
        public float GroundOrientationSharpness = 15f;

        // ---------------------------------------------------------------------
        // Air Movement
        // ---------------------------------------------------------------------

        [Header("Air Movement")]
        public float MaxAirMoveSpeed = 5f;
        public float AirAccelerationSpeed = 5f;
        public float AirDrag = 0.5f;

        // ---------------------------------------------------------------------
        // Jump Movement
        // ---------------------------------------------------------------------

        [Header("Jump Movement")]

        public float JumpSpeed = 8f;

        /// <summary>
        /// The extra time before landing where you can press jump and still
        /// jump once you land.
        /// </summary>
        public float JumpPreGroundingGraceTime = 0f;

        /// <summary>
        /// The extra time after leaving stable ground where you will still
        /// be allowed to jump.
        /// </summary>
        public float JumpPostGroundingGraceTime = 0f;

        public bool AllowJumpingWhenSliding = false;

        // ---------------------------------------------------------------------
        // Lift Movement
        // ---------------------------------------------------------------------

        [Header("Lift Movement")]

        public float LiftAcceleration = 0.1f;
        public float LiftTime = 1f;

        // ---------------------------------------------------------------------
        // Drift Movement
        // ---------------------------------------------------------------------

        [Header("Drift Movement")]

        public float DriftVAmplitude = 0.25f;
        public float DriftVFrequency = 2.5f;
        public float DriftHAmplitude = 0.125f;
        public float DriftHFrequency = 0.625f;

        // ---------------------------------------------------------------------
        // Camera Settings
        // ---------------------------------------------------------------------

        [Header("Free Camera Settings")]

        public float FreeCameraRotationSpeed = 200f;
    }
}
