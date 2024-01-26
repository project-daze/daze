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
        public Vector2 ZByYLimit;

        protected override OverrideTransform[] Rigs()
        {
            return new OverrideTransform[] { RigLeft, RigRight };
        }

        protected override void DoControl(Vector3 velocity)
        {
            float x = Mathf.Clamp(-velocity.y * VelocityMultiplier.x, XByYLimit.x, XByYLimit.y);
            float z = Mathf.Clamp(velocity.y * VelocityMultiplier.z, ZByYLimit.x, ZByYLimit.y);

            Rotate(TargetLeft, Quaternion.Euler(-Offset.x + -x, -Offset.y, -Offset.z + z));
            Rotate(TargetRight, Quaternion.Euler(-Offset.x + -x, Offset.y, Offset.z + -z));
        }
    }
}
