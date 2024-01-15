namespace Daze.Player.Avatar
{
    public enum StateType
    {
        Ground,
        Lift,
        Hover,
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
