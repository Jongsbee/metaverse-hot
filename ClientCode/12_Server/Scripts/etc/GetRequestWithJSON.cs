using System;
using System.Collections;
using System.Net;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;


public class TestDto 
{
    public int[] algoIds;
    public int[] languageIds;
    public string title;
    public int id;
}



public class GetRequestWithJSON : MonoBehaviour
{
    public string uri = "https://sccs.kr/api/mypage/history/qwerty123/2023/02";

    void Start()
    {
        TestDto test = new TestDto
        {
            algoIds = new int[] { 1, 2, 3, 4, 5, 6, 7 },
            languageIds = new int[] { 1, 2 },
            title = "",
            id = 0
        };

        

        string json = JsonUtility.ToJson(test);
        Debug.Log(json);
        // A correct website page.
        //StartCoroutine(GetRequest("https://sccs.kr/api/mypage/history/qwerty123/2023/02"));
        StartCoroutine(Upload("https://sccs.kr/api/studyroom/detail", json));
    }

    IEnumerator GetRequest(string uri)
    {
        using (UnityWebRequest webRequest = UnityWebRequest.Get(uri))
        {
            // Request and wait for the desired page.
            yield return webRequest.SendWebRequest();

            string[] pages = uri.Split('/');
            int page = pages.Length - 1;

            switch (webRequest.result)
            {
                case UnityWebRequest.Result.ConnectionError:
                case UnityWebRequest.Result.DataProcessingError:
                    Debug.LogError(pages[page] + ": Error: " + webRequest.error);
                    break;
                case UnityWebRequest.Result.ProtocolError:
                    Debug.LogError(pages[page] + ": HTTP Error: " + webRequest.error);
                    break;
                case UnityWebRequest.Result.Success:
                    Debug.Log(pages[page] + ":\nReceived: " + webRequest.downloadHandler.text);
                    break;
            }
        }
    }

    IEnumerator Upload(string URL, string json)
    {
        using (UnityWebRequest request = UnityWebRequest.Post(URL, json))
        {
            byte[] jsonToSend = Encoding.UTF8.GetBytes(json);
            
            request.uploadHandler = (UploadHandler)new UploadHandlerRaw(jsonToSend);
            request.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
            request.SetRequestHeader("Content-Type", "application/json");
            
            
            
            yield return request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.ConnectionError || request.result == UnityWebRequest.Result.ProtocolError)
            {
                
                Debug.Log(request.error);
                
            }
            else
            {
                Debug.Log(request.downloadHandler.text);
              
            }

           
        }


    }


}
