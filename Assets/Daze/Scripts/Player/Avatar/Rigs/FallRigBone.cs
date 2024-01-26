using UnityEngine;
using UnityEngine.Animations.Rigging;

namespace Daze.Player.Avatar.Rigs
{
    public abstract class FallRigBone : MonoBehaviour
    {
        public float Weight;
        public float RotationSpeed = 5f;
        public Vector3 Offset;
        public Vector3 VelocityMultiplier;

        protected OverrideTransform[] _rigs;

        public void Start()
        {
            _rigs = Rigs();
            SetWeight();
        }

        protected abstract OverrideTransform[] Rigs();

        public void Control(Vector3 velocity)
        {
            SetWeight();
            DoControl(velocity);
        }

        protected abstract void DoControl(Vector3 velocity);

        protected void Rotate(Transform bone, Quaternion rotation)
        {
            bone.localRotation = Quaternion.Slerp(
                bone.localRotation,
                rotation,
                RotationSpeed * Time.fixedDeltaTime
            );
        }

        protected void SetWeight()
        {
            foreach (OverrideTransform rig in _rigs) {
                rig.weight = Weight;
            }
        }
    }
}
