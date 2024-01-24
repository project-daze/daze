using UnityEngine;

namespace Daze.Player.Avatar
{
    public class FallRig : MonoBehaviour
    {
        public Transform Body;

        public Transform LeftUpperArmTarget;

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

            float z = Mathf.Clamp(_velocity.y * 50f, -45f, 45f);

            LeftUpperArmTarget.localRotation = Quaternion.Slerp(
                LeftUpperArmTarget.localRotation,
                Quaternion.Euler(0, 0, z),
                5f * Time.deltaTime
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
