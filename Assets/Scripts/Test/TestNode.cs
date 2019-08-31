using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class TestNode : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.S))
            StartCoroutine(Get("localhost:3000", Debug.Log));
        if(Input.GetKeyDown(KeyCode.W))
            StartCoroutine(Post("localhost:3000", "joao"));
    }

    // void print(string s)
    // {
    //     Debug.Log(s);
    // }

    IEnumerator Get(string url, System.Action<string> callBack)
    {
        using (UnityWebRequest webRequest = UnityWebRequest.Get(url))
        {
            // Request and wait for the desired page.
            yield return webRequest.SendWebRequest();

            if (webRequest.isNetworkError)
            {
                Debug.Log("Error: " + webRequest.error);
            }
            else
            {
                callBack(webRequest.downloadHandler.text);
            }
        }
    }

    IEnumerator Post(string url, string name)
    {
        WWWForm form = new WWWForm();
        form.AddField("name", name);

        using (UnityWebRequest www = UnityWebRequest.Post(url, form))
        {
            yield return www.SendWebRequest();

            if (www.isNetworkError || www.isHttpError)
            {
                Debug.Log(www.error);
            }
            else
            {
                Debug.Log("Form upload complete!");
            }
        }
    }
}
