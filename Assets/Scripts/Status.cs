using System;

public enum StatusType
{
    Stun,
    Burn,
    Bleed
}
[Serializable] public class Status
{
    public StatusType statusType;
    public int duration;
    public int damage;

    public Status(StatusType statusType, int duration, int damage)
    {
        this.statusType = statusType;
        this.duration = duration;
        this.damage = damage;
    }

    public string GetStatus() { return statusType.ToString(); }
}
