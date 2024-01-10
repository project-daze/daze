using UnityEngine;
using UnityEngine.InputSystem;

public class Inputs : Controls.IPlayerActions
{
    public Player Player;
    private Controls controls;

    public Vector2 MoveComposite;
    public Vector2 LookComposite;

    public void OnAwake(Player player)
    {
        Player = player;
    }

    public void OnEnable()
    {
        if (controls != null) return;

        controls = new Controls();
        controls.Player.SetCallbacks(this);
        controls.Player.Enable();
    }

    public void OnDisable()
    {
        controls.Player.Disable();
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        Player.Motion.Move(context.ReadValue<Vector2>());
    }

    public void OnLook(InputAction.CallbackContext context)
    {
        LookComposite = context.ReadValue<Vector2>();
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            Player.Motion.Jump();
        }
    }

    public void OnGravityOn(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            Player.Motion.GravityControl();
        }
    }

    public void OnGravityOff(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            Player.Motion.Drop();
        }
    }

    public void OnGravityDive(InputAction.CallbackContext context)
    {
        if (context.performed)
        {

        }
    }
}
