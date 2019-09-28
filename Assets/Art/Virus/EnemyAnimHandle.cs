using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAnimHandle : MonoBehaviour
{
    Animator anim;


    void Awake(){
        anim = GetComponent<Animator>();
    }

    public void GetDamage(){
        anim.SetTrigger("dmg");
    }

    public void Explode(){
        anim.SetTrigger("die");
    }

    public void Reset(){
        anim.SetTrigger("reset");
    }
}
