using UnityEngine;

public class MotionFall
{
    private readonly int _fallXHash = Animator.StringToHash("FallX");
    private readonly int _fallYHash = Animator.StringToHash("FallY");
    private readonly int _fallZHash = Animator.StringToHash("FallZ");

    public void OnFixedUpdate(Motion motion)
    {
        if (!motion.IsDiving) return;

        float speed = motion.DiveSpeed + motion.DiveAcceleration * Time.fixedDeltaTime;
        float min = 0f;
        float max = motion.DiveMaxSpeed;
        motion.DiveSpeed = Mathf.Clamp(speed, min, max);
        motion.Rigidbody.velocity = motion.GravityDirection * (motion.DiveSpeed * Time.fixedDeltaTime);

        float x = Remap(motion.Rigidbody.velocity.x, -8f, 8f, -1f, 1f);
        float y = Remap(motion.Rigidbody.velocity.y, -8f, 8f, -1f, 1f);
        float z = Remap(motion.Rigidbody.velocity.z, -8f, 8f, -1f, 1f);

        motion.Animator.SetFloat(_fallXHash, x);
        motion.Animator.SetFloat(_fallYHash, y);
        motion.Animator.SetFloat(_fallZHash, z);

        motion.Player.EmitDiving(motion.Rigidbody.velocity.magnitude);
    }

    public void Forward(Motion motion)
    {
        motion.IsGrounded = false;
        motion.IsRunning = false;
        motion.IsJumpPreparing = false;
        motion.IsJumpRising = false;
        motion.IsJumpFalling = false;
        motion.IsHovering = false;
        motion.IsStable = false;
        motion.IsLifting = false;
        motion.IsDiving = true;
        motion.MGravity.SetGravityDirection(motion, motion.Camera.forward);
    }

    float Remap(float value, float fromMin, float fromMax, float toMin, float toMax) {
        return (value - fromMin) / (fromMax - fromMin) * (toMax - toMin) + toMin;
    }
}
