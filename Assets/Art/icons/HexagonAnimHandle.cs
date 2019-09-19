using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HexagonAnimHandle : MonoBehaviour {

    public Animator core, shield;


    //back all states do start
    public void Reset(){
        core.SetTrigger("reset");
        shield.SetTrigger("reset");
    }

    //active = true -> turn on shield
    //active = false -> turno off shield
    public void SetShield(bool active){
        if(active)
            shield.SetTrigger("on");
        else
            shield.SetTrigger("off");
        
    }

    //final animation
    public void GameOver(){
        core.SetTrigger("die");
    }
}
