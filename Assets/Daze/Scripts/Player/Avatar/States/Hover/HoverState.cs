using UnityEngine;

namespace Daze.Player.Avatar
{
    public class HoverState : State
    {
        private bool _isStable = false;

        private float _driftTimeV = 0f;
        private float _driftTimeH = 0f;

        private readonly int _hoverHash = Animator.StringToHash("Hover");

        public HoverState(Context ctx) : base(ctx)
        { }

        public override void OnEnter()
        {
            _isStable = false;
            _driftTimeV = 0f;
            _driftTimeH = 0f;
            Ctx.FallRig.Enable();
            Ctx.Animator.SetTrigger(_hoverHash);
        }

        public override void UpdateVelocity(ref Vector3 velocity, float deltaTime)
        {
            // When entering this state, the player might be moving. So, at
            // first we will stabilize player to slowdown until certain
            // velocity treshold, then move to drifting state.
            if (!_isStable)
            {
                Stabilize(ref velocity, deltaTime);
                return;
            }

            Drift(ref velocity, deltaTime);
        }

        private void Stabilize(ref Vector3 velocity, float deltaTime)
        {
            // If the velocity is bigger than 0.5, we will slowdown the player
            // until it reaches less than 0.1.
            if (velocity.magnitude > 0.1f)
            {
                velocity += -velocity.normalized * deltaTime;
                return;
            }

            _isStable = true;
        }

        private void Drift(ref Vector3 velocity, float deltaTime)
        {
            _driftTimeV += Ctx.Settings.DriftVFrequency * deltaTime;
            _driftTimeH += Ctx.Settings.DriftHFrequency * deltaTime;

            float v = Mathf.Sin(-_driftTimeV * Mathf.PI) * Ctx.Settings.DriftVAmplitude;
            float h = Mathf.Cos(-_driftTimeH * Mathf.PI) * Ctx.Settings.DriftHAmplitude;

            Vector3 vOffset = Ctx.Motor.CharacterUp.normalized * v;
            Vector3 hOffset = Ctx.Motor.CharacterRight.normalized * h;

            velocity = vOffset + hOffset;
        }

        public override void UpdateRotation(ref Quaternion rotation, float deltaTime)
        {

        }

        public override void OnAnimatorIK()
        {
            // Ctx.Animator.SetIKPositionWeight(AvatarIKGoal.LeftHand, 0.2f);
            // Ctx.Animator.SetIKRotationWeight(AvatarIKGoal.LeftHand, 1f);
            // Ctx.Animator.SetIKPositionWeight(AvatarIKGoal.RightHand, 0.2f);
            // Ctx.Animator.SetIKRotationWeight(AvatarIKGoal.RightHand, 1f);
            // Ctx.Animator.SetIKPositionWeight(AvatarIKGoal.LeftFoot, 0.4f);
            // Ctx.Animator.SetIKRotationWeight(AvatarIKGoal.LeftFoot, 1f);
            // Ctx.Animator.SetIKPositionWeight(AvatarIKGoal.RightFoot, 0.4f);
            // Ctx.Animator.SetIKRotationWeight(AvatarIKGoal.RightFoot, 1f);

            // Ctx.Animator.SetIKPosition(AvatarIKGoal.LeftHand, Ctx.IKLeftHand.position);
            // Ctx.Animator.SetIKRotation(AvatarIKGoal.LeftHand, Ctx.IKLeftHand.rotation);
            // Ctx.Animator.SetIKPosition(AvatarIKGoal.RightHand, Ctx.IKRightHand.position);
            // Ctx.Animator.SetIKRotation(AvatarIKGoal.RightHand, Ctx.IKRightHand.rotation);
            // Ctx.Animator.SetIKPosition(AvatarIKGoal.LeftFoot, Ctx.IkLeftFoot.position);
            // Ctx.Animator.SetIKRotation(AvatarIKGoal.LeftFoot, Ctx.IkLeftFoot.rotation);
            // Ctx.Animator.SetIKPosition(AvatarIKGoal.RightFoot, Ctx.IkRightFoot.position);
            // Ctx.Animator.SetIKRotation(AvatarIKGoal.RightFoot, Ctx.IkRightFoot.rotation);
        }
    }
}
