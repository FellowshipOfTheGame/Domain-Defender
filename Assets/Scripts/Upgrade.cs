using UnityEngine;
using System.Collections.Generic;
 
[System.Serializable]
public class Upgrade
{
    [SerializeField]
    public int[] cost;
    [SerializeField]
    public float[] value;
}

[System.Serializable]
public class Upgrades
{
    [SerializeField]
    public Upgrade[] upgrades;
}