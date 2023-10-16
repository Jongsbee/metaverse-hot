using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UIElements;

public class ChatBalloonHandler : MonoBehaviour
{
    public Transform PlayerTransform;
    private Camera mainCamera;



    // Use this for initialization 
    void Start()
    {
        mainCamera = Camera.main;
        PlayerTransform = transform.parent;
    }

    // Update is called once per frame 
    void LateUpdate()
    {
        // 말풍선 위치 업데이트
        transform.position = PlayerTransform.position + new Vector3(0, PlayerTransform.GetChild(0).localScale.y * 19, 0);
                                                                                
        // 말풍선이 카메라를 향하도록 회전
        transform.LookAt(transform.position + mainCamera.transform.rotation * Vector3.forward, mainCamera.transform.rotation * Vector3.up);
        transform.Rotate(new Vector3(49.195f, 0, 0));
    }
}