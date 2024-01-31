using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations.Rigging;

namespace Daze.Player.Avatar.Rigs
{
    public class FallRig : MonoBehaviour
    {
        public Transform Body;
        public Rig Rig;

        [Header("Bone Assignments")]

        public List<FallRigBone> Bones;

        [Header("Settings")]

        public float WeightEnableSpeed;
        public float WeightDisableSpeed;

        [Header("Debug")]

        public bool UseManualVelocity = false;
        public Vector3 ManualVelocity = Vector3.zero;
        public bool UseManualWeight = false;
        public float ManualWeight = 0f;

        private Animator _animator;

        private bool _isEnabled = false;

        private Vector3 _prevPos;
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
                foreach (FallRigBone bone in Bones)
                {
                    bone.UseManualWeight(UseManualWeight, ManualWeight);
                }
            }
        }

        private void Start()
        {
            _animator = GetComponent<Animator>();

            Rig.weight = 0f;
            _prevPos = Body.position;
        }

        private void FixedUpdate()
        {
            UpdateVelocity();
            TransitionRigWeightTo(_isEnabled ? 1f : 0f);

            if (Rig.weight > 0)
            {
                foreach (FallRigBone bone in Bones)
                {
                    bone.Control(UseManualVelocity ? ManualVelocity : _velocity);
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
            if (Rig.weight != to)
            {
                Rig.weight = to == 1f
                    ? Mathf.Lerp(Rig.weight, 1, WeightEnableSpeed * Time.deltaTime)
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
            _prevPos = Body.position;
        }
    }
}
