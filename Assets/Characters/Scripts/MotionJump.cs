using UnityEngine;

public class MotionJump
{
    private readonly int _isGroundedHash = Animator.StringToHash("IsGrounded");
    private readonly int _jumpHash = Animator.StringToHash("Jump");
    private readonly int _jumpIntensity = Animator.StringToHash("JumpIntensity");

    public void OnFixedUpdate(Motion motion)
    {
        if (motion.IsJumpRising)
        {
            Vector3 localVelocity = motion.Rigidbody.transform.InverseTransformDirection(motion.Rigidbody.velocity);

            if (localVelocity.y < -0.1f) {
                motion.IsJumpRising = false;
                motion.IsJumpFalling = true;
            }
        }
    }

    public void Do(Motion motion)
    {
        if (!CanJump(motion)) return;

        if (motion.IsJumpPreparing) {
            Rise(motion);
            return;
        }

        if (motion.IsRunning) {
            Rise(motion);
            return;
        }

        Prepare(motion);
    }

    public void Prepare(Motion motion)
    {
        motion.IsJumpPreparing = true;
        motion.Animator.SetBool(_isGroundedHash, false);
        motion.Animator.SetTrigger(_jumpHash);

        SetJumpAnimationIntensity(motion);
    }

    private void SetJumpAnimationIntensity(Motion motion)
    {
        var jumpIntensity = 0f;

        if (motion.IsWalking)
        {
            jumpIntensity = 0.5f;
        }
        else if (motion.IsRunning)
        {
            jumpIntensity = 1f;
        }

        motion.Animator.SetFloat(_jumpIntensity, jumpIntensity);
    }

    public void Rise(Motion motion)
    {
        motion.Animator.SetBool(_isGroundedHash, false);

        if (!motion.IsJumpPreparing) {
            motion.Animator.SetTrigger(_jumpHash);
        }

        SetJumpAnimationIntensity(motion);
        motion.IsGrounded = false;
        motion.IsJumpPreparing = false;
        motion.IsJumpRising = true;
        motion.Rigidbody.AddForce(-motion.GravityDirection * motion.JumpForce, ForceMode.VelocityChange);
    }

    public bool CanJump(Motion motion)
    {
        return (
            motion.IsGrounded
            && !motion.IsJumpRising
            && !motion.IsJumpFalling
            && !motion.IsHovering
            && !motion.IsLifting
            && !motion.IsDiving
        );
    }
}
