using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using TMPro;

public class PhotonManagerForProtoType : MonoBehaviourPunCallbacks
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
        Screen.SetResolution(1920, 1080, false);
        PhotonNetwork.AutomaticallySyncScene = true;
        Connect();
    }

    void Update() => StatusText.text = PhotonNetwork.NetworkClientState.ToString();

    public void Connect() => PhotonNetwork.ConnectUsingSettings();

    public override void OnConnectedToMaster()
    {
        Debug.Log("서버 접속완료");
        JoinLobby();
    }

    public void Disconnect() => PhotonNetwork.Disconnect();
    public override void OnDisconnected(DisconnectCause cause) => Debug.Log("연결끊김");

    public void JoinLobby() => PhotonNetwork.JoinLobby();
    public override void OnJoinedLobby()
    {
        Debug.Log("로비접속완료");
       // CreateRoom();

    }
    public void CreateRoom() => PhotonNetwork.CreateRoom(roomInput.text, new RoomOptions { MaxPlayers = 6 });

    public void JoinRoom()
    {
        PhotonNetwork.JoinRoom(roomInput.text);
    }
    public void JoinOrCreateRoom() => PhotonNetwork.JoinOrCreateRoom(roomInput.text, new RoomOptions { MaxPlayers = 6 }, null);

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

   
}
