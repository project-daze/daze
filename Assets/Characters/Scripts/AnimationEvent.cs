using System;
using UnityEngine;

public class AnimationEvent : MonoBehaviour
{
    public event Action OnWalkL;
    public event Action OnWalkR;

    public event Action OnRunL;
    public event Action OnRunR;
    public event Action OnRunStopR;
    public event Action OnRunJumpRise;

    public event Action OnJumpRise;
    public event Action OnJumpLand;

    public event Action OnDiveLand;

    public void WalkL()
    {
        OnWalkL?.Invoke();
    }

    public void WalkR()
    {
        OnWalkR?.Invoke();
    }

    public void RunL()
    {
        OnRunL?.Invoke();
    }

    public void RunR()
    {
        OnRunR?.Invoke();
    }

    public void RunStopR()
    {
        OnRunStopR?.Invoke();
    }

    public void RunJumpRise()
    {
        OnRunJumpRise?.Invoke();
    }

    public void JumpRise()
    {
        OnJumpRise?.Invoke();
    }

    public void JumpLand()
    {
        OnJumpLand?.Invoke();
    }

    public void DiveLand()
    {
        OnDiveLand?.Invoke();
    }
}
