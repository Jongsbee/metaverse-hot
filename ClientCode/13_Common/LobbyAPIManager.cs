using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LobbyAPIManager : MonoBehaviour
{
    private MyAPIClient myAPIClient;
    private PhotonManagerForNewLobby photonManager;
    private string nickname;
    private string url;
    GameObject couponText;

    // Start is called before the first frame update
    void Start()
    {
        myAPIClient = GameObject.Find("PhotonManager").GetComponent<MyAPIClient>();
        photonManager = GameObject.Find("PhotonManager").GetComponent<PhotonManagerForNewLobby>();
        couponText = GameObject.Find("Canvas").transform.Find("LobbyPanel/CouponNotice").gameObject;
        couponText.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void couponSubmit(GameObject gameObject)
    {
        
        nickname = photonManager.responseNickname();
        string couponMsg = gameObject.GetComponent<TextMeshProUGUI>().text;
        url = MyAPIClient.BACKEND_URL + "api/member/coupon";

        Debug.Log($"쿠폰 전송 URL : {url}");
        Debug.Log($"쿠폰메시지 : {couponMsg}");

        string data = "{\"nickname\":\"" + nickname + "\", \"couponString\":\"" + couponMsg + "\"}";
        string trimmedData = data.Replace(" ", "").Replace("\u200B", "");

        Debug.Log($"쿠폰 전송 JSON :  {trimmedData}");
        StartCoroutine(myAPIClient.PostRequest(url, trimmedData, couponSuccess, couponFailed));        

    }

    public void couponSuccess (MyAPIClient.ResponseDTO data)
    {
        if(data.httpStatus == "OK")
        {
            StartCoroutine(noticeCouponCoroutine("정상적으로 쿠폰이" + "\n" +  "등록되었습니다."));

            Debug.Log("쿠폰 성공!");
        }
        else if (data.httpStatus == "IM_USED")
        {
            
            Debug.Log("쿠폰중복");
            StartCoroutine(noticeCouponCoroutine("기존에 등록된" + "\n" +  "쿠폰입니다."));
        }else if (data.httpStatus == "BAD_REQUEST")
        {
            StartCoroutine(noticeCouponCoroutine("올바르지 않은" + "\n" +  "쿠폰입니다."));
        }
        else
        {
            Debug.Log("이거뭐임?");
        }
        
    }

    public void couponFailed(MyAPIClient.ResponseDTO data)
    {
        if (data.httpStatus == "OK")
        {
            Debug.Log("쿠폰 에러!");
        }
        else
        {
            Debug.Log("쿠폰전송 안됨!");
        }
    }

    public IEnumerator noticeCouponCoroutine(string noticeText)
    {

        couponText.SetActive(true);
        couponText.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = noticeText;

        yield return new WaitForSeconds(2.5f);

        couponText.SetActive(false);
    }





}
