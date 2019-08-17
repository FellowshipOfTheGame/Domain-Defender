using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_AnimCtrl : MonoBehaviour {

	public Animator anim;
	public Vector3[] pos;
	int p=0;
	bool canMove=false;
	
	// Update is called once per frame
	void Update () {
		if(this.transform.position != pos[p]){
			if(canMove){
				this.transform.position = Vector3.Lerp(this.transform.position, pos[p], 0.5f);
				if((this.transform.position - pos[p]).magnitude <= 0.1f){
					this.transform.position = pos[p];
					canMove=false;
					endShift();
				}
			}
		}else{
			if(Input.GetKeyDown(KeyCode.RightArrow) && p==0){
				p=1;
				startShift(1);
				Invoke("Move", 0.2f);
			}else if(Input.GetKeyDown(KeyCode.LeftArrow) && p==1){
				p=0;
				Invoke("Move", 0.2f);
				startShift(0);
			}
		}

		if(Input.GetKeyDown(KeyCode.Space))
			anim.SetTrigger("die");
	}

	void Move(){
		canMove=true;
	}

	public void startShift(int i){
		anim.SetInteger("pos", i);
		anim.SetTrigger("move");
	}

	public void endShift(){
		anim.SetTrigger("stop");
	}


}
