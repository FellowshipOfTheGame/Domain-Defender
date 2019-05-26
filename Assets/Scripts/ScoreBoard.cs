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
	private int score = 0;
	
	public int Score { get { return score; } }

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
	}

	/// <summary>
	/// Updates score
	/// </summary>
	public void OnEnemyDeath ()
	{
		score++;
		scoreText.text = "Score: " + score.ToString();
	}
}