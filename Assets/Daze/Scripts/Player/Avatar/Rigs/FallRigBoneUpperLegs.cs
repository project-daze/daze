using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations.Rigging;

namespace Daze.Player.Avatar.Rigs
{
    public class FallRigBoneUpperLegs : FallRigBone
    {
        public OverrideTransform RigLeft;
        public OverrideTransform RigRight;
        public Transform TargetLeft;
        public Transform TargetRight;

        public Vector2 XByYLimit;
        public Vector2 XByZLimit;
        public Vector2 XTotalLimit;

        public Vector2 ZByYLimit;

        protected List<OverrideTransform> Rigs()
        {
            return new List<OverrideTransform> { RigLeft, RigRight };
        }

        protected List<Transform> Targets()
        {
            return new List<Transform> { TargetLeft, TargetRight };
        }

        protected void DoControl(Vector3 velocity)
        {
            // float xByY = Mathf.Clamp(velocity.y * VelocityMultiplier.x, XByYLimit.x, XByYLimit.y);
            // float xByZ = Mathf.Clamp(velocity.z * VelocityMultiplier.x, XByZLimit.x, XByZLimit.y);
            // float x =  Mathf.Clamp(xByY + xByZ, XTotalLimit.x, XTotalLimit.y);

            // float z = Mathf.Clamp(velocity.y * VelocityMultiplier.z, ZByYLimit.x, ZByYLimit.y);

            // Rotate(TargetLeft, Quaternion.Euler(-Offset.x + x, -Offset.y, -Offset.z + z));
            // Rotate(TargetRight, Quaternion.Euler(-Offset.x + x, Offset.y, Offset.z + -z));
        }
    }
}
