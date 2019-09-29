using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pause : MonoBehaviour
{
    public static Pause instance;
    [SerializeField] GameObject pausePanel;
    private bool canChangeState = true;
    public bool paused = false;
    [SerializeField] Shoot player;

    public bool CanChangeState
    {
        get { return canChangeState; }
        set 
        {
            canChangeState = value;
            player.CanShoot(value);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(this.gameObject);

        pausePanel.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
            TryToChangeState();
        
    }

    public void TryToChangeState()
    {
        if (canChangeState) 
        {
            if (paused)
                ResumeGame();
            else
                PauseGame();
        }
        
    }

    public void PauseGame()
    {
        player.CanShoot(false);
        paused = true;
        Time.timeScale = 0;
        pausePanel.SetActive(true);
    }

    public void ResumeGame()
    {
        player.CanShoot(true);
        paused = false;
        Time.timeScale = 1;
        pausePanel.SetActive(false);
    }

    public void Menu()
    {
        Time.timeScale = 1;
        GameManager.instance.BackToMenu();
    }
}
