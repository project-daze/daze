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
        private Vector3 LastTargetLocalPosition;

        private float TimeSinceLastTargetStopped;

        private Vector3 LastVelocity;

        private float LastDistance;

        private Vector3 VVV = Vector3.zero;

        /// <summary>
        /// Initialize the target position on start.
        /// </summary>
        public void Start()
        {
            LastVelocity = Vector3.zero;
            LastTargetLocalPosition = Target.localPosition;
        }

private Vector3 previousTargetPosition;
private Vector3 smoothedTargetVelocity;

public float BlendFactor = 0.5f;
public float CurrentMove = 0f;
public float MoveFactor = 0.5f;
public float ApproachSpeed = 0.5f;

public float CurrentDistance = 0f;

public float DampDistance = 0.5f;

private float PV = 0.0001f;

        /// <summary>
        /// Update the camera target's position and rotation. Doing this in
        /// `LateUpdate` to make sure the camera target because otherwise the
        /// camera movement becomes jittery. I don't know why.
        /// </summary>
        public void LateUpdate()
        {
            // Calculate Target's movement vector since the last frame.
            Vector3 targetMovement = Target.position - LastTargetLocalPosition;

            // Get the surface normal of the target.
            Vector3 surfaceNormal = transform.up;

            // Project Target's movement onto the ground plane and extract
            // the vertical movement.
            Vector3 groundMovement = Vector3.ProjectOnPlane(targetMovement, surfaceNormal);
            Vector3 verticalMovement = Vector3.Project(targetMovement, surfaceNormal);

CurrentMove = verticalMovement.magnitude;

            float distance = Vector3.Distance(Vector3.Project(Target.position, surfaceNormal), Vector3.Project(transform.position, surfaceNormal));
CurrentDistance = distance;

            Vector3 reducedVerticalMovement = verticalMovement * MoveFactor;

            float newDistance = Vector3.Distance(Vector3.Project(Target.position, surfaceNormal), Vector3.Project(transform.position, surfaceNormal) + reducedVerticalMovement);

            // float am;
            // if (newDistance > distance)
            // {
            //     reducedVerticalMovement = Vector3.zero;
            //     Damping = 1f;
            // }
            // else
            // {
            //     Damping = 0f;
            // }

            Vector3 mmm = (Vector3.Project(Target.position, surfaceNormal) - Vector3.Project(transform.position, surfaceNormal)) * Damping * Time.deltaTime;

            // float approachSpeed = Mathf.Lerp(0, ApproachSpeed * am, distance / DampDistance);
            // Vector3 approachDirection = (Vector3.Project(Target.position, surfaceNormal) - Vector3.Project(transform.position, surfaceNormal)).normalized * approachSpeed;

            float blend = Mathf.Clamp01(verticalMovement.magnitude / 0.04f);
            verticalMovement = Vector3.Lerp(mmm, reducedVerticalMovement, blend);

            // Combine ground and scaled vertical movement then apply to the
            // object position.
            transform.position += groundMovement + verticalMovement ;

            // Update the previous position of B for the next frame.
            LastTargetLocalPosition = Target.position;
            LastVelocity = verticalMovement;
            LastDistance = distance;
        }

            // if (scaledVerticalMovement.magnitude > 0.001f)
            // {
            //     verticalMovement = scaledVerticalMovement;
            // }
            // else
            // {
            //     Vector3 m = (Vector3.Project(Target.position, surfaceNormal) - Vector3.Project(transform.position, surfaceNormal)) * (Damping * Time.deltaTime);

            //     // verticalMovement = Vector3.Lerp(LastVelocity, m, 0.5f * Time.deltaTime);
            // }

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
