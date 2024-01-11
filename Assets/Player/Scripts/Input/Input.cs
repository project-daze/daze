using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Daze.Player
{
    public class Input : MonoBehaviour, Controls.IPlayerActions
    {
        [NonSerialized] public Controller Controller;

        private Controls _controls;

        public Vector2 MoveComposite;
        public Vector2 LookComposite;

        public event Action Jump;
        public event Action GravityOn;
        public event Action GravityOff;

        public void OnAwake(Controller controller)
        {
            Controller = controller;
        }

        public void OnEnable()
        {
            if (_controls != null) return;

            _controls = new Controls();
            _controls.Player.SetCallbacks(this);
            _controls.Player.Enable();
        }

        public void OnDisable()
        {
            _controls.Player.Disable();
        }

        public void OnMove(InputAction.CallbackContext context)
        {
            MoveComposite = context.ReadValue<Vector2>();
        }

        public void OnLook(InputAction.CallbackContext context)
        {
            LookComposite = context.ReadValue<Vector2>();
        }

        public void OnJump(InputAction.CallbackContext context)
        {
            if (context.performed)
            {
                Jump?.Invoke();
            }
        }

        public void OnGravityOn(InputAction.CallbackContext context)
        {
            if (context.performed)
            {
                GravityOn?.Invoke();
            }
        }

        public void OnGravityOff(InputAction.CallbackContext context)
        {
            if (context.performed)
            {
                GravityOff?.Invoke();
            }
        }

        public void OnGravityDive(InputAction.CallbackContext context)
        {
            if (context.performed)
            {

            }
        }
    }
}
