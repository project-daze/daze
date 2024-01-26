using UnityEngine;
using UnityEngine.Animations.Rigging;

namespace Daze.Player.Avatar.Rigs
{
    public class FallRig : MonoBehaviour
    {
        public Rig Rig;
        public Transform Body;
        public FallRigBone UpperAms;
        public FallRigBone Hands;
        public FallRigBone UpperLegs;
        public FallRigBone LowerLegs;

        private Animator _animator;

        private bool _isEnabled = false;

        private Vector3 _prevPos;
        private Vector3 _newPos;
        private Vector3 _velocity;

        public void Start()
        {
            _animator = GetComponent<Animator>();

            Rig.weight = 0f;

            _prevPos = Body.position;
            _newPos = Body.position;
        }

        public void FixedUpdate()
        {
            UpdateVelocity();

            TransitionRigWeightTo(_isEnabled ? 1f : 0f);

            if (Rig.weight > 0) {
                UpperAms.Control(_velocity);
                Hands.Control(_velocity);
                UpperLegs.Control(_velocity);
                LowerLegs.Control(_velocity);
            }
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
                    ? Rig.weight + (1f * Time.deltaTime)
                    : Rig.weight - (5f * Time.deltaTime);
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

        public void OnAnimatorIK()
        {
            float weight = Mathf.Lerp(1f, 0f, Rig.weight);
            _animator.SetIKPositionWeight(AvatarIKGoal.LeftFoot, weight);
            _animator.SetIKRotationWeight(AvatarIKGoal.LeftFoot, weight);
            _animator.SetIKPositionWeight(AvatarIKGoal.RightFoot, weight);
            _animator.SetIKRotationWeight(AvatarIKGoal.RightFoot, weight);
        }
    }
}
