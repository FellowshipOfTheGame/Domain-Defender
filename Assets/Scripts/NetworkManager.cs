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

    public delegate void OnStringAnswer(string data);
    public delegate void OnObjectReturn<T>(T stats);

    public static IEnumerator GetRequest<T>(string uri, OnObjectReturn<T> callback)
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
            callback(JsonUtility.FromJson<T>(uwr.downloadHandler.text));
        }
    }

    public static IEnumerator PostRequest<T>(string url, WWWForm form, OnObjectReturn<T> callback)
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
            callback(JsonUtility.FromJson<T>(uwr.downloadHandler.text));
        }
    }
}
