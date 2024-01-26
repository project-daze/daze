using UnityEngine;
using UnityEngine.Animations.Rigging;

namespace Daze.Player.Avatar.Rigs
{
    public class FallRigBoneHands : FallRigBone
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
            Rotate(TargetLeft, Quaternion.Euler(Offset.x, 0, 0));
            Rotate(TargetRight, Quaternion.Euler(Offset.x, 0, 0));
        }
    }
}
