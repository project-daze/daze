using System;
using UnityEngine;

namespace Daze.Player.Avatar
{
    public class AvatarAnimatorMoveHook : MonoBehaviour
    {
        public Action<Vector3> OnMove;

Vector3 lastPosition;
Vector3 deltaPosition;

    private Animator animator;

    void Start()
    {
        // Cache the Animator component
        animator = GetComponent<Animator>();
    }

        public void OnAnimatorMove()
        {
            Debug.Log("Applying root motion:" + animator.applyRootMotion);
            Debug.Log("TTT: " + animator.deltaPosition);

            OnMove?.Invoke(animator.deltaPosition);
        }
    }
}
