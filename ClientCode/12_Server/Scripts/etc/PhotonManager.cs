using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;
using TMPro;
public class PhotonManager : MonoBehaviourPunCallbacks
{
    public TMP_Text StatusText;
    public TMP_InputField roomInput, NickNameInput;

    public List<GameObject> Prefabs;

    void Start()
    {
        DefaultPool pool = PhotonNetwork.PrefabPool as DefaultPool;
        if (pool != null && this.Prefabs != null)
        {
            foreach (GameObject prefab in this.Prefabs)
            {
                pool.ResourceCache.Add(prefab.name, prefab);
            }
        }
    }
    void Awake()
    {
        Screen.SetResolution(1280, 720, false); 
        PhotonNetwork.AutomaticallySyncScene = true;
    }
    void Update() => StatusText.text = PhotonNetwork.NetworkClientState.ToString();

    //Callback 함수란? 어떠한 함수가 실행이 되었다면 불러지는 함수.
    // Connect -> ConnectUsingSettings() -> 콜백함수인 OnConnectedToMaster()
    public void Connect() => PhotonNetwork.ConnectUsingSettings();

    public override void OnConnectedToMaster()
    {
        Debug.Log("서버 접속완료");
        PhotonNetwork.LocalPlayer.NickName = NickNameInput.text;
    }

    public void Disconnect() => PhotonNetwork.Disconnect();
    public override void OnDisconnected(DisconnectCause cause) => Debug.Log("연결끊김");

    public void JoinLobby() => PhotonNetwork.JoinLobby();
    public override void OnJoinedLobby() => Debug.Log("로비접속완료");

    public void CreateRoom() => PhotonNetwork.CreateRoom(roomInput.text, new RoomOptions { MaxPlayers = 6 });

    public void JoinRoom() => PhotonNetwork.JoinRoom(roomInput.text);

    public void JoinOrCreateRoom() => PhotonNetwork.JoinOrCreateRoom(roomInput.text, new RoomOptions { MaxPlayers = 6}, null);

    public void JoinRandomRoom() => PhotonNetwork.JoinRandomRoom();

    public void LeaveRoom() => PhotonNetwork.LeaveRoom();

    public override void OnCreatedRoom() => Debug.Log("방만들기완료");

    public override void OnJoinedRoom()
    {
        Debug.Log("방참가완료");
        PhotonNetwork.NickName = NickNameInput.text;
        PhotonNetwork.LoadLevel("HaeSuk InGame");
    }
    public override void OnCreateRoomFailed(short returnCode, string message) => Debug.Log("방만들기실패");

    public override void OnJoinRoomFailed(short returnCode, string message) => Debug.Log("방참가실패");
    public override void OnJoinRandomFailed(short returnCode, string message) => Debug.Log("방랜덤참가실패");

    [ContextMenu("정보")]
    void Info()
    {
        if (PhotonNetwork.InRoom)
        {
            Debug.Log("현재 방 이름 : " + PhotonNetwork.CurrentRoom.Name);
            Debug.Log("현재 방 인원 : " + PhotonNetwork.CurrentRoom.PlayerCount);
            Debug.Log("현재 방 최대인원수 : " + PhotonNetwork.CurrentRoom.MaxPlayers);

            string playerStr = "방에 있는 플레이어 목록 : ";
            for(int i= 0; i< PhotonNetwork.PlayerList.Length; i++)
            {
                playerStr += PhotonNetwork.PlayerList[i].NickName + ", ";
                Debug.Log(playerStr);
            }


           
           


        }
        else
        {
            Debug.Log("접속한 인원 수 : " + PhotonNetwork.CountOfPlayers);
            Debug.Log("방 갯수 : "+ PhotonNetwork.CountOfRooms);
            Debug.Log("모든 방에 있는 인원 수 : "+ PhotonNetwork.CountOfPlayersInRooms);
            Debug.Log("로비에 있는가? : "+PhotonNetwork.InLobby);
            Debug.Log("연결되어있는가? " +PhotonNetwork.IsConnected);
        }
    }

}
