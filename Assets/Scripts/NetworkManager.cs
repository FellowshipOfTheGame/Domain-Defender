using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class NetworkManager : MonoBehaviour
{
    public static NetworkManager instance;
    [SerializeField] int timeout;

    private void Start()
    {
        if(instance == null)
            instance = this;
        else if(instance != this)
            Destroy(this);
    }

    public static string baseUrl = "https://bixoquest.icmc.usp.br:443";
    public static string token = null;
    private static string unity_token = "66B1132A0173910B01EE3A15EF4E69583BBF2F7F1E4462C99EFBE1B9AB5BF808";

    public delegate void OnStringAnswer(string data);
    public delegate void OnObjectReturn<T>(T stats);
    public delegate void OnError(string error);

    public static IEnumerator GetRequest<T>(string uri, OnObjectReturn<T> callback, OnError errorCallback)
    {
        UnityWebRequest uwr = UnityWebRequest.Get(baseUrl + uri);
        uwr.timeout = NetworkManager.instance.timeout;

        if(token != null)
        {
            uwr.SetRequestHeader("authorization", token);
            uwr.SetRequestHeader("unity_token", unity_token);
        }

        yield return uwr.SendWebRequest();

        if (uwr.isNetworkError || uwr.isHttpError)
        {
            Debug.Log("Error While Sending: " + uwr.error);
            errorCallback(uwr.error);
        }
        else
        {
            Debug.Log("Received: " + uwr.downloadHandler.text);
             if(uwr.responseCode == 200) // No error
                callback(JsonUtility.FromJson<T>(uwr.downloadHandler.text));
            else
                errorCallback(JsonUtility.FromJson<Message>(uwr.downloadHandler.text).message);
        }
    }

    public static IEnumerator PostRequest<T>(string url, WWWForm form, OnObjectReturn<T> callback, OnError errorCallback)
    {
        UnityWebRequest uwr = UnityWebRequest.Post(baseUrl + url, form);
        uwr.timeout = NetworkManager.instance.timeout;

        if(token != null)
        {
            uwr.SetRequestHeader("authorization", token);
            uwr.SetRequestHeader("unity_token", unity_token);
        }

        yield return uwr.SendWebRequest();

        if (uwr.isNetworkError || uwr.isHttpError)
        {
            Debug.Log("Error While Sending: " + uwr.error);
            errorCallback(uwr.error);
        }
        else
        {
            Debug.Log("Received: " + uwr.downloadHandler.text);
             if(uwr.responseCode == 200) // No error
                callback(JsonUtility.FromJson<T>(uwr.downloadHandler.text));
            else
                errorCallback(JsonUtility.FromJson<Message>(uwr.downloadHandler.text).message);
        }
    }

    public void AttemptLogin(string username, string password, OnObjectReturn<Token> callback, OnError errorCallback)
    {
        string uri = "/login";
        WWWForm form = new WWWForm();
        form.AddField("username", username);
        form.AddField("password", password);

        StartCoroutine(PostRequest(uri, form, callback, errorCallback));
    }

    public void AttemptSignup(string username, string password, string email, OnObjectReturn<Token> callback, OnError errorCallback)
    {
        string uri = "/register";
        WWWForm form = new WWWForm();
        form.AddField("username", username);
        form.AddField("password", password);
        form.AddField("email", email);

        StartCoroutine(PostRequest(uri, form, callback, errorCallback));
    }
}

[System.Serializable]
public class Message
{
    public string message;
}