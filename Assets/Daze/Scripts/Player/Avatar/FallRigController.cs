using UnityEngine;

namespace Daze.Player.Avatar
{
    public class FallRig : MonoBehaviour
    {
        [Header("Body Assignments")]

        public Transform Body;

        public Transform LeftUpperArmTarget;
        public Transform RightUpperArmTarget;

        [Header("Settings")]

        public float UpperArmVelocityMultiplier = 50f;

        private Vector3 _prevPos;
        private Vector3 _newPos;
        private Vector3 _velocity;

        public void Start()
        {
            _prevPos = Body.position;
            _newPos = Body.position;
        }

        public void FixedUpdate()
        {
            UpdateVelocity();
            ControlArms();
        }

        private void ControlArms()
        {
            float z = Mathf.Clamp(_velocity.y * UpperArmVelocityMultiplier, -45f, 45f);

            Rotate(LeftUpperArmTarget, Quaternion.Euler(0, 0, -10 + z), 5f);
            Rotate(RightUpperArmTarget, Quaternion.Euler(0, 0, 10 + -z), 5f);
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
