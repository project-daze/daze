using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations.Rigging;

namespace Daze.Player.Avatar.Rigs
{
    public class FallRigBoneUpperArms : FallRigBone
    {
        public OverrideTransform RigLeft;
        public OverrideTransform RigRight;
        public Transform TargetLeft;
        public Transform TargetRight;

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
            float z = Mathf.Clamp(velocity.y * VelocityMultiplier.z, ZByYLimit.x, ZByYLimit.y);

            Rotate(TargetLeft, Quaternion.Euler(0, 0, -Offset.z + z));
            Rotate(TargetRight, Quaternion.Euler(0, 0, Offset.z + -z));
        }
    }
}
