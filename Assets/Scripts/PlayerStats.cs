using UnityEngine;
using System.Collections.Generic;
 
[System.Serializable]
public class PlayerStats
{
    public int highScore;
    public string email;
    public int team;
    public int lastLogin;
    public int money;
    public int[] upgradeLevel;
    
    public enum StatType { FireRate, Damage, NumOfBullets, Shield, FireRateBoost, PenetratingShots }
}