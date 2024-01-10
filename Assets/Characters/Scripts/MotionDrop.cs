using UnityEngine;

public class MotionDrop
{
    public void Do(Motion motion)
    {
        motion.IsHovering = false;
        motion.IsStable = false;
        motion.IsLifting = false;
        motion.IsDiving = true;
        motion.MGravity.ResetGravityDirection(motion);
    }
}
