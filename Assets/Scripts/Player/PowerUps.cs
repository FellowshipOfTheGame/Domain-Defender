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
    [SerializeField] int numberOfBullets;


    private enum Projectiles { normal = 0, drill = 1 };


    /// <summary>
    /// Gets components and sets variables in player's Shoot script
    /// </summary>
    private void Start()
    {
        if (playerShoot == null)
            playerShoot = GetComponent<Shoot>();

        ResetProjectile();
        ResetCadence();
        playerShoot.numberOfBullets = numberOfBullets;
    }


    /// <summary>
    /// Gives a shield to the hexagon
    /// </summary>
    public void ShieldPowerUp()
    {
        hexagon.ActivateShield();
    }

    /// <summary>
    /// Changes the projectile for some time, and then resets it
    /// </summary>
    public void DrillPowerUp()
    {
        CancelInvoke("ResetProjectile");
        playerShoot.projectile = projectilePrefabs[(int)Projectiles.drill];
        Invoke("ResetProjectile", drillPowerUpDuration);
    }

    /// <summary>
    /// Resets projectile
    /// </summary>
    private void ResetProjectile()
    {
        playerShoot.projectile = projectilePrefabs[(int)Projectiles.normal];
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
    }
}