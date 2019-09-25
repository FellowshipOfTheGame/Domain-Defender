using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public static ShipType? currentShipType = null;
    public static bool backFromGameScene = false;

    // Start is called before the first frame update
    void Start()
    {
        if (instance == null)        
            instance = this;
        else if (instance != this)
            Destroy(this.gameObject);

        DontDestroyOnLoad(this.gameObject);
    }

    public static void StartGame()
    {
        SceneManager.LoadScene("ORecomecoComArte");
    }
}
