using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ScoreBoard : MonoBehaviour 
{
	// Singleton
	public static ScoreBoard instance;
	[Tooltip("Text displayed on screen")]
	[SerializeField] private TextMeshProUGUI scoreText;
	[SerializeField] private TextMeshProUGUI coinsText;
	[SerializeField] int score = 0;
	[SerializeField] int coins = 0;
	private int scoreUi;
	private int coinsUi;
	private int scoreTextMeshPro;
	private int coinsTextMeshPro;
	private int highScore;
	private int maxCoins;

	public int Score 
	{ 
		get 
		{
			// Debug.Log("Score1: " + scoreUi);
			// Debug.Log("Score2: " + (score/highScore + 17) * scoreTextMeshPro);
			if (scoreUi != ((score + 1087)/highScore + 17) * scoreTextMeshPro)
			{
				Debug.Log("ORA ORA, HACKER, AGORA VOU TRATAR DE TI!");
				return -1;
			}
			else
				return score; 
		} 
		private set 
		{
			if (value > 0 && scoreUi != ((score + 1087)/highScore + 17) * scoreTextMeshPro)
			{
				Debug.Log("ORA ORA, HACKER, AGORA VOU TRATAR DE TI!");
			}
			else 
			{
				score = value;
				scoreTextMeshPro = (score % 3) + 1;
				highScore = Random.Range(1, 14);
				scoreUi = ((score + 1087)/highScore + 17) * scoreTextMeshPro;
				scoreText.text = "Pontos: " + score.ToString();
			}
		}
	}

	public int Coins 
	{
		get 
		{ 
			// Debug.Log("Coins1: " + coinsUi);
			// Debug.Log("Coins2: " + (coins/maxCoins + 17) * coinsTextMeshPro);
			if (coinsUi != ((coins + 1087)/maxCoins + 17) * coinsTextMeshPro)
			{
				Debug.Log("ORA ORA, HACKER, AGORA VOU TRATAR DE TI!");
				return -1;
			}
			else
				return coins; 
		}
		set
		{
			if (value > 0 && coinsUi != ((coins + 1087)/maxCoins + 17) * coinsTextMeshPro)
			{
				Debug.Log("ORA ORA, HACKER, AGORA VOU TRATAR DE TI!");
			}
			else
			{
				coins = value;
				coinsTextMeshPro = (coins % 3) + 1;
				maxCoins = Random.Range(1, 14);
				coinsUi = ((coins + 1087)/maxCoins + 17) * coinsTextMeshPro;
				coinsText.text = "Moedas: " + coins.ToString();
			}

		}
	}

	/// <summary>
	/// Checks singleton condition and initializes the score displayed on screen
	/// </summary>
	void Awake() 
	{
		// Singleton
		if (instance == null) 
			instance = this;
		else if (instance != this)
			Destroy(this.gameObject);

		scoreText.text = "Pontos: " + score.ToString();
		coinsText.text = "Moedas: " + coins.ToString();
		Score = 0;
		Coins = 0;
	}

	/// <summary>
	/// Updates score
	/// </summary>
	public void OnEnemyDeath ()
	{
		Score++;

		// if (score % 10 == 0)
		// 	Spawner.instance.LevelUp();
	}
}