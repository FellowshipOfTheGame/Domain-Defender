using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Mute : MonoBehaviour
{
    [SerializeField] bool muted;
    public bool Muted
    {
        get => muted;
        set
        {
            muted = value;
            GameManager.instance.ChangeSoundState(!muted);
        }
    }

    void Start()
    {
        Muted = !GameManager.instance.muted;
        GetComponent<Toggle>().isOn = muted;
    }
}
