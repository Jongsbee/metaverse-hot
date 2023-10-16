using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;
using TMPro;
using Firebase.Auth;
using UnityEngine.SceneManagement;

public class PhotonManagerForNewLobby : MonoBehaviourPunCallbacks
{
    [Header("LobbyPanel")]
    public GameObject LobbyPanel;
    public TMP_InputField RoomInput;
    public TMP_Text WelcomeText;
    public TMP_Text LobbyInfoText;
    public Button[] CellBtn;
    public Button PreviousBtn;
    public Button NextBtn;
    public Button CreateRoomButton;

    [Header("RoomCreatePopUp")]
    public GameObject RoomCreatePanel;
    public GameObject PasswordConfirmPanel;
    public GameObject PasswordFailPanel;
    public TMP_InputField RoomNameInput;
    public TMP_InputField RoomPasswordInput;
    public TMP_InputField RoomPasswordConfirmInput;
    public Button CreateRoomSumbitButton;
    public Button PasswordSumbitButton;




    [Header("ETC")]
    public PhotonView PV;
    private MyAPIClient myAPIClient;
    List<RoomInfo> myList = new List<RoomInfo>();
    int currentPage = 1, maxPage, multiple;
    string commonUrl;
    public List<GameObject> Prefabs;

    public RawImage[] lobbyImages;
    public RawImage[] loadingImages;
    public RawImage backGroundImage;

    private int rnd;

    void Awake()
    {

        if (FirebaseAuth.DefaultInstance.CurrentUser != null)
        {
            Debug.Log(FirebaseAuth.DefaultInstance.CurrentUser.DisplayName);
        }
        else
        {
            Debug.Log("user is null");
        }
        Screen.SetResolution(1920, 1080, false);
        PhotonNetwork.AutomaticallySyncScene = true;
        Connect();
    }

    private void Start()
    {
        // API 로 GET 요청 가져오기 Test
        myAPIClient = this.gameObject.GetComponent<MyAPIClient>();
        Debug.Log(FirebaseAuth.DefaultInstance.CurrentUser.DisplayName);
        commonUrl = MyAPIClient.BACKEND_URL + $"api/member/info/{FirebaseAuth.DefaultInstance.CurrentUser.DisplayName}";
        StartCoroutine(myAPIClient.getRequest(commonUrl, getInfoSuccess, getInfoFailed));


        DefaultPool pool = PhotonNetwork.PrefabPool as DefaultPool;
        if (pool != null && this.Prefabs != null)
        {
            foreach (GameObject prefab in this.Prefabs)
            {
                if (pool.ResourceCache.ContainsKey(prefab.name))
                {
                    // 동일한 이름의 프리팹이 이미 있을 경우 덮어쓰기하려면 주석 해제
                    // pool.ResourceCache[prefab.name] = prefab;
                }
                else
                {
                    pool.ResourceCache.Add(prefab.name, prefab);
                }
            }
        }
    }

    public void getInfoSuccess(MyAPIClient.ResponseDTO data)
    {
        if (data.httpStatus.Equals("OK"))
        {
            Debug.Log("info check success: ");
        }
        else
        {
            Debug.Log("info check failed: ");
        }
    }
    public void getInfoFailed(MyAPIClient.ResponseDTO data)
    {
        if (data.httpStatus.Equals("OK"))
        {
            Debug.Log("info check success: ");
        }
        else
        {
            Debug.Log("info check failed: ");
        }
    }


    #region 방리스트 갱신
    // ◀버튼 -2 , ▶버튼 -1 , 셀 숫자
    public void MyListClick(int num)
    {
        if (num == -2) --currentPage;
        else if (num == -1) ++currentPage;
        else
        {
           
            

            if (myList[multiple+num].CustomProperties.ContainsKey("password"))
            {
                Debug.Log("방이 비밀번호가 걸려있습니다.");
                PasswordConfirmPanel.SetActive(true);
            }
            else
            {
                rnd = Random.Range(0, 6);
                backGroundImage.gameObject.SetActive(false);
                LobbyPanel.SetActive(false);
                loadingImages[rnd].gameObject.SetActive(true);
                PhotonNetwork.JoinRoom(myList[multiple + num].Name);
            }
            
        }
        MyListRenewal();
    }

