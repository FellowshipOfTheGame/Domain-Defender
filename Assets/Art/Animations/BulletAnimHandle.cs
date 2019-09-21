using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletAnimHandle : MonoBehaviour {

	static int doubleCadencePos = 0;
	public GameObject[] bullets;

	// Use this for initialization
	void Start () {
		
	}

	public void Initialize(int type){
		foreach (GameObject gb in bullets)
			gb.SetActive(false);

		if (bullets.Length > 0){
			if(type == 0){
				bullets[0].SetActive(true);
			}else{
				bullets[doubleCadencePos].SetActive(true);
				doubleCadencePos=1-doubleCadencePos;
			}
		}
	}
	
	// Update is called once per frame
	void Update () {
	}
}
