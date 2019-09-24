using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUps : MonoBehaviour
{
    [SerializeField] Hexagon hexagon;
    [SerializeField] Shoot playerShoot;

    [Space(5)]
    [SerializeField] GameObject[] projectilePrefabs;

    [Space(5)]
    [SerializeField] float baseCadence;
    [SerializeField] float cadenceMultiplier;
    [SerializeField] float cadencePowerUpDuration;
    [SerializeField] float drillPowerUpDuration;
    [SerializeField] float shieldPowerUpDuration;
    [SerializeField] int numberOfBullets;
    [SerializeField] int numberOfHits;
    [SerializeField] int damage;
    PlayerStats playerStats;


    private enum Projectiles { normal = 0, drill = 1 };


    /// <summary>
    /// Gets components and sets variables in player's Shoot script
    /// </summary>
    private void Start()
    {
        if (playerShoot == null)
            playerShoot = GetComponent<Shoot>();


        GetData();

    }

    private void GetData()
    {
        Time.timeScale = 0;

        StartCoroutine(NetworkManager.GetRequest<PlayerStats>("/player", GetDataCallback));
        
    }

    private void GetDataCallback(PlayerStats playerStats)
    {
        this.playerStats = playerStats;

        baseCadence = 1f / StatUpgradeMenu.upgradeableStats[(int)StatType.FireRate].value[playerStats[StatType.FireRate]];
        cadenceMultiplier = StatUpgradeMenu.upgradeableStats[(int)StatType.FireRateBoost].value[playerStats[StatType.FireRateBoost]];
        numberOfBullets = (int)StatUpgradeMenu.upgradeableStats[(int)StatType.NumOfBullets].value[playerStats[StatType.NumOfBullets]];
        shieldPowerUpDuration = StatUpgradeMenu.upgradeableStats[(int)StatType.Shield].value[playerStats[StatType.Shield]];
        numberOfHits = (int)StatUpgradeMenu.upgradeableStats[(int)StatType.PenetratingShots].value[playerStats[StatType.PenetratingShots]];
        damage = (int)StatUpgradeMenu.upgradeableStats[(int)StatType.Damage].value[playerStats[StatType.Damage]];

        ResetProjectile();
        ResetCadence();
        ResetShield();
        playerShoot.damage = damage;
        playerShoot.hits = numberOfHits;
        playerShoot.numberOfBullets = numberOfBullets;
        Time.timeScale = 1;
    }


    /// <summary>
    /// Gives a shield to the hexagon
    /// </summary>
    public void ShieldPowerUp()
    {
        CancelInvoke("ResetShield");
        hexagon.ActivateShield();
        Invoke("ResetShield", shieldPowerUpDuration);
    }

    public void ResetShield()
    {
        hexagon.DeactivateShield();
        CancelInvoke("ResetShield");
    }

    /// <summary>
    /// Changes the projectile for some time, and then resets it
    /// </summary>
    public void DrillPowerUp()
    {
        CancelInvoke("ResetProjectile");
        playerShoot.projectile = projectilePrefabs[(int)Projectiles.drill];
        playerShoot.drillPowerUp = true;
        Invoke("ResetProjectile", drillPowerUpDuration);
    }

    /// <summary>
    /// Resets projectile
    /// </summary>
    private void ResetProjectile()
    {
        playerShoot.projectile = projectilePrefabs[(int)Projectiles.normal];
        playerShoot.drillPowerUp = false;
        CancelInvoke("ResetProjectile");
    }
    
    /// <summary>
    /// Changes the cadence for some time, and then resets it
    /// </summary>
    public void CadencePowerUp()
    {
        CancelInvoke("ResetCadence");
        playerShoot.cooldown = baseCadence / cadenceMultiplier;
        playerShoot.type = 1;
        Invoke("ResetCadence", cadencePowerUpDuration);
    }

    /// <summary>
    /// Resets shoot cadence
    /// </summary>
    private void ResetCadence()
    {
        playerShoot.cooldown = baseCadence;
        playerShoot.type = 0;
        CancelInvoke("ResetCadence");
    }
}