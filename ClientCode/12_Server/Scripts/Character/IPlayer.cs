using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public interface IPlayer
{
    PhotonView GetPhotonView();
    void SetMovementEnabled(bool enabled);
    GameObject GetCharacterChatPanel();

    Transform GetTransform();
    
}
