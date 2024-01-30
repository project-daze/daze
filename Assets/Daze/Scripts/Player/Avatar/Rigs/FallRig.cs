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

        public float WeightEnableSpeed;
        public float WeightDisableSpeed;

        public bool UseManualVelocity = false;
        public Vector3 ManualVelocity = Vector3.zero;
        public bool DisplayMesh = false;

        private Animator _animator;
        private readonly List<FallRigBone> _bones = new();

        private bool _isEnabled = false;

        private Vector3 _prevPos;
        private Vector3 _velocity;
        private Vector3 _prevVelocity;
        private Vector3 _acceleration;

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

            ShowMesh();
        }

        private void FixedUpdate()
        {
            UpdateVelocity();

            TransitionRigWeightTo(_isEnabled ? 1f : 0f);

            if (Rig.weight > 0)
            {
                foreach (FallRigBone bone in _bones)
                {
                    bone.Control(
                        UseManualVelocity ? ManualVelocity : _velocity,
                        UseManualVelocity ? ManualVelocity : _acceleration
                    );
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

        public void Break()
        {
            foreach (FallRigBone bone in _bones)
            {
                bone.Break();
            }
        }

        private void TransitionRigWeightTo(float to)
        {
            if (Rig.weight != to)
            {
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
            Vector3 worldVelocity = (Body.position - _prevPos) / Time.fixedDeltaTime;
            Vector3 localVelocity = Body.InverseTransformDirection(worldVelocity);
            _velocity = localVelocity;
            _acceleration = _velocity - _prevVelocity;
            _prevPos = Body.position;
            _prevVelocity = localVelocity;
        }

        private void ShowMesh()
        {
            foreach (FallRigBone bone in _bones)
            {
                bone.ShowMesh(DisplayMesh);
            }
        }
    }
}
