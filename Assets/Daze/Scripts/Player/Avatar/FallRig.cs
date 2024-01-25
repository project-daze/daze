using System;
using UnityEngine;
using UnityEngine.Animations.Rigging;

namespace Daze.Player.Avatar
{
    public class FallRig : MonoBehaviour
    {
        public Rig Rig;

        [Header("Body Assignments")]

        public Transform Body;

        public Transform LeftUpperArmTarget;
        public Transform RightUpperArmTarget;
        public Transform LeftUpperLegTarget;
        public Transform RightUpperLegTarget;

        [Header("Upper Arms")]

        public float UpperArmVelocityMultiplier = 50f;
        public Vector3 UpperArmPositiveBendLimit = new(0, 0, 45f);
        public Vector3 UpperArmNegativeBendLimit = new(0, 0, -45f);
        public Vector3 UpperArmOffset = new(0, 0, 10f);
        public float UpperArmRotationSpeed = 5f;

        [Header("Upper Legs")]

        public float UpperLegVelocityMultiplier = 50f;
        public Vector3 UpperLegPositiveBendLimit = new(0, 0, 45f);
        public Vector3 UpperLegNegativeBendLimit = new(0, 0, -45f);
        public Vector3 UpperLegOffset = new(0, 0, 0);
        public float UpperLegRotationSpeed = 5f;

        private bool _isEnabled = false;

        private float _rigWeightTransitionProgress = 0f;

        private Vector3 _prevPos;
        private Vector3 _newPos;
        private Vector3 _velocity;

        public void Start()
        {
            Rig.weight = 0f;
            _prevPos = Body.position;
            _newPos = Body.position;
        }

        public void FixedUpdate()
        {
            UpdateVelocity();

            if (!_isEnabled) {
                TransitionRigWeightTo(0f);
                return;
            }

            TransitionRigWeightTo(1f);

            ControlArms();
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
            if (Rig.weight == to) {
                return;
            }

            Rig.weight += 0.8f * Time.deltaTime;
        }

        private void ControlArms()
        {
            float z = Mathf.Clamp(
                _velocity.y * UpperArmVelocityMultiplier,
                UpperArmNegativeBendLimit.z,
                UpperArmPositiveBendLimit.z
            );

            Rotate(LeftUpperArmTarget, Quaternion.Euler(0, 0, z + -UpperArmOffset.z), UpperArmRotationSpeed);
            Rotate(RightUpperArmTarget, Quaternion.Euler(0, 0, -z + UpperArmOffset.z), UpperArmRotationSpeed);
        }

        private void ControlLegs()
        {
            float z = Mathf.Clamp(
                _velocity.y * UpperLegVelocityMultiplier,
                UpperLegNegativeBendLimit.z,
                UpperLegPositiveBendLimit.z
            );

            Rotate(LeftUpperLegTarget, Quaternion.Euler(0, 0, z + -UpperLegOffset.z), UpperLegRotationSpeed);
            Rotate(RightUpperLegTarget, Quaternion.Euler(0, 0, -z + UpperLegOffset.z), UpperLegRotationSpeed);
        }

        private void Rotate(
            Transform bone,
            Quaternion rotation,
            float speed
        )
        {
            bone.localRotation = Quaternion.Slerp(
                bone.localRotation,
                rotation,
                speed * Time.deltaTime
            );
        }

        private void UpdateVelocity()
        {
            _newPos = Body.position;

            Vector3 worldVelocity = (_newPos - _prevPos) / Time.fixedDeltaTime;
            Vector3 localVelocity = Body.InverseTransformDirection(worldVelocity);
            _velocity = localVelocity;

            _prevPos = _newPos;
        }
    }
}
