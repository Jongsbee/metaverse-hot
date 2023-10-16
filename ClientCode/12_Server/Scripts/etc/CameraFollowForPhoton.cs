using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
public class CameraFollowForPhoton : MonoBehaviourPunCallbacks
{
    public Transform target;
    public Vector3 offset;
    //PhotonView PV;

    // offset만큼 떨어져서 카메라 추적
    void Start()
    {
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
        foreach (GameObject player in players)
        {
            if (PhotonView.Get(player).IsMine)
            {
                this.target = player.transform;
                break;
            }
        }

    }
    void Update()
    {
        transform.position = target.transform.position + offset;
    }
}
