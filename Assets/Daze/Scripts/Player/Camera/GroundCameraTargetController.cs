using UnityEngine;

namespace Daze.Player.Camera
{
    /// <summary>
    /// This class is responsible for controlling the object that ground camera
    /// targets. It will follow specified target (usually the player's avatar)
    /// but with damping on Y axis to make the camera movement match GR.
    /// </summary>
    public class GroundCameraTargetController : MonoBehaviour
    {
        /// <summary>
        /// The main camera that is used to check if the target is going out
        /// of the screen.
        /// </summary>
        [Tooltip("The main camera that is used to check if the target is going out of the screen.")]
        public UnityEngine.Camera Camera;

        /// <summary>
        /// The target (usually the avatar) to follow.
        /// </summary>
        [Tooltip("The target to follow.")]
        public Transform Target;

        /// <summary>
        /// The target's head position. This is used to check if the target is
        /// going out of the screen.
        /// </summary>
        [Tooltip("The target's head position.")]
        public Transform TargetHead;

        /// <summary>
        /// The target's feet position. This is used to check if the target is
        /// going out of the screen.
        /// </summary>
        [Tooltip("The target's feet position.")]
        public Transform TargetFeet;

        /// <summary>
        /// The damping factor for the camera movement on Y axis.
        /// </summary>
        [Tooltip("The damping factor for the camera movement on Y axis.")]
        public float Damping = 5f;

        /// <summary>
        /// The padding relative to the screen on Y axis that is treated as
        /// hard limit. If the target goes out of this limit, the camera
        /// target will accelerate to follow the target.
        /// </summary>
        [Tooltip("The hard limit area on Y axis relative to the screen.")]
        public float HardLimitY = 32;

        /// <summary>
        /// The target's current speed on Y axis. This is used to move camera
        /// target at the same speed as the target when the target is going out
        /// of the screen.
        /// </summary>
        private float TargetLocalVelocityY;

        /// <summary>
        /// The target's last local Y position . This is used to calculate the
        /// target's current speed which is stored in `TargetYSpeed`.
        /// </summary>
        private float LastTargetLocalPositionY;

        private float TimeSinceLastTargetStopped;

        /// <summary>
        /// Initialize the target position on start.
        /// </summary>
        public void Start()
        {
            LastTargetLocalPositionY = Target.localPosition.y;
        }

        /// <summary>
        /// Update the camera target's position and rotation. Doing this in
        /// `LateUpdate` to make sure the camera target because otherwise the
        /// camera movement becomes jittery. I don't know why.
        /// </summary>
        public void LateUpdate()
        {
            UpdateTargetSpeedAndPosition();
            UpdatePosition();
        }

        /// <summary>
        /// Update the target's speed and position.
        /// </summary>
        private void UpdateTargetSpeedAndPosition()
        {
            float targetVelocity = (Target.localPosition.y - LastTargetLocalPositionY) / Time.deltaTime;

            TargetLocalVelocityY = Mathf.Abs(targetVelocity) < 0.1f ? 0 : targetVelocity;

            LastTargetLocalPositionY = Target.localPosition.y;
        }

        /// <summary>
        /// Update the camera target's position to sync with the target. But,
        /// apply damping on local Y axis to make the camera movement match GR.
        /// </summary>
        private void UpdatePosition()
        {
            Vector3 newLocalPosition = transform.localPosition;

            newLocalPosition.x = Target.localPosition.x;
            newLocalPosition.z = Target.localPosition.z;

            newLocalPosition.y = GetNewLocalPositionY(newLocalPosition);

            transform.localPosition = newLocalPosition;
        }

        /// <summary>
        /// Set the Y axis position on given new local position.
        /// </summary>
        private float GetNewLocalPositionY(Vector3 localPosition)
        {
            float s = (Target.localPosition.y - transform.localPosition.y) * (Damping * Time.deltaTime);
            float ss = Mathf.Clamp(s, -0.5f, 0.5f);

            float sss = TargetLocalVelocityY * 0.2f * Time.deltaTime;
Debug.Log("s: " + s + ", ss: " + ss + ", sss: " + sss);
            if (Mathf.Abs(sss) > Mathf.Abs(ss))
            {
                return localPosition.y + sss;
            }

            return localPosition.y + ss;



            // If the target is in the safe zone, move toward the target
            // with damping to get smooth transition.
            if (IsTargetInSafeZone())
            {
                return Mathf.Lerp(transform.localPosition.y, Target.localPosition.y, Damping * Time.deltaTime);
            }

            // If the target is going out of the screen, we need move the
            // camera faster to catch up with the target.
            //
            // When the target is moving, we use the same speed as the target
            // to move the camera target.
            //
            // But if the target is not moving, or moving slowly, we use the
            // minimam speed to move the camera target closer. Because when the
            // target is very close to the camera, the target might be outiside
            // of the screen, but not moving in any direction.
            return Mathf.Abs(TargetLocalVelocityY) > 1f
                ? localPosition.y + TargetLocalVelocityY * Time.deltaTime
                : Mathf.Lerp(transform.localPosition.y, Target.localPosition.y, Damping * Time.deltaTime);
        }

        /// <summary>
        /// Check if the target is inside the hard limit area on Y axis.
        /// </summary>
        private bool IsTargetInSafeZone()
        {
            Vector3 headScreenPosition = Camera.WorldToScreenPoint(TargetHead.position);
            Vector3 feetScreenPosition = Camera.WorldToScreenPoint(TargetFeet.position);

            float hardLimitTop = Screen.height - HardLimitY;
            float hardLimitBottom = HardLimitY;

            if (
                headScreenPosition.y > hardLimitTop
                || headScreenPosition.y < hardLimitBottom
                || feetScreenPosition.y > hardLimitTop
                || feetScreenPosition.y < hardLimitBottom
            )
            {
                return false;
            }

            return true;
        }
    }
}
