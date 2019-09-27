using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Menu : MonoBehaviour {

	[SerializeField] InputField loginUsername, loginPassword;
	[SerializeField] InputField signupUsername, signupPassword, signupConfirm, signupEmail;
	[SerializeField] TextMeshProUGUI errorMessage;
	public bool login = true;
	public GameObject loginTab, signupTab, mainTab, logoutButton, shipTab, statsTab, loadingPanel;

	// Use this for initialization
	void Start ()
	{
		Initialize();
	}
	
	public void Initialize(){
		logoutButton.SetActive(login);
		if(GameManager.backFromGameScene)
		{
			mainTab.SetActive(true);
			shipTab.SetActive(true);
			statsTab.SetActive(true);
			GameManager.backFromGameScene = false;
		}
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
			ShowLoginError("Insira um username");
		else if(loginPassword.text.Length == 0)
			ShowLoginError("Insira uma senha");
		else
		{
			loadingPanel.SetActive(true);
			NetworkManager.instance.AttemptLogin(loginUsername.text, loginPassword.text, FinishLogin, ShowLoginError);
		}
	}

	public void AttemptSignup()
	{
		if(signupUsername.text.Length == 0)
			ShowLoginError("Insira um username");
		else if(signupPassword.text.Length <= 6)
			ShowLoginError("Sua senha deve ter mais que 6 caracteres");
		else if(signupPassword.text != signupConfirm.text)
			ShowLoginError("Suas senhas não são iguais");
		else
		{
			loadingPanel.SetActive(true);
			NetworkManager.instance.AttemptSignup(signupUsername.text, signupPassword.text, signupEmail.text, FinishLogin, ShowLoginError);
		}
	}

	void ShowLoginError(string errorMessage)
	{
		loadingPanel.SetActive(false);
		this.errorMessage.text = errorMessage;
	}

	public void FinishLogin(Token token)
	{
		loadingPanel.SetActive(false);
		errorMessage.text = "";
		NetworkManager.token = token.token;
		loginTab.SetActive(false);
		signupTab.SetActive(false);
		logoutButton.SetActive(true);
		login = true;
		mainTab.SetActive(true);

	}

	public void PlayGame()
	{
		GameManager.StartGame();
	}
}