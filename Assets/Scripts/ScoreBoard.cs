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
	private int score = 0;
	public int coins = 0;
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
			if (scoreUi != (score/highScore + 17) * scoreTextMeshPro)
			{
				Debug.Log("ORA ORA, HACKER, AGORA VOU TRATAR DE TI!");
				return -1;
			}
			else
				return score; 
		} 
		private set 
		{
			score = value;
			scoreTextMeshPro = (score % 3) + 1;
			highScore = Random.Range(1, 14);
			scoreUi = (score/highScore + 17) * scoreTextMeshPro;

			scoreText.text = "Score: " + score.ToString();
		}
	}

	public int Coins 
	{
		get 
		{ 
			// Debug.Log("Coins1: " + coinsUi);
			// Debug.Log("Coins2: " + (coins/maxCoins + 17) * coinsTextMeshPro);
			if (coinsUi != (coins/maxCoins + 17) * coinsTextMeshPro)
			{
				Debug.Log("ORA ORA, HACKER, AGORA VOU TRATAR DE TI!");
				return -1;
			}
			else
				return coins; 
		}
		set
		{
			coins = value;
			coinsTextMeshPro = (coins % 3) + 1;
			maxCoins = Random.Range(1, 14);
			coinsUi = (coins/maxCoins + 17) * coinsTextMeshPro;

			coinsText.text = "Coins: " + coins.ToString();
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

		scoreText.text = "Score: " + score.ToString();
		coinsText.text = "Coins: " + coins.ToString();
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