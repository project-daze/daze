using UnityEngine;

public class MotionLift
{
    private float _liftTimer;

    private readonly int _isGroundedHash = Animator.StringToHash("IsGrounded");
    private readonly int _hoverHash = Animator.StringToHash("Hover");

    public void OnFixedUpdate(Motion motion)
    {
        if (!motion.IsLifting) return;

        float speed = motion.DiveSpeed + motion.LiftAcceleration * Time.fixedDeltaTime;
        float min = 0f;
        float max = motion.DiveMaxSpeed;
        motion.DiveSpeed = Mathf.Clamp(speed, min, max);
        motion.Rigidbody.velocity = -motion.GravityDirection * (motion.DiveSpeed * Time.fixedDeltaTime);

        _liftTimer += Time.deltaTime;

        if (_liftTimer >= 0.75f) {
            motion.MHover.Do(motion);
        }
    }

    public void Do(Motion motion)
    {
        _liftTimer = 0f;
        motion.DiveSpeed = 0f;
        motion.IsGrounded = false;
        motion.IsLifting = true;
        motion.MGravity.RemoveGravityForce(motion);
        motion.Animator.SetBool(_isGroundedHash, false);
        motion.Animator.SetTrigger(_hoverHash);
        motion.Player.EmitHover();
    }
}
