using UnityEngine;
using Daze.Inputs;
using System;

namespace Daze.Camera
{
    [RequireComponent(typeof(PlayerActions))]
    public class FreeCameraControl : MonoBehaviour
    {
        private PlayerActions _actions;

        public float Speed = 200f;

        private Transform _transitionTarget;

        private bool _isActive = false;
        private bool _isTransitioningFrom = false;
        private bool _isTransitioningTo = false;

        private float _diveMoveDirection = 0f;

        public event Action OnTransitionedFrom;
        public event Action OnTransitionedTo;

        private void Awake()
        {
            _actions = GetComponent<PlayerActions>();
        }

        private void FixedUpdate()
        {
            UpdateRotation();
            UpdateTransitionFrom();
            UpdateTransitionTo();
        }

        public void UpdateTransitionFrom()
        {
            if (!_isTransitioningFrom) return;

            transform.rotation = _transitionTarget.rotation;
            _isTransitioningFrom = false;
            OnTransitionedFrom?.Invoke();
        }

        public void UpdateRotation()
        {
            if (!_isActive) return;

            UpdateMoveRotation();
            UpdateLookRotation();
        }

        public void UpdateMoveRotation()
        {
            if (_diveMoveDirection == 0f) return;

            float z = _diveMoveDirection * 10f * Time.deltaTime;
            transform.Rotate(0f, 0f, z);
        }

        public void UpdateLookRotation()
        {
            if (_actions.LookComposite == Vector2.zero) return;

            float x = -_actions.LookComposite.y * Speed * Time.deltaTime;
            float y = _actions.LookComposite.x * Speed * Time.deltaTime;
            transform.Rotate(x, y, 0f);

            if (_diveMoveDirection != 0f)
            {
                float z = _diveMoveDirection * Speed * Time.deltaTime;
                transform.Rotate(0f, 0f, z);
            }
        }

        public void UpdateTransitionTo()
        {
            if (!_isTransitioningTo) return;

            Quaternion targetRotation = Quaternion.LookRotation(transform.forward, _transitionTarget.up);

            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, 10f * Time.deltaTime);

            float diff = Quaternion.Angle(transform.rotation, targetRotation);

            if (diff <= 1f)
            {
                transform.rotation = targetRotation;
                _isTransitioningTo = false;
                OnTransitionedTo?.Invoke();
            }
        }

        public void Activate()
        {
            _isActive = true;
        }

        public void Deactivate()
        {
            _isActive = false;
        }

        public void TransitionFrom(Transform target)
        {
            _transitionTarget = target;
            _isTransitioningFrom = true;
        }

        public void TransitionTo(Transform target)
        {
            _transitionTarget = target;
            _isTransitioningTo = true;
        }

        public void SetDiveMoveDirection(float direction)
        {
            _diveMoveDirection = direction;
        }
    }
}
