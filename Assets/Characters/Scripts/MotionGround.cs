using UnityEngine;

public class MotionGround
{
    private readonly int _isGroundedHash = Animator.StringToHash("IsGrounded");

    public void OnFixedUpdate(Motion motion)
    {
        UpdateGroundCheck(motion);
        UpdateRotation(motion);
    }

    private void UpdateGroundCheck(Motion motion)
    {
        if (!ShouldCheckGround(motion)) return;

        // Check the ground by casting ray toward the gravity direction,
        // and if it doesn't hit anything, do nothing.
        (bool isGrounded, RaycastHit hit) = CheckGround(motion);

        if (!isGrounded) return;

        // When ground is detected and if the player is diving, set gravity
        // direction to the ground normal so that the player can stand on
        // a wall or a ceiling.
        if (motion.IsDiving)
        {
            motion.MGravity.SetGravityDirection(motion, -hit.normal);
        }

        // When grounded, set back gravity force so that the player can "stand"
        // and set states to the grounded mode.
        motion.IsGrounded = true;
        motion.IsJumpFalling = false;
        motion.IsDiving = false;

        motion.MGravity.ResetGravityForce(motion);
        motion.Animator.SetBool(_isGroundedHash, motion.IsGrounded);
        motion.Player.EmitDiving(0);
        motion.Player.EmitGrounded();
    }

    private void UpdateRotation(Motion motion)
    {
        if (!motion.IsGrounded) return;

        Quaternion upRotation = Quaternion.FromToRotation(motion.Rigidbody.transform.up, -motion.GravityDirection);
        Quaternion newRotation = Quaternion.Slerp(motion.Rigidbody.rotation, upRotation * motion.Rigidbody.rotation, Time.fixedDeltaTime * 100f);
        motion.Rigidbody.MoveRotation(newRotation);
    }

    public bool ShouldCheckGround(Motion motion)
    {
        return motion.IsJumpFalling || motion.IsDiving;
    }

    private (bool, RaycastHit) CheckGround(Motion motion)
    {
        bool check = Physics.Raycast(
            motion.GroundCheck.transform.position,
            motion.GravityDirection,
            out RaycastHit hit,
            1.1f,
            motion.GroundMask
        );

        return (check, hit);
    }
}
