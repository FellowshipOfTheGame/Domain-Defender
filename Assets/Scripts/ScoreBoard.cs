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
	
	public int Score {
		get{return score;}
	}

	// Use this for initialization
	void Awake () 
	{
		// Singleton
		if (instance == null) 
		{
			instance = this;
		}
		else if (instance != this)
		{
			Destroy(this);
		}
	}

	// Just debug
	void Update () 
	{
		if (Input.GetMouseButtonDown(0)) 
		{
			OnEnemyDeath();
		}
	}

	// Updates score
	public void OnEnemyDeath ()
	{
		score++;
		scoreText.text = "Score: " + score.ToString();
	}
}