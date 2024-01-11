using UnityEngine;

namespace Daze.Player
{
    public class Controller : MonoBehaviour
    {
        public Character Character;
        public Cam Cam;
        public Input Input;

        // ---------------------------------------------------------------------
        // Jump
        // ---------------------------------------------------------------------

        public bool JumpRequested = false;
        public float TimeSinceJumpRequested = Mathf.Infinity;

        // ---------------------------------------------------------------------
        // Fall
        // ---------------------------------------------------------------------

        public bool IsFalling = false;

        private void Awake()
        {
            Character.OnAwake(this);
            Cam.OnAwake(this);
            Input.OnAwake(this);

            SetupCharacter();
            SetupInput();
        }

        private void SetupCharacter()
        {
            Character.TimeSinceJumpRequestedUpdated += SetTimeSinceJumpRequested;
            Character.JumpConsumed += ResetJumpRequested;
            Character.RejectJumpRequest += ResetJumpRequested;
        }

        private void SetupInput()
        {
            Input.Jump += Jump;
            Input.GravityOn += GravityOn;
            Input.GravityOff += GravityOff;
        }

        private void Jump()
        {
            JumpRequested = true;
            TimeSinceJumpRequested = 0f;
        }

        private void SetTimeSinceJumpRequested(float time)
        {
            TimeSinceJumpRequested = time;
        }

        private void ResetJumpRequested()
        {
            JumpRequested = false;
        }

        private void GravityOn()
        {
            IsFalling = true;
        }

        private void GravityOff()
        {
            IsFalling = false;
        }
    }
}
