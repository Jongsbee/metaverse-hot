using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Photon.Pun;

public class NickManager : MonoBehaviour
{
    private TMP_Text[] nickText;
    private TMP_InputField nickInput;

    PhotonView photonView;

    private IPlayer playerMover;



    public void RegisterPlayer(IPlayer player)
    {
        if (player.GetPhotonView().IsMine)
        {
            playerMover = player;
        }
    }

    private void Awake()
    {
        photonView = GetComponent<PhotonView>();

    }

    private void Start()
    {
        int myViewID = playerMover.GetPhotonView().ViewID;    
    }
    // Update is called once per frame
    void Update()
    {
        // 없어도 될듯
        if (playerMover == null || !playerMover.GetPhotonView().IsMine)
        {
            return;
        }

    }


    [PunRPC] 

    public void DisplayChatMessage(string message, int viewID)
    {
        GameObject playerObject = PhotonView.Find(viewID).gameObject;

    }
}
