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
    PlayerStats playerStats;


    private enum Projectiles { normal = 0, drill = 1 };


    /// <summary>
    /// Gets components and sets variables in player's Shoot script
    /// </summary>
    private void Start()
    {
        if (playerShoot == null)
            playerShoot = GetComponent<Shoot>();


        Login();

    }

    private void Login()
    {
        WWWForm form = new WWWForm();
        form.AddField("email", "asd");
        StartCoroutine(NetworkManager.PostRequest("/login", form, LoginCallback));
    }

    private void LoginCallback(string token)
    {
        NetworkManager.token = token;

        GetData();
    }


    private void GetData()
    {
        Time.timeScale = 0;

        StartCoroutine(NetworkManager.GetRequest("/player", GetDataCallback));
        
    }

    private void GetDataCallback(PlayerStats playerStats)
    {
        this.playerStats = playerStats;
        ResetProjectile();
        ResetCadence();
        playerShoot.numberOfBullets = numberOfBullets;
        Time.timeScale = 1;
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