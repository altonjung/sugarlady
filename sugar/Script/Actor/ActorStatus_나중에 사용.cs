using System;
using System.Collections.Generic;

[Serializable]
public class ActorStatus
{
    public string name;
    public string status;   // normal, fight, illness

    public float  level;

    public float  health;
    public float  power;
    public float  strength;

    public int    coin;    
}
