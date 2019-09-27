using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletAnimHandle : MonoBehaviour {

	static int doubleCadencePos = 1;
	public static bool cadence = false, drill = false;
	public GameObject[] bullets;

	public static float alternance = 0.07f;

	// Use this for initialization
	void Start () {
		
	}

	public void Initialize(){
		if(cadence){
			int index = 0;
			if(!drill){
				bullets[0].SetActive(false);
				bullets[1].SetActive(true);
				index = 1;
			}

			bullets[index].transform.localPosition = Vector3.right * alternance * doubleCadencePos;
			doubleCadencePos = - doubleCadencePos;
		}else if (!drill){
			bullets[1].SetActive(false);
			bullets[0].SetActive(true);
		}
	}
	
	// Update is called once per frame
	void Update () {
	}
}
