using UnityEngine;
using UnityEngine.Animations.Rigging;

namespace Daze.Player.Avatar.Rigs
{
    public class FallRigBoneLowerLegs : FallRigBone
    {
        public OverrideTransform RigLeft;
        public OverrideTransform RigRight;
        public Transform TargetLeft;
        public Transform TargetRight;

        protected override OverrideTransform[] Rigs()
        {
            return new OverrideTransform[] { RigLeft, RigRight };
        }

        protected override void DoControl(Vector3 velocity)
        {
            Rotate(TargetLeft, Quaternion.Euler(Offset.x, -Offset.y, -Offset.z));
            Rotate(TargetRight, Quaternion.Euler(Offset.x, Offset.y, Offset.z));
        }
    }
}
