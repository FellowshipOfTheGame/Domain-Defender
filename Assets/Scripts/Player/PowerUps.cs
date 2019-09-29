using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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
    public PlayerStats playerStats;
    public int upgradesLevel;
    public float dps;

    [SerializeField] private LoadingPanel loadingPanel;


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

        // TODO: Tela de loading. Sugestão: usar classe LoadingPanel em um objeto de ui de painel.
        // ! Veja o script stat upgrade menu para exemplos.

        Time.timeScale = 0;
        loadingPanel.StartLoading("Carregando...");
        Pause.instance.CanChangeState = false;
        StartCoroutine(NetworkManager.GetRequest<PlayerStats>("/player", GetDataCallback, Error));
        
    }
    
    private void Error(string errorMessage)
    {
        Debug.Log("Error: " + errorMessage);

        Button.ButtonClickedEvent backToMenu = new Button.ButtonClickedEvent();
        backToMenu.AddListener(() => GameManager.instance.BackToMenu());
        loadingPanel.ShowError("Ops! Ocorreu um erro!", errorMessage, "Voltar", backToMenu);
        
        // TODO: Lidar com o erro. Sugestão: usar classe LoadingPanel em um objeto de ui de painel.
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
        playerShoot.numberOfHits = numberOfHits;
        playerShoot.numberOfBullets = numberOfBullets;
     
        CalculateUpgradesLevel();
        CalculateDPS();
        Spawner.instance.Initialize(dps);    
     
     
        loadingPanel.StopLoading();
        Time.timeScale = 1;

        Pause.instance.CanChangeState = true;
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
        BulletAnimHandle.drill = true;
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
        BulletAnimHandle.drill = false;
        CancelInvoke("ResetProjectile");
    }
    
    /// <summary>
    /// Changes the cadence for some time, and then resets it
    /// </summary>
    public void CadencePowerUp()
    {
        CancelInvoke("ResetCadence");
        playerShoot.cooldown = baseCadence / cadenceMultiplier;
        BulletAnimHandle.cadence = true;
        Invoke("ResetCadence", cadencePowerUpDuration);
    }

    /// <summary>
    /// Resets shoot cadence
    /// </summary>
    private void ResetCadence()
    {
        playerShoot.cooldown = baseCadence;
        BulletAnimHandle.cadence = false;
        CancelInvoke("ResetCadence");
    }

    private void CalculateDPS()
    {
        float damage = StatUpgradeMenu.upgradeableStats[(int)StatType.Damage].value[playerStats[StatType.Damage]];
        float bullets = StatUpgradeMenu.upgradeableStats[(int)StatType.NumOfBullets].value[playerStats[StatType.NumOfBullets]];
        float fireRate = StatUpgradeMenu.upgradeableStats[(int)StatType.FireRate].value[playerStats[StatType.FireRate]];
        
        dps = damage * bullets * fireRate;
    }

    private void CalculateUpgradesLevel()
    {
        upgradesLevel = 1;

        for (int i = 0; i < playerStats.upgradeLevel.Length; i++)
            upgradesLevel += playerStats.upgradeLevel[i];
    }

    /// <summary>
    /// Returns true if everything is ok, and false if a cheat is detected
    /// </summary>
    /// <returns></returns>
    public bool CheckUpgrades()
    {
        if (Mathf.Abs(baseCadence - (1f / StatUpgradeMenu.upgradeableStats[(int)StatType.FireRate].value[playerStats[StatType.FireRate]])) > 0.001f)
        {
            Debug.Log("Upgrade Cheat: base cadence\n" + baseCadence + " != " + 1f / StatUpgradeMenu.upgradeableStats[(int)StatType.FireRate].value[playerStats[StatType.FireRate]]);
            return false;
        }
        if (Mathf.Abs(cadenceMultiplier - StatUpgradeMenu.upgradeableStats[(int)StatType.FireRateBoost].value[playerStats[StatType.FireRateBoost]]) > 0.001f)
        {
            Debug.Log("Upgrade Cheat: cadence multiplier");
            return false;
        }
        if (numberOfBullets != (int)StatUpgradeMenu.upgradeableStats[(int)StatType.NumOfBullets].value[playerStats[StatType.NumOfBullets]])
        {
            Debug.Log("Upgrade Cheat: number of bullets");
            return false;
        }
        if (Mathf.Abs(shieldPowerUpDuration - StatUpgradeMenu.upgradeableStats[(int)StatType.Shield].value[playerStats[StatType.Shield]]) > 0.001f)
        {
            Debug.Log("Upgrade Cheat: shield duration");
            return false;
        }
        if (numberOfHits != (int)StatUpgradeMenu.upgradeableStats[(int)StatType.PenetratingShots].value[playerStats[StatType.PenetratingShots]])
        {
            Debug.Log("Upgrade Cheat: number of hits");
            return false;
        }
        if (damage != (int)StatUpgradeMenu.upgradeableStats[(int)StatType.Damage].value[playerStats[StatType.Damage]])
        {
            Debug.Log("Upgrade Cheat: damage");
            return false;
        }

        if (playerShoot.numberOfBullets != numberOfBullets)
        {
            Debug.Log("Upgrade Cheat: player shoot numebr of bullets");
            return false;
        }
        if (playerShoot.numberOfHits != numberOfHits)
        {
            Debug.Log("Upgrade Cheat: player shoot number of hits");
            return false;
        }
        if (playerShoot.damage != damage)
        {
            Debug.Log("Upgrade Cheat: player shoot damage");
            return false;
        }

        return true;
    }
}