using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimManager : MonoBehaviour {

	public static AnimManager instance;
	public Sprite[] shipSprites;
	public Animator playerAnim;
	public SpriteRenderer playerArt, playerArtMask;
	
	public void SetShip(int i){ //0=debian; 1=arch; 2=ubuntu; 3=fedora
		playerArt.sprite = shipSprites[2*i];
		playerArtMask.sprite = shipSprites[2*i+1];
	}

	public void MoveLeft(){ //active the rotate left animation
		playerAnim.SetInteger("pos", 0);
		playerAnim.SetTrigger("move");
		playerAnim.ResetTrigger("stop");
	}

	public void MoveRight(){ //active the rotate right animation
		playerAnim.SetInteger("pos", 0);
		playerAnim.SetTrigger("move");
		playerAnim.ResetTrigger("stop");
	}

	public void Stop(){  //active the animation to stop the rotation
		playerAnim.SetTrigger("stop");
	}

	public void GameOver(){ //active all the game over animation
		playerAnim.SetTrigger("die");
	}

	void Awake(){
		if (instance != null)
			Destroy(this.gameObject);
		else
			instance = this;
	}

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
