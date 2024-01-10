using UnityEngine;
using UnityEngine.InputSystem;

namespace Daze.Inputs
{
    public class PlayerActions : MonoBehaviour, Controls.IPlayerActions
    {
        private Controls _controls;

        public Vector2 MoveComposite;
        public Vector2 LookComposite;

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
        { }

        public void OnGravityOn(InputAction.CallbackContext context)
        { }

        public void OnGravityOff(InputAction.CallbackContext context)
        { }

        public void OnGravityDive(InputAction.CallbackContext context)
        { }
    }
}
