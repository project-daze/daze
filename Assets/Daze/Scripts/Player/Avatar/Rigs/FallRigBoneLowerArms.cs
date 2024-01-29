using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations.Rigging;

namespace Daze.Player.Avatar.Rigs
{
    public class FallRigBoneLowerArms : FallRigBone
    {
        public OverrideTransform RigLeft;
        public OverrideTransform RigRight;
        public Transform TargetLeft;
        public Transform TargetRight;

        public Vector2 YByZLimit;

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
            float y = Mathf.Clamp(Mathf.Abs(velocity.z) * VelocityMultiplier.y, YByZLimit.x, YByZLimit.y);

            Rotate(TargetLeft, Quaternion.Euler(0, -Offset.y + -y, -Offset.z));
            Rotate(TargetRight, Quaternion.Euler(0, Offset.y + y, Offset.z));
        }
    }
}
