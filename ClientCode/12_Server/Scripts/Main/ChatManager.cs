using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;
using TMPro;
using Firebase;
using Firebase.Auth;


public class ChatManager : MonoBehaviourPunCallbacks
{
    [Header("ChatPanel")]
    private GameObject chatPanel;
    private TMP_Text[] chatText;
    private TMP_InputField chatInput;
    private TMP_Text InGameChatInput;
    GameObject InGameChatPanel;


    [Header("SettingsPanel")]
    private GameObject settingsPanel;
    private Button settingsButton;

    [Header("ETC")]

    PhotonView photonView;
   // private TMP_Text nickText;
    private bool chatActive = false;
    private bool settingsPanelActive = false;

    private IPlayer playerMover;
    private Dictionary<int, Coroutine> displayCoroutines;

    public void RegisterPlayer(IPlayer player)
    {
        if (player.GetPhotonView().IsMine)
        {
            playerMover = player;
            InGameChatPanel = playerMover.GetCharacterChatPanel();
            InGameChatInput = InGameChatPanel.transform.GetChild(0).GetComponent<TMP_Text>();
            InGameChatPanel.SetActive(false);
           // nickText = playerMover.GetTransform().Find("NickNameCanvas/NickBox/Text_Nick").GetComponent<TMP_Text>();
        }
    }

    private void Awake()
    {
            
            photonView = GetComponent<PhotonView>();
            chatPanel = GameObject.Find("ChatPanel");
            chatInput = GameObject.Find("ChatPanel").GetComponentInChildren<TMP_InputField>();
            settingsPanel = GameObject.Find("Settings");
            settingsButton = settingsPanel.GetComponentInChildren<Button>();
            settingsButton.onClick.AddListener(CloseSettingsPanel);
            
            
            Transform content = GameObject.Find("Content").transform;
            chatText = content.GetComponentsInChildren<TMP_Text>();
            chatPanel.SetActive(false);
            settingsPanel.SetActive(false);
            displayCoroutines = new Dictionary<int, Coroutine>();

    }

    private void Start()
    {
        
    }

    public void CloseSettingsPanel()
    {
        settingsPanel.SetActive(false);
    }

    void Update()
    {
        if (playerMover == null || !playerMover.GetPhotonView().IsMine)
        {
            return;
        }

       // nickText.text = PhotonNetwork.NickName;

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            settingsPanelActive = !settingsPanelActive;
            settingsPanel.SetActive(settingsPanelActive);
        }


        if (Input.GetKeyDown(KeyCode.Return))
        {
            
            if (chatActive && !string.IsNullOrEmpty(chatInput.text) && Input.GetKeyDown(KeyCode.Return))
            {
                //Debug.Log("채팅 인풋이 비어있지않으므로 전송합니다.");
                Send();
                chatActive = !chatActive;
                chatPanel.SetActive(chatActive);
            }
            else
            {
                //Debug.Log("채팅 인풋이 비어있으므로 채팅창을 종료합니다.");
                chatActive = !chatActive;
                chatPanel.SetActive(chatActive);
            }

          
            if (chatActive)
            {
                playerMover.SetMovementEnabled(false);
                
                chatInput.ActivateInputField();
            }
            else
            {
                playerMover.SetMovementEnabled(true);
            }
        }








    }
    [PunRPC]
    public void DisplayChatMessage(string message, int viewID)
    {
        if (displayCoroutines.ContainsKey(viewID) && displayCoroutines[viewID] != null)
        {
            StopCoroutine(displayCoroutines[viewID]);
        }
        displayCoroutines[viewID] = StartCoroutine(DisplayChatMessageCoroutine(message, viewID));
    }

    IEnumerator DisplayChatMessageCoroutine(string message, int viewID)
    {
        
        GameObject playerObject = PhotonView.Find(viewID).gameObject;
        GameObject playerInGameChatPanel = playerObject.GetComponent<IPlayer>().GetCharacterChatPanel();
        TMP_Text playerInGameChatInput = playerInGameChatPanel.transform.GetChild(0).GetComponent<TMP_Text>();

        
        playerInGameChatInput.text = message;
        playerInGameChatPanel.SetActive(true);

       
        yield return new WaitForSeconds(5f);

       
        playerInGameChatPanel.SetActive(false);

       
        displayCoroutines.Remove(viewID);
    }







    #region 채팅
    public void Send()
    {
        if (string.IsNullOrEmpty(chatInput.text))
        {
            Debug.Log("비어있는데요???");
            return;
        } // 빈 문자열인 경우 메시지 전송하지 않음
        int myViewID = playerMover.GetPhotonView().ViewID;
        photonView.RPC("DisplayChatMessage", RpcTarget.All, PhotonNetwork.NickName + " : " + chatInput.text, myViewID); //DisplayChatMessage(chatInput.text);
        photonView.RPC("ChatRPC", RpcTarget.All, PhotonNetwork.NickName + " : " + chatInput.text);
        chatInput.text = "";
    }

    [PunRPC] // RPC는 플레이어가 속해있는 방 모든 인원에게 전달한다
    void ChatRPC(string msg)
    {
        int emptyIndex = -1;
        for (int i = 0; i < chatText.Length; i++)
        {
            if (chatText[i] == null)
            {
                continue;
            }

            if (chatText[i].text == "" && emptyIndex == -1)
            {
                emptyIndex = i;
            }
        }
        if (emptyIndex != -1) // 비어있는 인덱스를 찾았다면 그 위치에 메시지를 삽입합니다.
        {
            chatText[emptyIndex].text = msg;
        }
        else // 꽉차면 한칸씩 위로 올립니다.
        {
            for (int i = 1; i < chatText.Length; i++)
            {
                if (chatText[i - 1] == null)
                {
                    continue;
                }
                chatText[i - 1].text = chatText[i].text;
            }
            chatText[chatText.Length - 1].text = msg;
        }
    }
    #endregion
}
