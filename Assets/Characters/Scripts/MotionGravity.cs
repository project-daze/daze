using UnityEngine;

public class MotionGravity
{
    public void OnFixedUpdate(Motion motion)
    {
        if (!ShouldAddGravity(motion)) return;

        motion.Rigidbody.AddForce(motion.GravityDirection * (motion.GravityForce * Time.fixedDeltaTime), ForceMode.Acceleration);
    }

    public void SetGravityDirection(Motion motion, Vector3 direction)
    {
        motion.GravityDirection = direction;
    }

    public void ResetGravityDirection(Motion motion)
    {
        motion.GravityDirection = Vector3.down;
    }

    public void ReverseGravityDirection(Motion motion)
    {
        motion.GravityDirection = -motion.GravityDirection;
    }

    public void ResetGravityForce(Motion motion)
    {
        motion.GravityForce = 1200;
    }

    public void RemoveGravityForce(Motion motion)
    {
        motion.GravityForce = 0;
    }

    public bool ShouldAddGravity(Motion motion)
    {
        return (
            motion.IsGrounded
            || motion.IsJumpPreparing
            || motion.IsJumpRising
            || motion.IsJumpFalling
        );
    }
}
