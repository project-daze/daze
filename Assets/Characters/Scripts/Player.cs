using System;
using UnityEngine;
using Daze.Characters;

[RequireComponent(typeof(Rigidbody))]
public class Player : MonoBehaviour
{
    public Transform Camera;

    public Inputs Inputs = new();
    public Motion Motion = new();
    public Vfx Vfx = new();

    public event Action OnGrounded;
    public event Action<float> OnDiving;
    public event Action OnHover;
    public event Action<float> OnDiveMove;

    public Debug Debug = new();

    private void Awake()
    {
        Inputs.OnAwake(this);
        Motion.OnAwake(this);
        Vfx.OnAwake(this);
    }

    private void OnEnable()
    {
        Inputs.OnEnable();
    }

    private void OnDisable()
    {
        Inputs.OnDisable();
    }

    private void FixedUpdate()
    {
        Motion.OnFixedUpdate();
    }

    public void EmitGrounded()
    {
        OnGrounded?.Invoke();
    }

    public void EmitDiving(float magnitude)
    {
        OnDiving?.Invoke(magnitude);
    }

    public void EmitHover()
    {
        OnHover?.Invoke();
    }

    public void EmitDiveMove(float magnitude)
    {
        OnDiveMove?.Invoke(magnitude);
    }

    private void OnGUI()
    {
        Debug.OnGUI();
    }
}
