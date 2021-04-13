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
    public int gamesPlayed;
    
    public int this[StatType index]
    {
        get => upgradeLevel[(int)index];
        set => upgradeLevel[(int)index] = value;
    }

    public PlayerStats(string username, int money, int[] upgradeLevel, int highScore, int lastLogin, int gamesPlayed)
    {
        this.username = username;
        this.money = money;
        this.upgradeLevel = upgradeLevel;
        this.highScore = highScore;
        this.lastLogin = lastLogin;
        this.gamesPlayed = gamesPlayed;
    }
}
public enum StatType { FireRate, Damage, NumOfBullets, Shield, FireRateBoost, PenetratingShots }