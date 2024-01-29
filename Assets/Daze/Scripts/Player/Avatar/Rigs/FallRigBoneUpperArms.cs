using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations.Rigging;
using Daze.Player.Support;
using System;

namespace Daze.Player.Avatar.Rigs
{
    public class FallRigBoneUpperArms : FallRigBone
    {
        public OverrideTransform RigLeft;
        public OverrideTransform RigRight;
        public Transform TargetLeft;
        public Transform TargetRight;

        public Vector2 YByZLimit;
        public Vector2 ZByYLimit;

        protected override List<OverrideTransform> Rigs()
        {
            return new List<OverrideTransform> { RigLeft, RigRight };
        }

        protected override List<Transform> Targets()
        {
            return new List<Transform> { TargetLeft, TargetRight };
        }

        protected override void DoControl(Vector3 velocity)
        {
//             float z = Mathf.InverseLerp(0f, 15f, Mathf.Abs(velocity.y));
//             float m = Mathf.Lerp(z, 1f, 0.5f);
// Debug.Log("z in InverseLerp: " + z);
// Debug.Log("m in InverseLerp: " + m);
//             z = m;
//             //     ? (z * m) > 1 ? 1 : (z * m)
//             //     : (z * 1.01f);
// Debug.Log(z);
//             if (velocity.y == 0)
//             {
//                 z = 0;
//             }
//             else if (velocity.y < 0)
//             {
//                 z = Mathf.Lerp(0, -45, z);
//             }
//             else
//             {
//                 z = Mathf.Lerp(0, 45, z);
//             }

            float ym = Mathf.Clamp(Mathf.Abs(velocity.y * 40), 0, 45);

            float c = Mathf.Lerp(
                ym,
                45, // 45 / 15
                0.01f
            );
Debug.Log("Y: " + ym);
Debug.Log("C: " + c);
            float z = c * velocity.normalized.y;
Debug.Log("Z: " + z);
            z = Mathf.Clamp(z, ZByYLimit.x, ZByYLimit.y);


        // Map velocityY to a value between 0 and 1
//             float normalizedVelocity = Mathf.Clamp01(Mathf.Abs(velocity.y) * 40 / 600f);

//             // Apply non-linear mapping to give more sensitivity to lower values
//             float mappedValueB = Mathf.Pow(normalizedVelocity, 2) * 45;
// Debug.Log("mb1: " + mappedValueB);
//             // Apply damping factor as velocityY gets closer to 15
//             mappedValueB *= 1 - Mathf.Pow((600f - mappedValueB) / 600f, 2);
// Debug.Log("mb2: " + mappedValueB);
//             float z = mappedValueB * velocity.normalized.y;
// Debug.Log("z: " + z);
            float y = Mathf.Clamp(velocity.z * VelocityMultiplier.y, YByZLimit.x, YByZLimit.y);
            // float z = Mathf.Clamp(velocity.y * VelocityMultiplier.z, ZByYLimit.x, ZByYLimit.y);

            // TargetLeft.localRotation = Quaternion.Euler(0, 0, z);

        // Calculate a dynamic minValueB based on velocityY
//         float minValueB = Mathf.Lerp(45, 0f, Mathf.Abs(velocity.y) / 15f);

//         // Map Velocity Y to Value B using an exponential function
//         float mappedValueB = minValueB + (45 - minValueB) * Mathf.Pow((Mathf.Abs(velocity.y) / 15f), 2f);

//         // Ensure that the mappedValueB stays within the specified range
//         float z = mappedValueB * velocity.normalized.y;

// Debug.Log(z);

            Rotate(TargetLeft, Quaternion.Euler(0, -Offset.y + -y, -Offset.z + z));
            Rotate(TargetRight, Quaternion.Euler(0, Offset.y + y, Offset.z + -z));
        }
    }
}
