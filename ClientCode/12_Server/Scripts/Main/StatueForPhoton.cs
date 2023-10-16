using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Random = System.Random;
public class StatueForPhoton : MonoBehaviourPun
{
    GameObject[] Sounds;
    Random randomObj = new Random();
    EnemyForPhoton[] TargetEnemies;
    [SerializeField] ParticleSystem punish;
    [SerializeField] ParticleSystem punishEffect;
    [SerializeField] ParticleSystem deBuffEffect;
    int punishable;

    int randomValue;
    [SerializeField] float startPercent = 50;
    float currentPercent;

    public GameObject StatueParticle;

    BGM bgm;
    GameObject BGMObject;
    GMForPhoton gm;
   
    bool isNear = false;

    // Start is called before the first frame update
    void Start()
    {
        gm = FindObjectOfType<GMForPhoton>();
        punishable = gm.Punishable;
        StatueParticle = transform.GetChild(1).gameObject;
        StatueParticle.SetActive(punishable > 0);
        BGMObject = GameObject.Find("BGM");
        bgm = FindObjectOfType<BGM>();
        
       
        currentPercent = startPercent;
    }
    [PunRPC]
    public void Pray(int randomValue)
    {

      
       
        Debug.Log($"장승 확률은? {randomValue}%");

        GameObject.FindGameObjectsWithTag("Enemy");

        BGMObject.GetComponent<AudioSource>().mute = true;
        Debug.Log($"오디오 소스가 뮤트인지? : {BGMObject.GetComponent<AudioSource>().mute}");
        bool sucees = randomValue < currentPercent;


        // 이부분 Instantiate 어떻게 할건지
        if(gm.EnemyNavi.transform.position == null)
        {
            Debug.Log("맵에 몹이 없음!!!");
        }else
        {
            ParticleSystem nuke = Instantiate(punish, gm.EnemyNavi.transform.position, Quaternion.identity);
            if (!sucees)
            {
                foreach (ParticleSystem nukeChild in nuke.GetComponentsInChildren<ParticleSystem>())
                {
                    var em = nukeChild.emission;
                    em.enabled = false;
                }
            }
            // all enemies Die!
            StartCoroutine(doPunish(sucees));
            StartCoroutine(playAgain(BGMObject.GetComponent<AudioSource>()));
        }
    }

    IEnumerator doPunish(bool sucees)
    {
        yield return new WaitForSeconds(7f);
        //GameObject TargetSource = GameObject.Find("startPointForPhoton");
        //TargetEnemies = TargetSource.GetComponentsInChildren<EnemyForPhoton>();
        TargetEnemies = gm.EnemyList.ToArray();
        
        //for (int i = 0; i<TargetEnemies.Count; i++)
        //{
        //    EnemyForPhoton enemy = TargetEnemies[i];
        //    if (sucees)
        //    {
        //        enemy.ProcessHit(Mathf.Infinity, punishEffect);
        //        currentPercent = currentPercent / 2;

        //        // 적들을 모두 죽임
        //    }
        //    else
        //    {
        //        doom(enemy); // 디버프
        //    }
        //}
        foreach (EnemyForPhoton enemy in TargetEnemies)
        {
            if (sucees)
            {
                enemy.ProcessHit(Mathf.Infinity, punishEffect);
                enemy.photonView.RPC("ProcessHitWithNoParticle",RpcTarget.Others, Mathf.Infinity);
                currentPercent = currentPercent / 2;

                // 적들을 모두 죽임
            }
            else
            {
                doom(enemy); // 디버프
            }
        }
        yield break;
    }

    void doom(EnemyForPhoton thisEnemy)
    {
        StartCoroutine((doomEnemy(thisEnemy)));
    }

    IEnumerator doomEnemy(EnemyForPhoton enemy)
    {
        int cnt = 0;
        while (cnt < 4 && PhotonNetwork.IsMasterClient)
        {
            Instantiate(deBuffEffect, enemy.transform.position, Quaternion.identity);
            enemy.ProcessReduceArmor(-3f, deBuffEffect);
            enemy.ProcessReduceSpeed(-3f, deBuffEffect);
            enemy.photonView.RPC("ProcessReduceSpeedWithNoParticle",RpcTarget.Others, -3f);
            enemy.photonView.RPC("ProcessReduceArmorWithNoParticle", RpcTarget.Others, -3f);
            cnt++;
            yield return new WaitForSeconds(4f);
        }

    }

    IEnumerator playAgain(AudioSource play)
    {
        yield return new WaitForSeconds(14f);

        play.mute = false;

        yield break;
    }
}
