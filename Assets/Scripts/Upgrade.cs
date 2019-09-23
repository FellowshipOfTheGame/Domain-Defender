using UnityEngine;
using System.Collections.Generic;
 
[System.Serializable]
public class Upgrade
{
    public int[] cost;
    public int[] value;
}

public class Upgrades
{
    public Upgrade[] upgrades;
}