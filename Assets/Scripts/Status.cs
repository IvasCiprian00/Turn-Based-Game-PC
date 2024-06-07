using System;

public enum Type
{
    Stun
}
[Serializable] public class Status
{
    public Type type;
    public int duration;

    public string GetStatus() { return type.ToString(); }
}
