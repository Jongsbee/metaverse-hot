using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveSystemForPhoton : MonoBehaviour
{
    // 현재 스테이지의 모든 웨이브 정보
    //[SerializeField] private WaveForPhoton[] waves;
    [SerializeField] private EnemyWavesData[] waves;
    public EnemyWavesData[] AllWaveInfos { get { return waves; } }
    [SerializeField] private SpecialEnemyWaveData[] specialWaves;


    [SerializeField] private EnemySpawnerForPhoton enemySpawner;
    [SerializeField] private Invest invest;
    private GameObject stageObejct;
    private GameObject topNoticeBanner, waveStartNotice, bossWaveNotice;
    private NoticeBannerSelector noticeBannerSelector;

    // 현재 웨이브 인덱스
    private int currentWaveIndex = -1;

    // 보스 웨이브 인덱스
    private int bossWaveIdx = 0;
    // 현재 웨이브 참조

    private void Awake()
    {
        stageObejct = GameObject.Find("Stages").gameObject;
        invest = stageObejct.transform.Find("Stage2/EventBuildings/StockBuilding/Event_4_Stock").GetComponent<Invest>();
        noticeBannerSelector = GameObject.Find("Manager").transform.Find("EventManager").GetComponent<NoticeBannerSelector>();
    }
    public void Start()
    {

        waves = new EnemyWavesData[]
{       
        // 소환딜레이 - 최대 몹 개수 - 몹 HP - 길 - 라이프 패널티 - 속도 - 아머
        new EnemyWavesData (1.5f, 10, 150, "Path_1_1", 1, 10f, 1f ), //1
        new EnemyWavesData (1f, 10, 160, "Path_1_2", 1, 10f, 7f ), //2
        new EnemyWavesData (1.5f, 10, 170, "Path_1_3", 1, 10f, 3f), // 3
        new EnemyWavesData (1.5f, 5, 180, "Path_1_4", 1, 20f, 1f ), // 4
        new EnemyWavesData (2f, 10, 200, "Path_1_5", 1, 10f, 3f ), // 5
        new EnemyWavesData (1f, 10, 320, "Path_2_1", 2, 15f, 1f ), // 6
        new EnemyWavesData (1f, 10, 340, "Path_2_2", 2, 20f, 5f ), // 7
        new EnemyWavesData (1.5f, 10, 360, "Path_2_3", 2, 8f, 1f), // 8
        new EnemyWavesData (1f, 10, 380, "Path_2_4", 2, 18f, 1f ), // 9
        new EnemyWavesData (2f, 15, 400, "Path_2_5", 2, 13f, 5f ), // 10
        new EnemyWavesData (1f, 15, 520, "Path_3_1", 3, 12f, 5f ), // 11
        new EnemyWavesData (1f, 15, 540, "Path_3_1", 3, 10f, 1f ), // 12
        new EnemyWavesData (1.5f, 15, 560, "Path_3_2", 3, 8f, 10f ), // 13
        new EnemyWavesData (2f, 15, 580, "Path_3_2", 3, 5f, 20f ), // 14
        new EnemyWavesData (2f, 15, 600, "Path_3_3", 3, 12f, 25f ) // 15
};

        specialWaves = new SpecialEnemyWaveData[]
        {
        // 소환딜레이 - 최대 몹 개수 - 몹 HP - 길 - 라이프 패널티 - 속도 - 아머
        new SpecialEnemyWaveData (2f, 1, 700, "Path_1_1", 100, 12f, 10f, 0, 5f,75,50 ), //1
        new SpecialEnemyWaveData  (2f, 1, 1100, "Path_2_5", 100, 15f, 15f, 1, 5f,100,50 ), //2
        new SpecialEnemyWaveData  (2f, 1, 1500, "Path_3_3", 100, 20f, 20f, 2, 5f,75,50 ), // 3
        };



    }

    public void StartWave()
    {
        // 특정 웨이브마다(보스는 5웨이브) specialWave 실행
        if ((currentWaveIndex + 2) % 5 == 0)
        {
            noticeBannerSelector.showNoticeBanner("Boss");
            enemySpawner.StartSpecialWave(specialWaves[bossWaveIdx], bossWaveIdx);
            bossWaveIdx++;
        }else
        {
            noticeBannerSelector.showNoticeBanner("WaveStart");
        }
        
        if (currentWaveIndex < waves.Length - 1)
        {
            // 인덱스의 시작이 -1이기 때문에 웨이브 인덱스 증가를 제일 먼저 함
            currentWaveIndex++;
            // EnemySpawner의 Startwave() 함수 호출, 현재 웨이브 정보 제공
            
            Debug.Log("waveIndex : " + waves[0].spawnTime + ", " + waves[0].pathString + ", " + waves[0].mobHP + ", " + waves[0].speed + ", " + waves[0].maxEnemyCount + ", " + waves[0].speed + ", " + waves[0].armor);
            enemySpawner.StartWave(waves[currentWaveIndex], currentWaveIndex);
            invest.ThisWave(currentWaveIndex);
            
        }
    }

    
    //public IEnumerator noticeWaveStarted(bool isBoss)
    //{
    //    if (isBoss)
    //    {
    //        bossWaveNotice.SetActive(true);
    //        yield return new WaitForSeconds(3f);
    //        bossWaveNotice.SetActive(false);
    //    }
    //    else
    //    {
    //        waveStartNotice.SetActive(true);
    //        yield return new WaitForSeconds(3f);
    //        waveStartNotice.SetActive(false);
    //    }
        
    //}




}






// 웨이브 커스텀 시스템
//[System.Serializable]
//public struct WaveForPhoton
//{
//    public float spawnTime;
//    public int maxEnemyCount;
//    public int mobHP;
//    public string pathString;
//    public int lifePenalty;
//    public float speed;
//    public float armor;
//}

//[System.Serializable]
//public struct SpecialWaveForPhoton
//{
//    public float spawnTime;
//    public int maxEnemyCount;
//    public int mobHP;
//    public string pathString;
//    public int lifePenalty;
//    public float speed;
//    public float armor;

//    /* 스킬
//    0 : 넉백
//    1 : 힐링
//    2 : 스턴(짜부)
//    */
//    public int skillIdx;
//    public float coolDown;
//    public int range;
//    public int power;
//}


