using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.Animations.Rigging;

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
        public float Damping = 1f;

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
        private Vector3 LastTargetPosition;

        private Vector3 LastPosition;

        private float TimeSinceLastTargetStopped = 0f;

        private Vector3 LastVelocity;

        private float LastDistance;

        private Vector3 VVV = Vector3.zero;

        /// <summary>
        /// Initialize the target position on start.
        /// </summary>
        public void Start()
        {
            LastVelocity = Vector3.zero;
            LastPosition = Vector3.Project(Target.position, transform.up);
            LastTargetPosition = Target.position;
        }

public float BlendFactor = 0.5f;
public float CurrentMove = 0f;
public float MoveFactor = 0.5f;
public float ApproachSpeed = 0.5f;

public float CurrentDistance = 0f;

public float DampDistance = 0.5f;

public float maxSpeed = 0f;
public float minSpeed = 0f;
public float maxFollowSpeed = 0f;

public float MoveAheadAwayDistance = -2f;
public float MoveAheadCloserDistance = 2f;
public float MoveAheadDistanceSpeedFactor = 15f;

        /// <summary>
        /// Update the camera target's position and rotation. Doing this in
        /// `LateUpdate` to make sure the camera target because otherwise the
        /// camera movement becomes jittery. I don't know why.
        /// </summary>
        public void LateUpdate()
        {
            // Calculate Target's velocity by comparing to the ast frame.
            Vector3 targetVelocity = (Target.position - LastTargetPosition) / Time.deltaTime;

            // Get the current surface normal. This can be current "up" because
            // the camera target is going to be rotated when the target is
            // standing on a wall.
            Vector3 surfaceNormal = transform.up;

            // Get target's ground position (x and z axis). This value will be
            // applied to the camera target's position directly since we don't
            // add any damping on x and z axis movement.
            Vector3 targetGroundPosition = Vector3.ProjectOnPlane(Target.position, surfaceNormal);

            // Get the vertical position, and the velocity of the vertical
            // movement of the target.
            Vector3 verticalPosition = Vector3.Project(transform.position, surfaceNormal);
            Vector3 targetVerticalPosition = Vector3.Project(Target.position, surfaceNormal);
            Vector3 targetVerticalVelocity = Vector3.Project(targetVelocity, surfaceNormal);

            if (maxSpeed < targetVerticalVelocity.magnitude) {
                maxSpeed = targetVerticalVelocity.magnitude;
            }
            if (minSpeed > targetVerticalVelocity.magnitude) {
                minSpeed = targetVerticalVelocity.magnitude;
            }

            // Get the distance between the target's vertical position and the
            // camera target's vertical position. Use this to determine if the
            // target is moving away from the camera target or closer to it.
            // float distance = Vector3.Distance(verticalPosition, targetVerticalPosition);

            // If the target is moving on vertical axis, move the camera
            // target a bit further to make the camera movement smoother.
            // Vector3 newVerticalPosition = verticalPosition + targetVerticalVelocity * 0.2f;

            // if (targetVerticalVelocity.magnitude > 0.01f)
            // {
                // float moveAheadDistance = distance >= LastDistance ? MoveAheadAwayDistance : MoveAheadCloserDistance;
                // float dynamicLeadDistance = Mathf.Clamp(targetVerticalVelocity.magnitude / MoveAheadDistanceSpeedFactor * moveAheadDistance, 0, moveAheadDistance);

                // newTargetVerticalPosition = targetVerticalPosition + targetVerticalVelocity.normalized * dynamicLeadDistance;
            // }

            // Move the vertical position toward the target's vertical position
            // with damping to make the camera movement smooth.
            Vector3 newVerticalPosition = Vector3.Lerp(
                verticalPosition,
                verticalPosition + targetVerticalVelocity * 0.2f,
                Damping * Time.deltaTime
            );

            // Vector3 movementVector = newVerticalPosition - verticalPosition;
            // float moveDistance = movementVector.magnitude;
            // float maxDistanceThisFrame = 0.6f * Time.deltaTime;

            // if (maxFollowSpeed < moveDistance) {
            //     maxFollowSpeed = moveDistance;
            // }

            // if (moveDistance > maxDistanceThisFrame) {
            //     newVerticalPosition = verticalPosition + movementVector.normalized * maxDistanceThisFrame;
            // }
            // else
            // {
            //     newVerticalPosition = verticalPosition + movementVector;
            // }

            // if (targetVerticalVelocity.magnitude > 0.01f)
            // {
                // if (distance >= LastDistance)
                // {
                //     LastPosition += targetVerticalVelocity * 0.5f * Time.deltaTime;
                //     newVerticalPosition = Vector3.Lerp(
                //         verticalPosition,
                //         LastPosition,
                //         Damping * Time.deltaTime
                //     );
                // }
                // else
                // {
                //     LastPosition += targetVerticalVelocity * 0.5f * Time.deltaTime;
                //     newVerticalPosition = Vector3.Lerp(
                //         verticalPosition,
                //         LastPosition,
                //         Damping * Time.deltaTime
                //     );
                // }
            // }
            // else if (TimeSinceLastTargetStopped > 0.1f)
            // {
            //     newVerticalPosition = Vector3.Lerp(
            //         verticalPosition,
            //         targetVerticalPosition,
            //         Damping * Time.deltaTime
            //     );
            // }

            // Combine ground and calculated vertical position then apply to
            // the object position.
            transform.position = targetGroundPosition + newVerticalPosition;

            // Record target's position and distance for the next frame.
            // LastPosition = newVerticalPosition;
            LastTargetPosition = Target.position;
            LastVelocity = targetVerticalPosition;
            // LastDistance = distance;
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
