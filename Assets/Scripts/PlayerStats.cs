using UnityEngine;
using System.Collections.Generic;
 

[System.Serializable]
public class PlayerStats
{
    public string username;
    public int money;
    public int[] upgradeLevel;
    public int highScore;
    public int lastLogin;
    
    public int this[StatType index]
    {
        get => upgradeLevel[(int)index];
        set => upgradeLevel[(int)index] = value;
    }
}
public enum StatType { FireRate, Damage, NumOfBullets, Shield, FireRateBoost, PenetratingShots }