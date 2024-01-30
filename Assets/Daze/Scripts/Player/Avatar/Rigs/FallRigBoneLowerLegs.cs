using System.Collections.Generic;
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
            // Rotate(TargetLeft, Quaternion.Euler(Offset.x, -Offset.y, -Offset.z));
            // Rotate(TargetRight, Quaternion.Euler(Offset.x, Offset.y, Offset.z));
        }
    }
}
