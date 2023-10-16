using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
public class Arrow : MonoBehaviourPun
{

    public GameObject player;
    PlayerStatForPhoton playerStat;
    Weapon weapon;
    public float damage;
    public ParticleSystem hitEffect;
    public PhotonView playerPhotonView;

    void Start()
    {

        playerStat = player.GetComponent<PlayerStatForPhoton>();
        weapon = player.GetComponent<Weapon>();
        playerPhotonView = player.GetComponent<PhotonView>();

        //Debug.Log("캐릭터 스탯 : " + playerStat.CurrentDamage);
        //Debug.Log("무기 데미지 : " + weapon.damage);
        damage = playerStat.CurrentStatus(1) + weapon.damage;
        weapon.trailEffect = weapon.arrow.GetComponentInChildren<TrailRenderer>();
    }


    void Update()
    {
        transform.Translate(Vector3.forward * 2.3f);
    }



}
