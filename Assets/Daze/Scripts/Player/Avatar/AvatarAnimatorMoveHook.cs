using System;
using UnityEngine;

namespace Daze.Player.Avatar
{
    [RequireComponent(typeof(Animator))]
    public class AvatarAnimatorMoveHook : MonoBehaviour
    {
        private Animator animator;

        public Action<Vector3, Quaternion> OnMove;

        void Start()
        {
            animator = GetComponent<Animator>();
        }

        public void OnAnimatorMove()
        {
            OnMove?.Invoke(animator.deltaPosition, animator.deltaRotation);
        }
    }
}
