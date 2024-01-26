using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations.Rigging;

namespace Daze.Player.Avatar.Rigs
{
    public class FallRig : MonoBehaviour
    {
        public Rig Rig;
        public Transform Body;
        public FallRigBone UpperArms;
        public FallRigBone LowerArms;
        public FallRigBone Hands;
        public FallRigBone UpperLegs;
        public FallRigBone LowerLegs;

        [Header("Settings")]

        public float WeightEnableSpeed = 1f;
        public float WeightDisableSpeed = 5f;

        public bool DisplayMesh = false;

        private Animator _animator;
        private readonly List<FallRigBone> _bones = new();

        private bool _isEnabled = false;

        private Vector3 _prevPos;
        private Vector3 _newPos;
        private Vector3 _velocity;

        private void OnValidate()
        {
            UnityEditor.EditorApplication.delayCall += DoValidate;
        }

        private void DoValidate()
        {
            UnityEditor.EditorApplication.delayCall -= DoValidate;
            if (this != null)
            {
                ShowMesh();
            }
        }

        private void Start()
        {
            _animator = GetComponent<Animator>();

            _bones.Add(UpperArms);
            _bones.Add(LowerArms);
            _bones.Add(Hands);
            _bones.Add(UpperLegs);
            _bones.Add(LowerLegs);

            Rig.weight = 0f;

            _prevPos = Body.position;
            _newPos = Body.position;

            ShowMesh();
        }

        private void FixedUpdate()
        {
            UpdateVelocity();

            TransitionRigWeightTo(_isEnabled ? 1f : 0f);

            if (Rig.weight > 0) {
                foreach (FallRigBone bone in _bones) {
                    bone.Control(_velocity);
                }
            }
        }

        private void OnAnimatorIK()
        {
            float weight = Mathf.Lerp(1f, 0f, Rig.weight);
            _animator.SetIKPositionWeight(AvatarIKGoal.LeftFoot, weight);
            _animator.SetIKRotationWeight(AvatarIKGoal.LeftFoot, weight);
            _animator.SetIKPositionWeight(AvatarIKGoal.RightFoot, weight);
            _animator.SetIKRotationWeight(AvatarIKGoal.RightFoot, weight);
        }

        public void Enable()
        {
            _isEnabled = true;
        }

        public void Disable()
        {
            _isEnabled = false;
        }

        private void TransitionRigWeightTo(float to)
        {
            if (Rig.weight != to) {
                Rig.weight = to == 1f
                    ? Rig.weight + (WeightEnableSpeed * Time.deltaTime)
                    : Rig.weight - (WeightDisableSpeed * Time.deltaTime);
            }
        }

        /// <summary>
        /// Because we use Kinematic Character Controller to move the
        /// character, we must calculate the velocity ourselves.
        /// </summary>
        private void UpdateVelocity()
        {
            _newPos = Body.position;

            Vector3 worldVelocity = (_newPos - _prevPos) / Time.fixedDeltaTime;
            Vector3 localVelocity = Body.InverseTransformDirection(worldVelocity);
            _velocity = localVelocity;

            _prevPos = _newPos;
        }

        private void ShowMesh()
        {
            foreach (FallRigBone bone in _bones) {
                bone.ShowMesh(DisplayMesh);
            }
        }
    }
}
