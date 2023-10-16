using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Photon.Pun;
using Photon.Pun.Demo.Cockpit;

public class GMForPhoton : MonoBehaviour
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

    private List<EnemyForPhoton> enemyList; // [1-3] 기지에서 가장 가까운 적의 위치(젤앞적transform.position)
    public List<EnemyForPhoton> EnemyList { get { return enemyList; } }
    private EnemyForPhoton enemyNavi;
    private GameObject accountExpGet, accountCoinGet;

    public EnemyForPhoton EnemyNavi { get { return enemyNavi; } }

    private bool isGameStart = false;   // [1-4] 방장의 게임 시작 여부

    private Transform myTowerNavi;  // [1-5] 내가 설치한 포탑의 위치

    private int punishable = 3; // 장승 사용 가능 횟수
    public int Punishable
    {
        get { return punishable; }
        set { punishable = value; }
    }

    // [1-6] 상대의 위치, 나와 다른 사람이 설치한 포탑의 위치?, 능력치, 골드 등(추가)

    // [1-7] 알림창 코멘트(이거 억덕하지 좀 빡세네)

    /*  
   * [2] 디폴트 설정 
   *
   */


    // [2-1] 계정 정보
    public int userExp;
    public int userLv;
    public int userCharType;
    private int expReward, coinReward;

    private string playerName = "TEST";
    private int characterIdx = 0;

    //[2-2] 게임 필수정보 (왼쪽상단 UI)  default gold is 0
    private int gold = 1000;
    private int stocks = 0;
    private int towerCnt = 10;
    private int killCnt = 0;
    private int hasTower = 0;

    //HaeSuk custom kill count event
    public int enemyKilled = 0;
    public int StockProperty
    {      // 주식
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
    UserEventControllerForPhoton userEventController;

    public int currentPlayer;

    // [2-3] Getter Setter
    public int Gold { get { return gold; } } // 골드
    public int TowerCnt { get { return towerCnt; } } // 타워
    private EnemyWavesData[] allWaveInfos;

    // 캐릭터 status(일단 보류)(강화정보)


    // 게임 정보


    /* 아이템정보 - 다른곳으로 옮김
     *  private List<GameObject> myItems;
     *  public List<GameObject> MyItems { get { return myItems; } }
     */


    //private int waveMobTotal;

    // [2-4] 웨이브 정보

    private int stage = 1;
    private int timer = 0;
    private int finalWave = 15;
    private ItemControllerForPhoton itemController;


    public PhotonView PV;
   
   
   

    GameObject startPoint;

    // SerializeField
    [SerializeField] Button startBtn; private TextMeshProUGUI stageText, waveText, lifeText, mobCntText, timerText, killCntText, goldText, towerCntText, waveMobTotalText, stockText;
    [SerializeField] private TextMeshProUGUI[] playerStatusTextArray;
    [SerializeField] private TextMeshProUGUI[][] towerStatusTextArray;

    /*  
    * [3] 하이어라키 오브젝트
    *
    */
    private GameObject stagesObject;
    private GameObject totalManager;
    private GameObject uiManager;
    public GameObject nowStage;

    // UserEventController 로 이동
    //[Tooltip("캐릭터 선택 UI")] 
    //[SerializeField] GameObject CharacterSelectPopUp;
    //[Tooltip("보상 UI")]
    private GameObject waveClearPopUp, stageClearPopUp, gameClearPopUp, gameOverPopUp;
    //[Tooltip("결과 UI")]
    //[SerializeField] GameObject ResultPopUp;
    //[Tooltip("상호작용 UI")]
    //[SerializeField] GameObject InteractionPopUp;


    // 받아와야할 값들
    private void Awake()
    {
        userExp = 500;
        userLv = 0;


        startPoint = GameObject.FindGameObjectWithTag("StartPoint");
        stagesObject = GameObject.Find("Stages");
        totalManager = GameObject.Find("Manager");
        uiManager = GameObject.Find("UI");
        itemController = totalManager.transform.Find("ItemManager").GetComponent<ItemControllerForPhoton>();
        startBtn = uiManager.transform.Find("StartBtn").gameObject.GetComponent<Button>();
        if (PhotonNetwork.IsMasterClient)
        {
            Debug.Log("마스터 클라이언트 입니다.");
            startBtn.gameObject.SetActive(true);
            startBtn.onClick.AddListener(CallStart);
            
        }
        else
        {
            Debug.Log("마스터 클라이언트가 아니므로 스타트 권한이 없습니다.");
            startBtn.gameObject.SetActive(false);
        }
        // wave의 index는 현재 wave - 1
        allWaveInfos = startPoint.GetComponent<WaveSystemForPhoton>().AllWaveInfos;
        userEventController = totalManager.transform.Find("EventManager").GetComponent<UserEventControllerForPhoton>();
        waveClearPopUp = uiManager.transform.Find("MidCenter_PopUps/PopUp_WaveClear").gameObject;
        stageClearPopUp = uiManager.transform.Find("MidCenter_PopUps/PopUp_StageClear").gameObject;
        gameClearPopUp = uiManager.transform.Find("MidCenter_PopUps/PopUp_Result_Cleared").gameObject;
        gameOverPopUp = uiManager.transform.Find("MidCenter_PopUps/PopUp_Result_Failed").gameObject;
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
        stageClearPopUp.SetActive(false);
        gameClearPopUp.SetActive(false);
        gameOverPopUp.SetActive(false); 


    }
    private void Start()
    {
        playerStatusTextArray = new TextMeshProUGUI[100];
        towerStatusTextArray = new TextMeshProUGUI[6][];

        // UI 초기 값 할당
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
        finalWave = 5;
        enemyList = startPoint.GetComponent<EnemySpawnerForPhoton>().EnemyList;
    }
    private void CallStart()
    {
        Debug.Log("CallStart() Called");
        
        if (PhotonNetwork.InRoom)
        {
            Debug.Log("방에 들어가있고 닫는 작업을 진행합니다.");

            PhotonNetwork.CurrentRoom.IsVisible = false;
            PhotonNetwork.CurrentRoom.IsOpen = false;
        }
        PV.RPC("GameStart", RpcTarget.All); 
    }
    // 타워 설치
    public void BuildTower()
    {
        towerCnt -= 1;
        towerCntText.text = towerCnt.ToString();
        // towerCntText.GetComponent<TMP_Text>().text = towerCnt.ToString();
    }

    // 네비게이션용 업데이트(임시비활)
    private void Update()
    {
        if (enemyList.Count > 0)
        {
            enemyNavi = enemyList[0];
        }

        mobCntText.text = enemyList.Count.ToString();

      
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

    [PunRPC]
    public void GameStart()
    {
        currentPlayer = PhotonNetwork.PlayerList.Length;
        Debug.Log("GM currentPlayer = ? " + currentPlayer);
        Debug.Log("게임스타트!!!");
        isGameStart = true;
        startBtn.gameObject.SetActive(false);
        timerText.GetComponent<TMP_Text>().text = "-";
        startPoint.SetActive(true);
        nowStage = stagesObject.transform.GetChild(0).transform.Find("Here").gameObject;
        startPoint.transform.position = nowStage.transform.position;
        startPoint.GetComponent<WaveSystemForPhoton>().StartWave();
        waveClearPopUp.SetActive(true);
    }

    public void EnemyKilled()
    {
        enemyKilled++;
        killCntText.text = enemyKilled.ToString();
       
      
    }

    

    // 생명력 감소
    public void LooseLife(int amount)
    {
        life -= Mathf.Abs(amount);
        lifeText.text = life.ToString();
        //PV.RPC("MobCounter", RpcTarget.AllBuffered, false, false);
       
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
        while (timer > 0)
        {
            timer -= 1;
            timerText.text = timer.ToString();
            yield return new WaitForSeconds(1);
        }
        wave++;
        waveText.text = wave.ToString();
        stageText.text = stage.ToString();
        timerText.text = "-";
        startPoint.GetComponent<WaveSystemForPhoton>().StartWave();
    }

    // 타이머 시작
    private void StartTimer(int tieMur = 3)
    {
        timer = tieMur;
        timerText.GetComponent<TMP_Text>().text = timer.ToString();
        StartCoroutine("Timer");
    }

    // 웨이브 클리어
    // HaeSuk private -> public
    public void ClearWave()
    {
        // 해당 웨이브 클리어, 보상 UI 출력, 최종웨이브 클리어시 게임셋
        if (wave == 5 && stage == 3)
        {
            GameEnd(true);
            
        }
        else
        {
            if (wave % 5 == 0)
            {
                stage++;
                nowStage = stagesObject.transform.GetChild(stage - 1).transform.Find("Here").gameObject;
                startPoint.transform.position = nowStage.transform.position;
                wave = 0;
                // 보스웨이브
                
                StartTimer(10);
                stageClearPopUp.SetActive(true);
            }
            else
            {
                // 알반웨이브
                waveClearPopUp.SetActive(true);
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
            //userEventController.ResultClearedPopUp.SetActive(true);
            // clear ending
            accountExpGet = GameObject.Find("UI").transform.Find("MidCenter_PopUps/PopUp_Result_Cleared/Popup/Exp/Text_ExpAmount").gameObject;
            accountCoinGet = GameObject.Find("UI").transform.Find("MidCenter_PopUps/PopUp_Result_Cleared/Popup/Coin/Text_CoinAmount").gameObject;

            gameClearPopUp.SetActive(true);
            accountResult(true);
        }
        else
        {
            //userEventController.ResultFailedPopUp.SetActive(true);
            this.life = 999;
            gameOverPopUp.SetActive(true);
            accountResult(false);
        }
            
            // failure ending

    }

    public void accountResult(bool isGameClear)
    {
        if(isGameClear) // 클리어 했을경우
        {
            accountExpGet = GameObject.Find("UI").transform.Find("MidCenter_PopUps/PopUp_Result_Cleared/Popup/Exp/Text_ExpAmount").gameObject;
            accountCoinGet = GameObject.Find("UI").transform.Find("MidCenter_PopUps/PopUp_Result_Cleared/Popup/Coin/Text_CoinAmount").gameObject;
        }else // 클리어 못했을 경우
        {
            accountExpGet = GameObject.Find("UI").transform.Find("MidCenter_PopUps/PopUp_Result_Failed/Popup/Exp/Text_ExpAmount").gameObject;
            accountCoinGet = GameObject.Find("UI").transform.Find("MidCenter_PopUps/PopUp_Result_Failed/Popup/Coin/Text_CoinAmount").gameObject;
        }

        for(int i = 1; i <= wave; i++)
        {
            expReward += i;
            coinReward += (1+i/5)*10;
        }
        
            accountExpGet.GetComponent<TextMeshProUGUI>().text = expReward.ToString();
            accountCoinGet.GetComponent<TextMeshProUGUI>().text = coinReward.ToString();
            itemController.submitResult(expReward, coinReward);

    }








}

