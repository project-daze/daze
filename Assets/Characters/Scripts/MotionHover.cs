using UnityEngine;

public class MotionHover
{
    private float _driftTimeV = 0f;
    private float _driftTimeH = 0f;

    private readonly int _isGroundedHash = Animator.StringToHash("IsGrounded");
    private readonly int _hoverHash = Animator.StringToHash("Hover");

    public void OnFixedUpdate(Motion motion)
    {
        if (!motion.IsHovering) return;
        Stabilize(motion);
        Drift(motion);
    }

    public void Do(Motion motion)
    {
        motion.Animator.SetBool(_isGroundedHash, false);

        if (!motion.IsLifting)
        {
            motion.Animator.SetTrigger(_hoverHash);
        }

        _driftTimeV = 0f;
        _driftTimeH = 0f;
        motion.DiveSpeed = 0f;
        motion.IsRunning = false;
        motion.IsJumpPreparing = false;
        motion.IsJumpRising = false;
        motion.IsJumpFalling = false;
        motion.IsHovering = true;
        motion.IsStable = false;
        motion.IsLifting = false;
        motion.IsDiving = false;

        motion.Player.EmitHover();
    }

    public void Stabilize(Motion motion)
    {
        if (motion.IsStable) return;

        if (motion.Rigidbody.velocity.magnitude > 0.5f)
        {
            motion.Rigidbody.velocity *= motion.DiveDeceleration;
            motion.Player.EmitDiving(motion.Rigidbody.velocity.magnitude);
            return;
        }

        _driftTimeV = 0f;
        _driftTimeH = 0f;
        motion.HoverPosition = motion.Rigidbody.transform.position;
        motion.IsStable = true;
        motion.Player.EmitDiving(0);
    }

    public void Drift(Motion motion)
    {
        if (!motion.IsStable) return;

        _driftTimeV += Time.deltaTime * motion.DriftVFrequency;
        _driftTimeH += Time.deltaTime * motion.DriftHFrequency;

        float v = _driftTimeV % (2 * Mathf.PI) > Mathf.PI
            ? Mathf.Sin(-_driftTimeV * Mathf.PI) * motion.DriftVAmplitude2
            : Mathf.Sin(-_driftTimeV * Mathf.PI) * motion.DriftVAmplitude1;

        float h = Mathf.Sin(-_driftTimeH * Mathf.PI) * motion.DriftHAmplitude;

        Vector3 offset = (motion.GravityDirection * v) + (motion.Camera.right * h);

        motion.Rigidbody.MovePosition(Vector3.Lerp(
            motion.Rigidbody.transform.position,
            motion.HoverPosition + offset,
            Time.deltaTime
        ));
    }
}
