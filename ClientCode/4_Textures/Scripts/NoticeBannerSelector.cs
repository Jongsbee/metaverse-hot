using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class NoticeBannerSelector : MonoBehaviour
{
    private GameObject waveStartNotice, bossNotice;
    private float closeDelay;



    private void Awake()
    {
        closeDelay = 3.0f;
        waveStartNotice = GameObject.Find("UI").transform.Find("TopCenter_NoticeBanner/WaveStartNotice").gameObject;
        bossNotice = GameObject.Find("UI").transform.Find("TopCenter_NoticeBanner/BossNotice").gameObject;
        Debug.Log(bossNotice);

    }
    void Start()
    {
        waveStartNotice.SetActive(false);
        bossNotice.SetActive(false);
    }
    public void noticeSwitch(string info, bool stateInfo)
    {
        switch (info)
        {
            case "WaveStart":
                waveStartNotice.SetActive(stateInfo);
                break;
            case "Boss":
                bossNotice.SetActive(stateInfo);
                break;
        }
    }
    IEnumerator WaitClose(string info)
    {
        yield return new WaitForSeconds(closeDelay);
        noticeSwitch(info, false);
    }
    // 외부에서 이 함수를 호출하면 배너가 표시되었다가 사라집니다.
    public void showNoticeBanner(string info)
    {
        noticeSwitch(info, true);
        StartCoroutine(WaitClose(info));
    }
}
