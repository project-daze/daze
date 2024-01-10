using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Motion
{
    [NonSerialized] public Player Player;
    [NonSerialized] public Transform Camera;
    [NonSerialized] public Rigidbody Rigidbody;
    [NonSerialized] public Animator Animator;
    [NonSerialized] private AnimationEvent _animationEvent;

    [NonSerialized] public MotionGravity MGravity = new();
    [NonSerialized] public MotionGround MGround = new();
    [NonSerialized] public MotionMove MMove = new();
    [NonSerialized] public MotionJump MJump = new();
    [NonSerialized] public MotionDive MDive = new();
    [NonSerialized] public MotionLift MLift = new();
    [NonSerialized] public MotionHover MHover = new();
    [NonSerialized] public MotionFall MFall = new();
    [NonSerialized] public MotionDrop MDrop = new();

    public LayerMask GroundMask;
    public Transform GroundCheck;
    public float GroundCheckRadius = 0.05f;

    public Vector3 GravityDirection = Vector3.down;
    public float GravityForce = 1200;

    public Vector2 MoveDirection = Vector2.zero;
    public float WalkStartSpeed = 35f;
    public float WalkIdleSpeed = 80f;
    public float RunSpeed = 300f;
    public float TurnSpeed = 800f;
    public float JumpForce = 12f;

    public float DiveSpeed = 0f;
    public float DiveMaxSpeed = 1200f;
    public float DiveAcceleration = 500f;
    public float DiveDeceleration = 0.95f;
    public float DiveMoveSpeed = 0.15f;
    public float DiveTurnSpeed = 0f;
    public float DiveTurnAcceleration = 0.5f;
    public float DiveMaxTurnSpeed = 10f;

    public float LiftAcceleration = 100f;

    public Vector3 HoverPosition = Vector3.zero;
    public float DriftVAmplitude1 = 0.4f;
    public float DriftVAmplitude2 = 0.25f;
    public float DriftVFrequency = 1f;
    public float DriftHAmplitude = 0.25f;
    public float DriftHFrequency = 0.25f;

    public bool IsGrounded = true;

    public bool IsWalking = false;
    public bool IsRunning = false;

    public bool IsJumpPreparing = false;
    public bool IsJumpRising = false;
    public bool IsJumpFalling = false;

    public bool IsHovering = false;
    public bool IsStable = false;
    public bool IsLifting = false;
    public bool IsDiving = false;

    public void OnAwake(Player player)
    {
        Player = player;
        Camera = Player.Camera;

        Rigidbody = Player.GetComponent<Rigidbody>();
        Animator = Player.GetComponentInChildren<Animator>();
        _animationEvent = Player.GetComponentInChildren<AnimationEvent>();

        _animationEvent.OnJumpRise += Jump;
    }

    public void OnFixedUpdate()
    {
        MGravity.OnFixedUpdate(this);
        MGround.OnFixedUpdate(this);
        MMove.OnFixedUpdate(this);
        MJump.OnFixedUpdate(this);
        MLift.OnFixedUpdate(this);
        MHover.OnFixedUpdate(this);
        MFall.OnFixedUpdate(this);
        MDive.OnFixedUpdate(this);
    }

    public void Move(Vector2 direction)
    {
        MoveDirection = direction;
    }

    public void Jump()
    {
        MJump.Do(this);
    }

    public void GravityControl()
    {
        if (IsGrounded)
        {
            MLift.Do(this);
            return;
        }

        if (IsLifting || IsHovering)
        {
            MFall.Forward(this);
            return;
        }

        MHover.Do(this);
        return;
    }

    public void Drop()
    {
        MDrop.Do(this);
    }
}
