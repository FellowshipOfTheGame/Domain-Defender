using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimTest : MonoBehaviour {

	public PlayerAnimHandle handle;
	Vector3[] pos;
	bool canMove = false;
	int destiny=0;

	// Use this for initialization
	void Start () {
		pos = new Vector3[2];
		pos[0] = 6.0f * Vector3.left;
		pos[1] = 6.0f * Vector3.right;
	}
	
	// Update is called once per frame
	void Update () {
		if(Input.GetKeyDown(KeyCode.RightArrow)){
			handle.MoveRight();
			destiny=1;
			Invoke("Move", 0.2f);
		}
		if(Input.GetKeyDown(KeyCode.LeftArrow)){
			handle.MoveLeft();
			destiny=0;
			Invoke("Move", 0.2f);
		}
		if(Input.GetKeyDown(KeyCode.Space)){
			handle.GameOver();
		}

		if(canMove){
			this.transform.position = Vector3.Lerp(this.transform.position, pos[destiny], 0.3f);
			if ((this.transform.position - pos[destiny]).magnitude <= 0.2f){
				this.transform.position=pos[destiny];
				canMove=false;
				handle.Stop();
			}
		}
	}

	void Move(){
		canMove = true;
	}
}