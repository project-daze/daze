using System.Collections.Generic;
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
            Rotate(TargetLeft, Quaternion.Euler(Offset.x, 0, 0));
            Rotate(TargetRight, Quaternion.Euler(Offset.x, 0, 0));
        }
    }
}
