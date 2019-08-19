using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "new PlayerStats", menuName = "Stats/PlayerStats", order = 1)]
public class PlayerStats : ScriptableObject
{
    public enum StatType { FireRate, Damage, NumOfBullets, Shield, FireRateBoost, PenetratingShots }

    [System.Serializable]
    public class Stat
    {
        [System.Serializable] public class CostList { public List<int> subCosts; }

        public int level;
        public int sublevel;
        public int numOfSubLevels;
        public List<int> values;
        public List<CostList> costs;
        [HideInInspector] public int currentValue;
    }

    public Stat damage, numOfBullets, fireRate, shield, fireRateBoost, penetratingShots;

    public int Coins { get; set; }

    public Stat this[StatType i]
    {
        get { return StatTypeToValue(i); }
        set { StatTypeToValue(i) = value; }
    }

    private ref Stat StatTypeToValue(StatType type)
    {
        switch(type)
        {
            case PlayerStats.StatType.FireRate:
                return ref fireRate;
                
            case PlayerStats.StatType.Damage:
                return ref damage;

            case PlayerStats.StatType.NumOfBullets:
                return ref numOfBullets;

            case PlayerStats.StatType.FireRateBoost:
                return ref fireRateBoost;

            case PlayerStats.StatType.Shield:
                return ref shield;

            case PlayerStats.StatType.PenetratingShots:
                return ref penetratingShots;

            default:
                throw new System.NullReferenceException("Invalid stat. Make sure it's not null.");
        }
    }
}