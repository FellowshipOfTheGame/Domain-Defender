using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class LoadingPanel : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI loadingMessage;
    [SerializeField] TextMeshProUGUI errorMessage;
    [SerializeField] Button errorButton;
    [SerializeField] TextMeshProUGUI buttonText;

    public void StartLoading(string message)
    {
        loadingMessage.text = message;
        this.gameObject.SetActive(true);
    }

    public void ShowError(string message, string errorMessage)
    {
        errorButton.gameObject.SetActive(true);
        this.errorMessage.text = errorMessage;
        loadingMessage.text = message;
    }

    public void ShowError(string message, string errorMessage, string buttonText, Button.ButtonClickedEvent buttonEvent)
    {
        errorButton.onClick = buttonEvent;
        errorButton.onClick.AddListener(() => this.gameObject.SetActive(false));
        this.buttonText.text = buttonText;
        ShowError(message, errorMessage);
    }

    public void StopLoading()
    {
        errorMessage.text = "";
        errorButton.gameObject.SetActive(false);
        this.gameObject.SetActive(false);
    }
}