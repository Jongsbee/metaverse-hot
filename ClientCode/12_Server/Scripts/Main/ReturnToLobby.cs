using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.UI;

public class ReturnToLobby : MonoBehaviourPunCallbacks
{
    // Start is called before the first frame update
    private void Awake()
    {
        Button button = GetComponent<Button>();
        button.onClick.AddListener(LeaveRoomAndReturnToLobby);
    }

    public void LeaveRoomAndReturnToLobby()
    {
        if (PhotonNetwork.InRoom)
        {
            PhotonNetwork.LeaveRoom();
        }
    }

    public override void OnLeftRoom()
    {
        // Load the lobby scene when the player has left the room
        PhotonNetwork.LoadLevel("LobbySceneMain");
    }
}
