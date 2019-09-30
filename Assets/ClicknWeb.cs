using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClicknWeb : MonoBehaviour
{
    [SerializeField] public string webpage;
    public void OpenUrl()
    {
        Application.OpenURL(webpage);
    }
}
