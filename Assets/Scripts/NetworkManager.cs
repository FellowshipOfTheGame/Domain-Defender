using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class NetworkManager : MonoBehaviour
{
    public static NetworkManager instance;

    private void Start()
    {
        if(instance == null)
            instance = this;
        else if(instance != this)
            Destroy(this);
    }

    public static string baseUrl = "localhost:3000";
    public static string token = null;

    public static IEnumerator GetRequest(string uri)
    {
        UnityWebRequest uwr = UnityWebRequest.Get(baseUrl + uri);

        if(token != null)
            uwr.SetRequestHeader("authorization", token);

        yield return uwr.SendWebRequest();

        if (uwr.isNetworkError)
        {
            Debug.Log("Error While Sending: " + uwr.error);
        }
        else
        {
            Debug.Log("Received: " + uwr.downloadHandler.text);
        }
    }

    public delegate void OnReceived(string data);

    public static IEnumerator PostRequest(string url, WWWForm form, OnReceived callback)
    {
        // WWWForm form = new WWWForm();
        // form.AddField("myField", "myData");
        // form.AddField("Game Name", "Mario Kart");

        UnityWebRequest uwr = UnityWebRequest.Post(baseUrl + url, form);

        if(token != null)
            uwr.SetRequestHeader("authorization", token);

        yield return uwr.SendWebRequest();

        if (uwr.isNetworkError)
        {
            Debug.Log("Error While Sending: " + uwr.error);
        }
        else
        {
            Debug.Log("Received: " + uwr.downloadHandler.text);
            // if(url == "/login")
            //     token = uwr.downloadHandler.text;

            callback(uwr.downloadHandler.text);
        }
    }

    // public void AddCoins();
    // public void Upgrade();
    // public void SubmitScore();
    // public void GetPlayerInfo();
    // void SubmitUpgradePrices();
}
