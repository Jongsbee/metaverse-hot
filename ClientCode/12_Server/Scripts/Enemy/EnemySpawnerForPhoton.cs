using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class EnemySpawnerForPhoton : MonoBehaviourPun
{
    // 적 프리펩
    [SerializeField] private GameObject[] enemyPrefabs;
    // 스페셜 몹 프리펩
    [SerializeField] private GameObject[] specialPrefabs;
    // 임의 시작용 wavesystem
    [SerializeField] private GameObject startt;

    // 현재 웨이브 정보
    private EnemyWavesData currentWave;
    public EnemyWavesData CurrentWave { get { return currentWave; } }

    // 현재 맵에 존재하는 모든 적의 정보
    public List<EnemyForPhoton> enemyList;

    // 적의 생성과 삭제는 EnemySpawner에서 하기 때문에 Set은 필요 없다.
    public List<EnemyForPhoton> EnemyList => enemyList;

    GMForPhoton gm;
    private int spawnEnemyCount;

    // 플레이어 수
    private float playerCnt = 1;

    // 적 생성 시 파티클
    [SerializeField] ParticleSystem SpawnEffect;

    private SpecialEnemyWaveData currentSpecialWave;
    GameObject clone;
    private void Awake()
    {
        // 적 리스트 메모리 할당
        enemyList = new List<EnemyForPhoton>();
        gm = FindObjectOfType<GMForPhoton>();
    }

    public void StartWave(EnemyWavesData wave, int idx)
    {
        
        
        if (idx  == 0)
        {
            playerCnt = 1 + 0.9f * (gm.currentPlayer - 1);
            Debug.Log("스타트 웨이브 플레이어 카운트는?? " + playerCnt);
        }
        // 매개변수로 받아온 웨이브 정보 저장
        currentWave = wave;
        Debug.Log("StartWave() Called + " + idx);
        Debug.Log($"현재 웨이브 : {currentWave.pathString}");

        // 현재 웨이브 시작
        StartCoroutine("SpawnEnemy", idx);
    }

    // 1 - 종섭추가
    public void StartSpecialWave(SpecialEnemyWaveData SpecialWave, int idx)
    {
        currentSpecialWave = SpecialWave;

        if (PhotonNetwork.IsMasterClient)
        {
            clone = PhotonNetwork.Instantiate(specialPrefabs[idx].name, transform.position, Quaternion.identity);
            photonView.RPC("ReceiveBossInfo", RpcTarget.AllBuffered, clone.GetComponent<PhotonView>().ViewID);
            //gm.PV.RPC("MobCounter", RpcTarget.AllBuffered, true, true);
        }
        
        //gm.MobCounter(true);
    }


    // 2 - 종섭추가
    private IEnumerator SpawnEnemy(int idx)
    {

        int spawnEnemyCount = 0;

        while (spawnEnemyCount < currentWave.maxEnemyCount)
        {
            // 몹 바꾸려면 여기서 바꿔주면 됨
            
            if (PhotonNetwork.IsMasterClient)
            {
                clone = PhotonNetwork.Instantiate(enemyPrefabs[idx].name, transform.position, Quaternion.identity);
                photonView.RPC("ReceiveEnemyInfo", RpcTarget.AllBuffered, clone.GetComponent<PhotonView>().ViewID);
                PhotonNetwork.Instantiate(SpawnEffect.name, clone.transform.position, Quaternion.identity);
                //gm.PV.RPC("MobCounter", RpcTarget.AllBuffered, true,true);
            }

            spawnEnemyCount++;
            //Instantiate(SpawnEffect, clone.transform.position, Quaternion.identity);
            //Debug.Log("몹이 현재 소환중입니다. 현재 소환된 몹의 마릿수는 .. " + spawnEnemyCount);
           
            if (spawnEnemyCount == currentWave.maxEnemyCount)
            {
                gm.isEndWave = true;
            }
            yield return new WaitForSeconds(currentWave.spawnTime);
        }



    }

    //public void StartSpecialWave(SpecialWaveForPhoton SpecialWave, int idx)
    //{
       
//    gm.MobCounter(true);


//}


    [PunRPC]
    private void ReceiveEnemyInfo(int enemyViewID)
    {
      
        PhotonView enemyPV = PhotonView.Find(enemyViewID);
        if (enemyPV != null)
        {
            GameObject clone = enemyPV.gameObject;

            EnemyForPhoton enemy = clone.GetComponent<EnemyForPhoton>();
            enemy.pathString = currentWave.pathString;
            enemy.mobHP = currentWave.mobHP*playerCnt;
            //enemy.isBoss = currentWave.isBoss;
            //if (enemy.isBoss) { enemy.transform.GetChild(0).transform.localScale *= 2f; }
            enemy.lifePenalty = currentWave.lifePenalty;
            enemy.speed = currentWave.speed;
            enemy.armor = currentWave.armor;
            enemyList.Add(enemy);
            //Debug.Log("ReceiveEnemyInfo + " + enemyList.Count);
        }
        else
        {
            Debug.Log("에러!! enemyPV가 null 값입니다.");
        }
        
        
    }

    [PunRPC]
    private void ReceiveBossInfo(int enemyViewID)
    {

        PhotonView enemyPV = PhotonView.Find(enemyViewID);

        if (enemyPV != null)
        {
            GameObject clone = enemyPV.gameObject;

            EnemyForPhoton enemy = clone.GetComponent<EnemyForPhoton>();
            enemy.transform.GetChild(0).transform.localScale *= 2f;
            enemy.pathString = currentSpecialWave.pathString;
            enemy.mobHP = currentSpecialWave.mobHP * playerCnt;
            enemy.lifePenalty = currentSpecialWave.lifePenalty;
            enemy.speed = currentSpecialWave.speed;
            enemy.armor = currentSpecialWave.armor;
            enemy.isBoss = true;
            enemy.skillIdx = currentSpecialWave.skillIdx;
            enemy.coolDown = currentSpecialWave.coolDown;
            enemy.range = currentSpecialWave.range;
            enemy.power = currentSpecialWave.power;
            enemyList.Add(enemy);
        }
        else
        {
            Debug.Log("에러!! enemyPV가 null 값입니다.");
        }


    }





    public void DestroyEnemy(EnemyForPhoton enemy, bool isKill = true)
    {
        enemyList.Remove(enemy);
        if (isKill)
        {
            // 리스트에서 사망하는 적 정보 삭제
            Destroy(enemy.gameObject, 0.8f);
          
        }
        if (gm.isEndWave && enemyList.Count == 0)
        {
            gm.isEndWave = false;
            Debug.Log("웨이브 클리어!");
            gm.ClearWave();

        }

    }
    
}
