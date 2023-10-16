using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class EnterToCreateRoom : MonoBehaviour
{
    private Button createRoomBtn;
    private Button submitBtn;
    private TMP_InputField inputRoom;
    private TMP_InputField inputCoupon;

    private void Awake()
    {
        createRoomBtn = GameObject.Find("CreateRoomBtn").GetComponent<Button>();
        submitBtn = GameObject.Find("SubmitBtn").GetComponent<Button>();
        inputRoom = GameObject.Find("RoomInput").GetComponent<TMP_InputField>();
        inputCoupon = GameObject.Find("CouponInput").GetComponent <TMP_InputField>();
    }

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.KeypadEnter))
        {
            if (EventSystem.current.currentSelectedGameObject == inputRoom.gameObject)
            {
                createRoomBtn.onClick.Invoke();
            }
            else if (EventSystem.current.currentSelectedGameObject == inputCoupon.gameObject)
            {
                submitBtn.onClick.Invoke();
            }
        }
    }
}