    void MyListRenewal()
    {


        maxPage = (myList.Count % CellBtn.Length == 0) ? myList.Count / CellBtn.Length : myList.Count / CellBtn.Length + 1;

        // 최대페이지


        // 이전, 다음버튼
        PreviousBtn.interactable = (currentPage <= 1) ? false : true;
        NextBtn.interactable = (currentPage >= maxPage) ? false : true;

        // 페이지에 맞는 리스트 대입
        multiple = (currentPage - 1) * CellBtn.Length;
        for (int i = 0; i < CellBtn.Length; i++)
        {
           
            CellBtn[i].interactable = (multiple + i < myList.Count) ? true : false;
            CellBtn[i].transform.GetChild(0).GetComponent<TMP_Text>().text = (multiple + i < myList.Count) ? myList[multiple + i].Name : "";
            CellBtn[i].transform.GetChild(1).GetComponent<TMP_Text>().text = (multiple + i < myList.Count) ? myList[multiple + i].PlayerCount + "/" + myList[multiple + i].MaxPlayers : "";
            if (multiple + i < myList.Count && myList[multiple + i].CustomProperties.ContainsKey("password"))
            {
                CellBtn[i].transform.GetChild(2).GetComponent<RawImage>().gameObject.SetActive(true);
            }
            else
            {
                CellBtn[i].transform.GetChild(2).GetComponent<RawImage>().gameObject.SetActive(false);
            }
        }
    }

    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        int roomCount = roomList.Count;
        for (int i = 0; i < roomCount; i++)
        {
            if (!roomList[i].RemovedFromList)
            {
                if (!myList.Contains(roomList[i])) myList.Add(roomList[i]);
                else myList[myList.IndexOf(roomList[i])] = roomList[i];
            }
            else if (myList.IndexOf(roomList[i]) != -1) myList.RemoveAt(myList.IndexOf(roomList[i]));
        }
        MyListRenewal();
    }
    #endregion


    #region 서버연결


    public string responseNickname()
    {
        return FirebaseAuth.DefaultInstance.CurrentUser.DisplayName;
    }
    void Update()
    {
        //StatusText.text = PhotonNetwork.NetworkClientState.ToString();
        LobbyInfoText.text = (PhotonNetwork.CountOfPlayers - PhotonNetwork.CountOfPlayersInRooms) + "로비 / " + PhotonNetwork.CountOfPlayers + "접속";
    }

    public void Connect()
    {
        rnd = Random.Range(0, 3);
        Debug.Log(rnd);
        lobbyImages[rnd].gameObject.SetActive(true);
        PhotonNetwork.ConnectUsingSettings();
    }

    public override void OnConnectedToMaster() => PhotonNetwork.JoinLobby();

    public override void OnJoinedLobby()
    {

        lobbyImages[rnd].gameObject.SetActive(false);
        backGroundImage.gameObject.SetActive(true);
        LobbyPanel.SetActive(true);
        PhotonNetwork.LocalPlayer.NickName = FirebaseAuth.DefaultInstance.CurrentUser.DisplayName;
        WelcomeText.text = PhotonNetwork.LocalPlayer.NickName + "님 환영합니다";
        myList.Clear();
    }

    public void Disconnect() => PhotonNetwork.Disconnect();

    public override void OnDisconnected(DisconnectCause cause)
    {
        
        LobbyPanel.SetActive(false);
        SceneManager.LoadScene("HaeSuk Login");
    }
    #endregion

    public void ExitCreateRoomPopUp()
    {
        RoomCreatePanel.SetActive(false);
        CreateRoomButton.gameObject.SetActive(true);
    }

    public void CreateRoomPopUp()
    {
        RoomCreatePanel.SetActive(true);
        CreateRoomButton.gameObject.SetActive(false);

    }

    public void ExitPasswordFailPanel()
    {
        PasswordFailPanel.SetActive(false);
    }

    public void ExitPasswordPanel()
    {
        PasswordConfirmPanel.SetActive(false);
    }

    public void ConfirmRoomPassword()
    {
        if (myList[multiple].CustomProperties.ContainsKey("password"))
        {
            string roomPassword = (string)myList[multiple].CustomProperties["password"];

            if (RoomPasswordConfirmInput.text == roomPassword)
            {
                rnd = Random.Range(0, 6);
                backGroundImage.gameObject.SetActive(false);
                LobbyPanel.SetActive(false);
                loadingImages[rnd].gameObject.SetActive(true);
                PhotonNetwork.JoinRoom(myList[multiple].Name);
            }
            else
            {
                Debug.Log("비밀번호가 일치하지 않습니다.");
                PasswordFailPanel.SetActive(true);
            }
        }
    }


    public void CreateRoom()
    {
        if (PhotonNetwork.IsConnected)
        {
            // Set the custom room properties with the password if it exists
            var roomOptions = new RoomOptions();
            //public TMP_InputField RoomNameInput;
            // public TMP_InputField RoomPasswordInput;
            if (!string.IsNullOrEmpty(RoomPasswordInput.text))
            {
                roomOptions.CustomRoomProperties = new ExitGames.Client.Photon.Hashtable { { "password", RoomPasswordInput.text } };
                roomOptions.CustomRoomPropertiesForLobby = new string[] { "password" };
            }
            roomOptions.MaxPlayers = 4;

            
            // Create the room
            PhotonNetwork.CreateRoom(RoomNameInput.text == "" ? "Room" + Random.Range(0, 100) : RoomNameInput.text, roomOptions);
            rnd = Random.Range(0, 6);
            backGroundImage.gameObject.SetActive(false);
            LobbyPanel.SetActive(false);
            loadingImages[rnd].gameObject.SetActive(true);
        }
        //CreateRoom changes to custom property settings.


      

        //PhotonNetwork.CreateRoom(RoomInput.text == "" ? "Room" + Random.Range(0, 100) : RoomInput.text, new RoomOptions { MaxPlayers = 4 });
    }
    public void JoinRandomRoom() => PhotonNetwork.JoinRandomRoom();

    public void LeaveRoom() => PhotonNetwork.LeaveRoom();

    public override void OnJoinedRoom()
    {
        //PhotonNetwork.LoadLevel("HaeSukMain");
        
        PhotonNetwork.LoadLevel("종습Main");
        // RoomPanel.SetActive(true);
        // RoomRenewal();
        //ChatInput.text = "";
        // for (int i = 0; i < ChatText.Length; i++) ChatText[i].text = "";
    }

    public override void OnCreateRoomFailed(short returnCode, string message) { RoomInput.text = ""; CreateRoom(); }

    public override void OnJoinRandomFailed(short returnCode, string message) { RoomInput.text = ""; CreateRoom(); }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
       // RoomRenewal();
       // ChatRPC("<color=yellow>" + newPlayer.NickName + "님이 참가하셨습니다</color>");
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        //RoomRenewal();
        //ChatRPC("<color=yellow>" + otherPlayer.NickName + "님이 퇴장하셨습니다</color>");
    }

   
}
