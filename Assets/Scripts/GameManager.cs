using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public static ShipType? currentShipType = null;
    public static bool backFromGameScene = false;
    private AudioSource audioSource;
    [SerializeField] AudioClip[] clips;

    // Start is called before the first frame update
    void Start()
    {
        if (instance == null)        
            instance = this;
        else if (instance != this)
            Destroy(this.gameObject);

        DontDestroyOnLoad(this.gameObject);
        
        audioSource = GetComponent<AudioSource>();
        audioSource.clip = clips[0];
        audioSource.Play();
    }

    public void BackToMenu()
    {
        SceneManager.LoadScene("Menu");
        audioSource.clip = clips[0];
        audioSource.Play();
    }

    public void StartGame()
    {
        SceneManager.LoadScene("ORecomecoComArte");
        audioSource.clip = clips[1];
        audioSource.Play();
    }
}
