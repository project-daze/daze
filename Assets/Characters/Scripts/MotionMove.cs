using UnityEngine;

public class MotionMove
{
    private Vector3 _moveDirection = Vector3.zero;
    private Vector3 _diveDirection = Vector3.zero;

    private readonly int _isWalkingHash = Animator.StringToHash("IsWalking");
    private readonly int _isWalkingLastFrameHash = Animator.StringToHash("IsWalkingLastFrame");
    private readonly int _isRunningHash = Animator.StringToHash("IsRunning");
    private readonly int _isRunningLastFrameHash = Animator.StringToHash("IsRunningLastFrame");

    public void OnFixedUpdate(Motion motion)
    {
        UpdateDirection(motion);
        UpdateWalk(motion);
        UpdateRun(motion);
        UpdateJumpMove(motion);
        UpdateDiveMove(motion);
    }

    private void UpdateDirection(Motion motion)
    {
        UpdateMoveDirection(motion);
        UpdateDiveDirection(motion);
    }

    private void UpdateMoveDirection(Motion motion)
    {
        if (motion.IsDiving) return;

        Vector3 right = motion.Camera.right;
        Vector3 forward = Vector3.Cross(right, -motion.GravityDirection).normalized;

        Vector3 forwardRelativeVInput = forward * motion.MoveDirection.y;
        Vector3 rightRelativeHInput = right * motion.MoveDirection.x;

        _moveDirection = forwardRelativeVInput + rightRelativeHInput;
    }

    private void UpdateDiveDirection(Motion motion)
    {
        if (!motion.IsDiving) return;

        _diveDirection = motion.MoveDirection;
        motion.Player.EmitDiveMove(_diveDirection.x);
    }

    private void UpdateWalk(Motion motion)
    {
        motion.Animator.SetBool(_isWalkingLastFrameHash, motion.IsWalking);
        if (!CanWalk(motion))
        {
            motion.IsWalking = false;
            motion.Animator.SetBool(_isWalkingHash, false);
            return;
        }

        motion.IsWalking = true;
        motion.IsRunning = false;

        float speed = motion.Animator.GetCurrentAnimatorStateInfo(0).IsName("Kat@Walk_Start")
            ? motion.WalkStartSpeed
            : motion.WalkIdleSpeed;

        motion.Rigidbody.velocity = _moveDirection.normalized * (speed * Time.fixedDeltaTime);

        UpdateMoveRotation(motion);

        motion.Animator.SetBool(_isWalkingHash, true);
    }

    private bool CanWalk(Motion motion)
    {
        return (
            motion.IsGrounded
            && motion.MoveDirection.magnitude > 0.1f
            && motion.MoveDirection.magnitude < 0.6f
        );
    }

    private void UpdateRun(Motion motion)
    {
        motion.Animator.SetBool(_isRunningLastFrameHash, motion.IsRunning);
        if (!CanRun(motion))
        {
            motion.IsRunning = false;
            motion.Animator.SetBool(_isRunningHash, false);
            return;
        }

        motion.IsWalking = false;
        motion.IsRunning = true;

        motion.Rigidbody.velocity = _moveDirection.normalized * (motion.RunSpeed * Time.fixedDeltaTime);
        UpdateMoveRotation(motion);
        motion.Animator.SetBool(_isRunningHash, true);
    }

    private bool CanRun(Motion motion)
    {
        return (
            motion.IsGrounded
            && motion.MoveDirection.magnitude > 0.6f
        );
    }

    private void UpdateJumpMove(Motion motion)
    {
        if (!motion.IsJumpRising && !motion.IsJumpFalling) return;

        motion.Rigidbody.AddForce(_moveDirection * (10f * Time.fixedDeltaTime), ForceMode.VelocityChange);
        UpdateMoveRotation(motion);
    }

    private void UpdateMoveRotation(Motion motion)
    {
        if (_moveDirection == Vector3.zero) return;

        Quaternion rotation = Quaternion.LookRotation(_moveDirection, motion.Rigidbody.transform.up);
        Quaternion newRotation = Quaternion.RotateTowards(motion.Rigidbody.rotation, rotation, motion.TurnSpeed * Time.fixedDeltaTime);
        motion.Rigidbody.MoveRotation(newRotation);
    }

    private void UpdateDiveMove(Motion motion)
    {
        if (!motion.IsDiving) return;

        UpdateDiveMoveGravityRotation(motion);
        UpdateDiveMoveBodyRotation(motion);
    }

    private void UpdateDiveMoveGravityRotation(Motion motion)
    {
        if (_diveDirection == Vector3.zero) return;

        float inputX = _diveDirection.x;
        float inputY = _diveDirection.y;

        Vector3 cameraUp = motion.Camera.up;
        Vector3 cameraRight = motion.Camera.right;

        Vector3 rotation = cameraUp * inputY + cameraRight * inputX;

        motion.GravityDirection = Vector3.Lerp(
            motion.GravityDirection,
            rotation,
            motion.DiveMoveSpeed * Time.deltaTime
        ).normalized;
    }

    private void UpdateDiveMoveBodyRotation(Motion motion)
    {
        if (_diveDirection == Vector3.zero)
        {
            motion.DiveTurnSpeed = 0.0f;
            return;
        }

        float speed = _diveDirection.x * motion.DiveMaxTurnSpeed;
        motion.DiveTurnSpeed = Mathf.Lerp(motion.DiveTurnSpeed, speed, motion.DiveTurnAcceleration * Time.deltaTime);
        motion.Rigidbody.AddRelativeTorque(-motion.DiveTurnSpeed * Vector3.up, ForceMode.Acceleration);
    }
}
