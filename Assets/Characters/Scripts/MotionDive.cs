using UnityEngine;

public class MotionDive
{
    private readonly int _diveHash = Animator.StringToHash("Dive");

    public void OnFixedUpdate(Motion motion)
    {
        // if (!motion.IsDiving) return;

        // float speed = motion.DiveSpeed + motion.DiveAcceleration * Time.fixedDeltaTime;
        // float min = 0f;
        // float max = motion.DiveMaxSpeed;
        // motion.DiveSpeed = Mathf.Clamp(speed, min, max);
        // motion.Rigidbody.velocity = motion.GravityDirection * (motion.DiveSpeed * Time.fixedDeltaTime);

        // Quaternion upRotation = Quaternion.FromToRotation(motion.Rigidbody.transform.up, motion.GravityDirection);
        // Quaternion newRotation = Quaternion.Slerp(motion.Rigidbody.rotation, upRotation * motion.Rigidbody.rotation, 3f * Time.fixedDeltaTime);
        // motion.Rigidbody.MoveRotation(newRotation);

        // motion.Player.EmitDiving(motion.Rigidbody.velocity.magnitude);
    }

    public void Forward(Motion motion)
    {
        // motion.IsGrounded = false;
        // motion.IsRunning = false;
        // motion.IsJumpPreparing = false;
        // motion.IsJumpRising = false;
        // motion.IsJumpFalling = false;
        // motion.IsHovering = false;
        // motion.IsStable = false;
        // motion.IsLifting = false;
        // motion.IsDiving = true;
        // motion.MGravity.SetGravityDirection(motion, motion.Camera.forward);
        // motion.Animator.SetTrigger(_diveHash);
    }
}
