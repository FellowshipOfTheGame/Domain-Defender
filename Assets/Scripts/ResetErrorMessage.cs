using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ResetErrorMessage : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI errorMessage;
    private void OnDisable()
    {
        errorMessage.text = "";
    }
}
