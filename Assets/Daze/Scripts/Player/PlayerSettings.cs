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

        /// <summary>
        /// The speed when the character is running. You can control how fast
        /// it reaches the target speed by `GroundMovementSharpness`.
        /// </summary>
        public float GroundRunSpeed = 10f;

        /// <summary>
        /// The treshold until the character starts running. This is the
        /// character input value and uses `sqrMagnitude` of input vector.
        /// The value is always between 0 and 1.
        /// </summary>
        public float GroundRunMagnitudeTreshold = 0.2f;

        /// <summary>
        /// How sharp will character accelerate to the target speed. This value
        /// is used to calculate smooth factor for `Lerp` function.
        /// </summary>
        public float GroundMovementSharpness = 15f;

        /// <summary>
        /// Same as `GroundMovementSharpness` but for character rotation.
        /// </summary>
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

        public float LiftAcceleration = 0.08f;
        public float LiftTime = 0.8f;
        public float LiftBrakeSpeed = 1f;

        // ---------------------------------------------------------------------
        // Drift Movement
        // ---------------------------------------------------------------------

        [Header("Drift Movement")]

        public float DriftEnterMagnitude = 0.1f;
        public float DriftVAmplitude = 0.25f;
        public float DriftVFrequency = 0.9f;
        public float DriftHAmplitude = 0.1f;
        public float DriftHFrequency = 0.45f;

        // ---------------------------------------------------------------------
        // Fall Movement
        // ---------------------------------------------------------------------

        [Header("Fall Movement")]

        public float FallAcceleration = 5f;
        public float MaxFallSpeed = 15f;
        public float FallBrakeSpeed = 10f;
    }
}
