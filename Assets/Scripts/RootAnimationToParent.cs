using UnityEngine;

namespace Daze
{
    public class RootAnimationToParent : MonoBehaviour
    {
        private Animator _animator;

        void Awake()
        {
            _animator = GetComponent<Animator>();
        }

        void OnAnimatorMove()
        {
            transform.parent.transform.position += _animator.deltaPosition;
        }
    }
}
