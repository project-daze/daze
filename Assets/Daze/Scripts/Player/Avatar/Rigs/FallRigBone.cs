using System.Collections.Generic;
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

        protected List<OverrideTransform> _rigs = new();
        protected List<Transform> _targets = new();

        public void Awake()
        {
            _rigs = Rigs();
            _targets = Targets();
            SetWeight();
        }

        public void ShowMesh(bool show)
        {
            foreach (Transform target in _targets) {
                target.GetComponent<MeshRenderer>().enabled = show;
            }
        }

        protected abstract List<OverrideTransform> Rigs();
        protected abstract List<Transform> Targets();

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
