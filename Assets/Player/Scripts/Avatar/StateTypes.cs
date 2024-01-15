namespace Daze.Player.Avatar
{
    public enum StateType
    {
        Ground,
        Lift,
        Hover,
        Fall,
    }

    public enum StateEvent
    {
        GravityOn,
        GravityOff,

        Update,
        FixedUpdate,
        UpdateVelocity,
        UpdateRotation,
        AfterCharacterUpdate
    }
}
