using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Menu : MonoBehaviour {

	[SerializeField] InputField loginUsername, loginPassword;
	[SerializeField] InputField signupUsername, signupPassword, signupConfirm;
	[SerializeField] TextMeshProUGUI errorMessage;


	public bool login = true;
	public GameObject loginTab, signupTab, mainTab, logoutButton;

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
		NetworkManager.token = null;
		logoutButton.SetActive(false);
	}

	public void checkLogin(){
		if (login){
			mainTab.SetActive(true);
		}else{
			loginTab.SetActive(true);
		}
	}

	public void AttemptLogin()
	{
		if(loginUsername.text.Length == 0)
			ShowError("Insira um username");
		else if(loginPassword.text.Length == 0)
			ShowError("Insira uma senha");
		else
		{
			StartCoroutine(NetworkManager.AttemptLogin(false, loginUsername.text, loginPassword.text, FinishLogin));
		}
	}

	public void AttemptSignup()
	{
		if(signupUsername.text.Length == 0)
			ShowError("Insira um username");
		else if(signupPassword.text.Length <= 6)
			ShowError("Sua senha deve ter mais que 6 caracteres");
		else if(signupPassword.text != signupConfirm.text)
			ShowError("Suas senhas não são iguais");
		else
		{
			StartCoroutine(NetworkManager.AttemptLogin(true, signupUsername.text, signupPassword.text, FinishLogin));
		}
	}

	void ShowError(string errorMessage)
	{
		this.errorMessage.text = errorMessage;
	}

	public void FinishLogin(Token token, string error)
	{
		if(error == null)
		{
			errorMessage.text = "";
			NetworkManager.token = token.token;
			loginTab.SetActive(false);
			signupTab.SetActive(false);
			logoutButton.SetActive(true);
			login = true;
			mainTab.SetActive(true);
		}
		else
		{
			ShowError(error);
		}
	}

	public void PlayGame()
	{
		GameManager.StartGame();
	}
}