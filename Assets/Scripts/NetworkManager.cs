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
    public delegate void OnObjectReturnWithError<T>(T stats, string error);

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

    public static IEnumerator AttemptLogin(bool signup, string username, string password, OnObjectReturnWithError<Token> callback)
    {
        string uri = signup ? "/register" : "/login";
        WWWForm form = new WWWForm();
        form.AddField("username", username);
        form.AddField("password", password);

        UnityWebRequest uwr = UnityWebRequest.Post(baseUrl + uri, form);

        yield return uwr.SendWebRequest();

        if (uwr.isNetworkError)
        {
            Debug.Log("Error While Sending: " + uwr.error);
            callback(null, "Erro de conexão.");
        }
        else
        {
            Debug.Log("Received: " + uwr.downloadHandler.text);
            if(uwr.responseCode == 200)
                callback(JsonUtility.FromJson<Token>(uwr.downloadHandler.text), null);
            else
                callback(null, JsonUtility.FromJson<Message>(uwr.downloadHandler.text).errorMessage);
        }
    }
}

public class Message
{
    public string errorMessage;
}