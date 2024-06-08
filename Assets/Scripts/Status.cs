using System;

public enum StatusType
{
    Stun
}
[Serializable] public class Status
{
    public StatusType statusType;
    public int duration;

    public Status(StatusType statusType, int duration)
    {
        this.statusType = statusType;
        this.duration = duration;
    }

    public string GetStatus() { return statusType.ToString(); }
}
