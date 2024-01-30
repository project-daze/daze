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

        [Header("Y By Z")]

        public Vector2 YByZMultiplier;
        public Vector2 YByZCounterThreshold;
        public Vector2 YByZLimit;

        [Header("Z By Y")]

        public Vector2 ZByYMultiplier;
        public Vector2 ZByYCounterThreshold;
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
            float y = Value(velocity.z, YByZMultiplier, YByZCounterThreshold, YByZLimit);
            float z = Value(velocity.y, ZByYMultiplier, ZByYCounterThreshold, ZByYLimit);

            Rotate(TargetLeft, Quaternion.Euler(0, -Offset.y + -y, -Offset.z + z));
            Rotate(TargetRight, Quaternion.Euler(0, Offset.y + y, Offset.z + -z));
        }
    }
}
