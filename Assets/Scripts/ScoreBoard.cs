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
	private int coins = 0;
	
	public int Score { get { return score; } }

	public int Coins 
	{
		get { return coins; }
		set
		{
			coins = value;
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
	}

	/// <summary>
	/// Updates score
	/// </summary>
	public void OnEnemyDeath ()
	{
		score++;
		scoreText.text = "Score: " + score.ToString();

		if (score % 10 == 0)
			Spawner.instance.LevelUp();
	}
}