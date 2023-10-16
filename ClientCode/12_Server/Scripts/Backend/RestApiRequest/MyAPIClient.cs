
using System;
using System.Collections;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;
using Newtonsoft.Json;

public class MyAPIClient : MonoBehaviour
{
    public static MyAPIClient instance;

    [Serializable]
    public class ResponseDTO
    {
        public string msg;
        public string errMsg;
        public string httpStatus;
        public object data;
    }
    [Serializable]
    public class InfoResponseDto
    {
        public string nickname;
        public int coin;
        public int exp;
        public bool isHammerUnlocked;
        public bool isDeveloperUnlocked;
        public bool isAlyakOK;
    }

    private UnityWebRequest request;
        private string jsonString;
        private byte[] jsonToSend;
        private ResponseDTO response;
    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }else
        {
            Destroy(instance);
            instance = this;
        }
        
    }

    //public static readonly string BACKEND_URL = "http://localhost:8200/";
    public static readonly string BACKEND_URL = "http://t0902.p.ssafy.io:8200/";
    //singleton

    public IEnumerator getRequest(string url, System.Action<ResponseDTO> onSuccess, System.Action<ResponseDTO> onError)
    {

            // get 방식으로 전송
            using (request = UnityWebRequest.Get(url))
            { 
                // 응답이 올때까지 기다린 후 return
                yield return request.SendWebRequest();


                if (request.result != UnityWebRequest.Result.Success)
                {
                    Debug.Log("Get 요청 실패!");
                onError?.Invoke(null);
            }
            
                else
                {
                    Debug.Log("Get 요청 성공!");

                jsonString = request.downloadHandler.text;    // string 값으로 변경
                response = JsonConvert.DeserializeObject<ResponseDTO>(jsonString);

                InfoResponseDto infoResponseDto = JsonConvert.DeserializeObject<InfoResponseDto>(JsonConvert.SerializeObject(response.data));


                onSuccess?.Invoke(response);
            }
            }
    }

    public IEnumerator PostRequest(string url, string data, System.Action<ResponseDTO> onSuccess, System.Action<ResponseDTO> onError)
    {
        using (UnityWebRequest request = UnityWebRequest.Post(url, data))
        {
            // Json parse
            jsonToSend = Encoding.UTF8.GetBytes(data);
            // Reqeust 
            request.uploadHandler = new UploadHandlerRaw(jsonToSend);
            request.downloadHandler = new DownloadHandlerBuffer();
            request.SetRequestHeader("Content-Type", "application/json");
            // send Request
            yield return request.SendWebRequest();

            if (request.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError($"Error: {request.error}");
                Debug.Log("POST 전송 실패!!");
                onError?.Invoke(null);
            }
            else
            {
                Debug.Log("POST 전송 성공!");
                Debug.Log($"Received: {request.downloadHandler.text}");
                string jsonString = request.downloadHandler.text;
                response = JsonUtility.FromJson<ResponseDTO>(jsonString);

                onSuccess?.Invoke(response);
            }
        }
    }


}
