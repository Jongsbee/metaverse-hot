using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GM : MonoBehaviour
{
    /*
    1. 씬 선택 기능

    2. 팝업 씬 선택 기능? (오브젝트에 OnClick으로 다는 게 나을지는 미정)

    3. 게임 알림 메시지 표시 기능
        - 곧 게임이 시작됩니다.
        - 다음 스테이지로 이동해 주세요.
        - 광산으로 가서 퀘스트를 진행해주세요.
        - ??님이 아이템을 사용했습니다!

    4. HUD에 표시할 변수
        - 스테이지, 웨이브 정보
        - 남은 시간
        - 남은 적 마릿수, 플레이어가 잡은 적 마릿수
        - 플레이어 소지 골드
        - 기지의 남은 체력
        - 플레이어의 포탑 설치 여부?

    5. 최종 결과 표시
        - 서버와 통신해서 방 안에 있는 플레이어들의 정보를 받아오기

    6. 추가로 저장해야할 정보들
        - killCount(Tower kill, Player kill)
        - 업적 저장할 공간
        - 그 외 인게임 결과값들..(생각해보기)
    */

    /*  
    * [1] 변수 설정
    *
    */

    // 기타 변수 및 개발중
    public bool isEndWave = false;  

    private string nowScene = "InGame";  // [1-1] 씬정보

    private bool isPopup = false; // [1-2] 팝업 여부(1개라도 띄워져 있으면 게임 화면은 모든 마우스, 키보드 입력 비활성화)

    private List<Enemy> enemyList; // [1-3] 기지에서 가장 가까운 적의 위치(젤앞적transform.position)
    private Enemy enemyNavi;
    public Enemy EnemyNavi { get { return enemyNavi; } }
   
    private bool isGameStart = false;   // [1-4] 방장의 게임 시작 여부
    
    private Transform myTowerNavi;  // [1-5] 내가 설치한 포탑의 위치

    // [1-6] 상대의 위치, 나와 다른 사람이 설치한 포탑의 위치?, 능력치, 골드 등(추가)

    // [1-7] 알림창 코멘트(이거 억덕하지 좀 빡세네)



    /*  
     * [2] 디폴트 설정 
     *
     */

    // [2-1] 계정 정보
    public int userExp = 1;
    public int userLv = 0;
    
    private string playerName = "TEST";
    private int characterIdx = 0;


    //[2-2] 게임 필수정보 (왼쪽상단 UI)  default gold is 0
    private int gold = 5000;
    private int stocks = 0;
    private int towerCnt = 10;
    private int killCnt = 0;

    private int hasTower = 0;
    private int punishable = 3; // 장승 사용 가능 횟수
    public int Punishable { 
        get { return punishable; } 
        set { punishable = value; } 
    }
    public int StockProperty {      // 주식
        get { return stocks; }
        set { this.stocks = value; }
    } 
    public int Stage { get { return stage; } }  // 스테이지
    private int life = 100;
    private int wave = 1;
    private int mobCnt = 0;

    private string characterName;
    public string CharName
    {
        get { return characterName; }
        set
        { characterName = value; }
    }

    // [2-3] Getter Setter

    public int Gold { get { return gold; } } // 골드
    public int TowerCnt { get { return towerCnt; } } // 타워
    private Wave[] allWaveInfos;

    // 캐릭터 status(일단 보류)(강화정보)


    // 게임 정보


    /* 아이템정보 - 다른곳으로 옮김
     *  private List<GameObject> myItems;
     *  public List<GameObject> MyItems { get { return myItems; } }
     */


    //private int waveMobTotal;

    // [2-4] 웨이브 정보

    private int stage = 1;
    private int timer = 10;
    private int finalWave = 15;

    private GameObject startPoint;
    GameObject startBtn;
    // SerializeField

    //종섭 수정 - SerializeField - private, GameObject - TextMeshProUGUI
    [SerializeField] private TextMeshProUGUI stageText, waveText, lifeText, mobCntText, timerText, killCntText, goldText, towerCntText, waveMobTotalText, stockText;
    [SerializeField] private TextMeshProUGUI[] playerStatusTextArray;
    [SerializeField] private TextMeshProUGUI[][] towerStatusTextArray;
    /*  
    * [3] 하이어라키 오브젝트
    *
    */
    private GameObject stagesObject;
    private GameObject totalManager;
    private GameObject uiManager;

    // 받아와야할 값들
    private void Awake()
    {
        startPoint = GameObject.FindGameObjectWithTag("StartPoint"); // startPoint 옮겨야함
        startBtn = GameObject.Find("StartBtn");
        stagesObject = GameObject.Find("Stages");
        totalManager = GameObject.Find("uiManager");
        uiManager = GameObject.Find("UI");

        // wave의 index는 현재 wave - 1
        allWaveInfos = startPoint.GetComponent<WaveSystem>().AllWaveInfos;

        // UI 값들 초기화

        stageText = uiManager.transform.Find("TopLeft_HUD_Infos/Info_StageAndTimer/Text_Stage").GetComponent<TextMeshProUGUI>();
        waveText = uiManager.transform.Find("TopLeft_HUD_Infos/Info_StageAndTimer/Text_Wave").GetComponent<TextMeshProUGUI>();
        timerText = uiManager.transform.Find("TopLeft_HUD_Infos/Info_StageAndTimer/Text_Timer").GetComponent<TextMeshProUGUI>();
        lifeText = uiManager.transform.Find("TopLeft_HUD_Infos/Info_Life/Text_Life").GetComponent<TextMeshProUGUI>();
        mobCntText = uiManager.transform.Find("TopLeft_HUD_Infos/Info_Mob/Text_MobCnt").GetComponent<TextMeshProUGUI>();
        killCntText = uiManager.transform.Find("TopLeft_HUD_Infos/Info_KillCnt/Text_KillCnt").GetComponent<TextMeshProUGUI>();
        goldText = uiManager.transform.Find("TopLeft_HUD_Infos/Info_Gold/Text_Gold").GetComponent<TextMeshProUGUI>();
        towerCntText = uiManager.transform.Find("TopLeft_HUD_Infos/Info_TowerCnt/Text_TowerCnt").GetComponent<TextMeshProUGUI>();
        stockText = uiManager.transform.Find("TopLeft_HUD_Infos/Info_Stock/Text_Stock").GetComponent<TextMeshProUGUI>();

        // UI 초기 값 할당
    }

    private void Start()
    {
        playerStatusTextArray = new TextMeshProUGUI[100];
        towerStatusTextArray = new TextMeshProUGUI[6][];

            // 1~100 : 유저, 101~200 : 타워, 201 ~ 300 : 몬스터

        stageText.text = stage.ToString();
        waveText.text = wave.ToString();
        lifeText.text = life.ToString();
        mobCntText.text = mobCnt.ToString();
        timerText.text = timer.ToString();
        killCntText.text = killCnt.ToString();
        goldText.text = gold.ToString();
        towerCntText.text = towerCnt.ToString();
        stockText.text = stocks.ToString();


        // 이건 임의로 내가 계속 정해줘야하나? 웨이브를 스크립트로 작성하지 않는 한
        finalWave = 15;
        enemyList = startPoint.GetComponent<EnemySpawner>().EnemyList;
        GameStart();
    }

    // 타워 설치
    public void BuildTower()    
    {
        towerCnt -= 1;
        hasTower += 1;
        towerCntText.text = towerCnt.ToString();
        
    }

    // 네비게이션용 업데이트(임시비활 )
    private void Update()
    {
        if (enemyList.Count > 0)
        {
            enemyNavi = enemyList[0];
        }
    }

    // 돈 관리
    public void Withdraw(int amount, bool isPlus)
    {
        if (isPlus)
        {
            gold += amount;
        }
        else
        {
            gold -= Mathf.Abs(amount);
        }
        goldText.text = gold.ToString();
    }

    public void UpdateStocks(int stock)
    {
        this.stocks += stock;
        stockText.text = this.stocks.ToString();

    }

    // 게임 시작
    public void GameStart()
    {
        isGameStart = true;
        //startBtn.SetActive(false);
        //timerText.GetComponent<TMP_Text>().text = "yes";
        startPoint.SetActive(true);
        GameObject nowStage = stagesObject.transform.GetChild(0).transform.Find("Here").gameObject;
        startPoint.transform.position = nowStage.transform.position;
        startPoint.GetComponent<WaveSystem>().StartWave();

    }

    // 몹 정보
    public void MobCounter(bool isPlus, bool isKill = true)
    {
        Debug.Log("werwerwer");
        // 웨이브 끝을 판단하는 로직
        if (isPlus)
        {
            mobCnt ++;
            mobCntText.text = mobCnt.ToString();
        } 
        else
        {
            if (isKill)
            {
                killCnt ++;
                mobCnt --;
                killCntText.text = killCnt.ToString();
                mobCntText.text = mobCnt.ToString();
            }
            else
            {
                mobCnt --;
                mobCntText.text = mobCnt.ToString();
            }
            if (isEndWave && mobCnt == 0)
            {
                isEndWave = false;

                ClearWave();
            }
        }
    }

    // 생명력 감소
    public void LooseLife(int amount)
    {
        Debug.Log("gm생명력감소");
        life -= Mathf.Abs(amount);
        lifeText.text = life.ToString();
        MobCounter(false, false);
        if (life <= 0)
        {
            lifeText.text = 0.ToString();
            //게임오버 메소드 호출
            GameEnd(false);
        }
    }
    public void towerCntUpdate(int count)
    {
        this.towerCnt += count;
    }

    // 타이머
        private IEnumerator Timer()
    {
        while ( timer > 0)
        {
            timer -= 1;
            timerText.GetComponent<TMP_Text>().text = timer.ToString();
            yield return new WaitForSeconds(1);
        }
        wave++;
        waveText.text = wave.ToString();
        stageText.text = stage.ToString();
        timerText.text = "yes";
        startPoint.GetComponent<WaveSystem>().StartWave();
    }

    // 타이머 시작
    private void StartTimer(int tieMur = 3)
    {
        // 임시 시간 수정
        timer = tieMur;
        timerText.GetComponent<TMP_Text>().text = timer.ToString();
        StartCoroutine("Timer");
    }

    // 웨이브 클리어
    private void ClearWave()
    {
        // 해당 웨이브 클리어, 보상 UI 출력, 최종웨이브 클리어시 게임셋
        if (wave == 5 && stage == 3)
        {
            GameEnd(true);
        } 
        else
        {
            if(wave % 5 == 0)
            {
                stage++;
                GameObject nowStage = stagesObject.transform.GetChild(stage - 1).transform.Find("Here").gameObject;
                startPoint.transform.position = nowStage.transform.position;
                wave = 0;
                StartTimer(10);

            }
            else
            {
                Debug.Log("그냥시작");
                StartTimer();
            }
        }        
    }

    private void GameEnd(bool gameResult)
    {
        // 결과 UI 출력
        // gameResult 가 true면 게임을 클리어한 ending, false면 클리어 실패 ending
        if (gameResult)
        {
            // clear ending
        }
        else
        {
            // failure ending
        }
    }

}
