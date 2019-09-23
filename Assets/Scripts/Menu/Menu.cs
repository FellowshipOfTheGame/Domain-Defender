using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour {

	public bool login = true;
	public GameObject loginTab, mainTab, logoutButton;

	// Use this for initialization
	void Start () {
		Initialize();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void Initialize(){
		logoutButton.SetActive(login);
	}

	public void Logout(){
		login = false;
		logoutButton.SetActive(false);
	}

	public void checkLogin(){
		if (login){
			mainTab.SetActive(true);
		}else{
			loginTab.SetActive(true);
		}
	}

	public void finishLogin(){
		loginTab.SetActive(false);
		logoutButton.SetActive(true);
		login = true;
	}


	public void PlayGame()
	{
		SceneManager.LoadScene("ORecomecoComArte");
	}
}