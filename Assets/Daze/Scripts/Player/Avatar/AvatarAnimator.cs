using UnityEngine;

namespace Daze.Player.Avatar
{
    public class AvatarAnimator
    {
        /// <summary>
        /// The unity animator component.
        /// </summary>
        private readonly Animator _animator;

        /// <summary>
        /// The animation root motion posistion delta. The value is set in
        /// `AvatarController` through `OnAnimatorMove` callback.
        /// </summary>
        public Vector3 RootMotionPositionDelta = Vector3.zero;

        /// <summary>
        /// The animation root motion rotation delta. The value is set in
        /// `AvatarController` through `OnAnimatorMove` callback.
        /// </summary>
        public Quaternion RootMotionRotationDelta = Quaternion.identity;

        /// <summary>
        /// The ground movement velocity float parameter. This parameter is
        /// used to animate the ground based animation such as walk and run,
        /// including when the character is in the air like jumping.
        ///
        /// The parameter is set in ground state during when it updates
        /// character movement.
        /// </summary>
        private readonly int _paramIsRunning = Animator.StringToHash("IsRunning");

        /// <summary>
        /// The hover trigger parameter.
        /// </summary>
        private readonly int _paramHover = Animator.StringToHash("Hover");

        /// <summary>
        /// The land trigger parameter.
        /// </summary>
        private readonly int _paramLand = Animator.StringToHash("Land");

        /// <summary>
        /// The fall trigger parameter.
        /// </summary>
        private readonly int _paramFall = Animator.StringToHash("Fall");

        /// <summary>
        /// The unity animator component.
        /// </summary>
        public AvatarAnimator(Animator animator)
        {
            _animator = animator;
        }

        /// <summary>
        /// Set the ground movement velocity parameter.
        /// </summary>
        public void SetIsRunning(bool value)
        {
            _animator.SetBool(_paramIsRunning, value);
        }

        /// <summary>
        /// Trigger the hover parameter.
        /// </summary>
        public void TriggerHover()
        {
            _animator.SetTrigger(_paramHover);
        }

        /// <summary>
        /// Trigger the land parameter.
        /// </summary>
        public void TriggerLand()
        {
            _animator.SetTrigger(_paramLand);
        }

        /// <summary>
        /// Trigger the fall parameter.
        /// </summary>
        public void TriggerFall()
        {
            _animator.SetTrigger(_paramFall);
        }

        /// <summary>
        /// Check if the clip of the given name is playing.
        /// </summary>
        public bool IsClipPlaying(string clipName)
        {
            AnimatorClipInfo[] clipInfo = _animator.GetCurrentAnimatorClipInfo(0);

            if (clipInfo.Length == 0)
            {
                return false;
            }

            return clipInfo[0].clip.name == clipName;
        }
    }
}
