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
            float z = Mathf.InverseLerp(0f, 15f, Math.Abs(velocity.y));
            float m = Mathf.Lerp(20f, 0f, z);
Debug.Log("z in InverseLerp: " + z);
Debug.Log("m in InverseLerp: " + m);
            z += z * m;
Debug.Log(z);
            if (velocity.y == 0)
            {
                z = 0;
            }
            else if (velocity.y < 0)
            {
                z = Mathf.Lerp(0, -45, z);
            }
            else
            {
                z = Mathf.Lerp(0, 45, z);
            }

            float y = Mathf.Clamp(velocity.z * VelocityMultiplier.y, YByZLimit.x, YByZLimit.y);
            // float z = Mathf.Clamp(velocity.y * VelocityMultiplier.z, ZByYLimit.x, ZByYLimit.y);

            Rotate(TargetLeft, Quaternion.Euler(0, -Offset.y + -y, -Offset.z + z));
            Rotate(TargetRight, Quaternion.Euler(0, Offset.y + y, Offset.z + -z));
        }
    }
}
