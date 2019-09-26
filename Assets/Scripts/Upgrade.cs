using UnityEngine;
using System.Collections.Generic;
 
[System.Serializable]
public class Upgrade
{
    public int[] cost;
    public float[] value;
}

public class Upgrades
{
    public Upgrade[] upgrades;
}