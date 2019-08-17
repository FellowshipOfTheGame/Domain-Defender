using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimHandle : MonoBehaviour {

	public Sprite[] sprites;
	public Animator anim;
	public SpriteRenderer ship, mask;
	
	public void SetShip(int i){ //0=debian; 1=arch; 2=ubuntu; 3=fedora
		ship.sprite = sprites[2*i];
		mask.sprite = sprites[2*i+1];
	}

	public void MoveLeft(){ //active the rotate left animation
		anim.SetInteger("pos", 0);
		anim.SetTrigger("move");
	}

	public void MoveRight(){ //active the rotate right animation
		anim.SetInteger("pos", 0);
		anim.SetTrigger("move");
	}

	public void Stop(){  //active the animation to stop the rotation
		anim.SetTrigger("stop");
	}

	public void GameOver(){ //active all the game over animation
		anim.SetTrigger("die");
	}
}