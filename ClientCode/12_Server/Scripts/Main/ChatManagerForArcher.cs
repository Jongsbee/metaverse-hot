using UnityEngine;
using Photon.Pun;
using TMPro;



public class ChatManagerForArcher : MonoBehaviourPunCallbacks
{
    [Header("ChatPanel")]
    private GameObject chatPanel;
    private TMP_Text[] chatText;
    private TMP_InputField chatInput;

    [Header("ETC")]
    ArcherPlayerMoverForPhoton playerMover;

    private bool chatActive = false;



    private void Awake()
    {

        if (photonView.IsMine)
        {

            chatPanel = GameObject.Find("ChatPanel");
            chatInput = GameObject.Find("ChatPanel").GetComponentInChildren<TMP_InputField>();
            Transform content = GameObject.Find("Content").transform;
            chatText = content.GetComponentsInChildren<TMP_Text>();
            playerMover = GetComponent<ArcherPlayerMoverForPhoton>();
            chatPanel.SetActive(false);
            chatActive = false; // chatActive 변수 초기화
        }

    }

    void Update()
    {
        if (photonView.IsMine)
        {
            // Check if the Enter key is pressed
            if (Input.GetKeyDown(KeyCode.Return))
            {
                // Check if the chat UI is active and the Enter key is pressed
                if (chatActive && !string.IsNullOrEmpty(chatInput.text) && Input.GetKeyDown(KeyCode.Return))
                {
                    Debug.Log("채팅 인풋이 비어있지않으므로 전송합니다.");
                    Send();
                }
                else
                {
                    Debug.Log("채팅 인풋이 비어있으므로 채팅창을 종료합니다.");
                    chatActive = !chatActive;
                    chatPanel.SetActive(chatActive);
                }


                // If the chat UI is now active, disable character movement
                if (chatActive)
                {
                    playerMover.enabled = false;
                    // Set the input focus to the chat input field
                    chatInput.ActivateInputField();
                }
                else
                {
                    playerMover.enabled = true;
                }
            }

        }
      

    }


    #region 채팅
    public void Send()
    {
        if (string.IsNullOrEmpty(chatInput.text)) return; // 빈 문자열인 경우 메시지 전송하지 않음
        photonView.RPC("ChatRPC", RpcTarget.All, PhotonNetwork.NickName + " : " + chatInput.text);
        chatInput.text = "";
    }

    [PunRPC] // RPC는 플레이어가 속해있는 방 모든 인원에게 전달한다
    void ChatRPC(string msg)
    {
        if (chatText == null)
        {
            return;
        }
        bool isInput = false;
        for (int i = 0; i < chatText.Length; i++)
        {

            if (chatText[i] == null)
            {
                continue;
            }

            if (chatText[i].text == "")
            {
                isInput = true;
                chatText[i].text = msg;
                break;
            }
        }

        if (!isInput) // 꽉차면 한칸씩 위로 올림
        {
            for (int i = 1; i < chatText.Length; i++)
            {
                if (chatText[i - 1] == null)
                {
                    continue;
                }
                chatText[i - 1].text = chatText[i].text;
                chatText[chatText.Length - 1].text = msg;

            }
        }
    }
    #endregion
}
