using Com.MyTutorial.Photon;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using TMPro;
using Newtonsoft.Json;
using static MyAPIClient;

public class ItemControllerForPhoton : MonoBehaviourPun
{
    public static readonly int MAX_ENHANCE_STACK = 50; // 최대 강화의 수 : 30
    public static readonly int MAX_BUFF_COUNT = 10; // 최대 버프의 수 : 10
    public static readonly int KIND_OF_STACK = 401; // 최대 강화종류의 수 : 301
                                                    // 유저강화 : 1~100, 타워강화 : 101~200, 몬스터강화 : 201 ~300, 계정 : 301 ~ 400

    private GameObject playerObject;
    private GameObject stageObject;
    private GameObject totalManager;

    
    //GameManger is for Photon. different GM which is for local gamemanager.
    private GameManager gm;
    private GMForPhoton gameManager;
    private PlayerStatForPhoton playerStat;
    ItemEnums.EnhanceItemEnums enhanceItems;
    private Coroutine buffcoroutine, debuffcoroutine; // 버프 코루틴, 디버프 코루틴
    private GameObject buffUI;
    private GameManager gameManagerObject;

    private int[] totalStackArray;
    private int[] enhanceStackArray;
    public int[] buffStackArray;
    public int[] debuffStackArray;
    private int[] accountStackArray;
    private GameObject[] buffUIArray;
    private GameObject passiveSkill;
    private GameObject[] passiveSkillArray;
    private GameObject[] charIconUI;
    private GameObject[] charStatUI;
    private MyAPIClient myApiClient;

    public bool isHammerUnlocked;


    private TextMeshProUGUI[] currentGoldTexts;

    private int[] buffIndex;
    private int[] debuffIndex;
    private Dictionary<int, Coroutine> buffCoroutineDic;
    private Dictionary<int, Coroutine> debuffCoroutineDic;

    private GameObject hammerUI, lockedUI;

    private void Awake()
    {
        currentGoldTexts = new TextMeshProUGUI[3];
        enhanceStackArray = new int[KIND_OF_STACK];
        buffStackArray = new int[KIND_OF_STACK];
        debuffStackArray = new int[KIND_OF_STACK];
        totalStackArray = new int[KIND_OF_STACK];
        accountStackArray = new int[KIND_OF_STACK];
        buffCoroutineDic = new Dictionary<int, Coroutine>();
        debuffCoroutineDic = new Dictionary<int, Coroutine>();
        buffIndex = new int[KIND_OF_STACK];
        debuffIndex = new int[KIND_OF_STACK];



        charStatUI = new GameObject[3];
        charIconUI = new GameObject[4];
        passiveSkillArray = new GameObject[8];

        stageObject = GameObject.Find("Stages").gameObject;
        totalManager = GameObject.Find("Manager").gameObject;
        myApiClient = totalManager.transform.Find("RestApiManager").GetComponent<MyAPIClient>();

        buffUIArray = new GameObject[4];
        buffUI = GameObject.Find("UI").transform.Find("LeftBottom_BuffUI/Buffs").gameObject;
        hammerUI = GameObject.Find("UI").transform.Find("MidCenter_PopUps/PopUp_CharacterSelect/Popup/Character_Toggle/Character_2/Icon_Character_Hammer").gameObject;
        lockedUI = GameObject.Find("UI").transform.Find("MidCenter_PopUps/PopUp_CharacterSelect/Popup/Character_Toggle/Character_2/Icon_Character_Locked").gameObject;
        hammerUI.SetActive(false);
        lockedUI.SetActive(false);

        getuserInfo();

        currentGoldTexts[0] = GameObject.Find("UI").transform.Find("MidCenter_EventUI/UI_Hostpital/UI_Shop/CurrentGoldInfo/Text_CurrentGoldInfo").GetComponent<TextMeshProUGUI>() ;
        currentGoldTexts[1] = GameObject.Find("UI").transform.Find("MidCenter_EventUI/UI_HamburgerShop/UI_Shop/CurrentGoldInfo/Text_CurrentGoldInfo").GetComponent<TextMeshProUGUI>();
        currentGoldTexts[2] = GameObject.Find("UI").transform.Find("MidCenter_EventUI/UI_EnhanceItemShop/UI_Shop/CurrentGoldInfo/Text_CurrentGoldInfo").GetComponent<TextMeshProUGUI>();


        for (int i = 0; i < buffUIArray.Length; i++)
        {
            buffUIArray[i] = buffUI.transform.GetChild(i).gameObject;
            buffUIArray[i].SetActive(false);
        }
        passiveSkill = GameObject.Find("UI").transform.Find("StatusToggleScreen/Info_LevelSkill").gameObject;
        for(int i = 0;i < passiveSkillArray.Length; i++)
        {
            passiveSkillArray[i] = passiveSkill.transform.GetChild(i).gameObject;
            passiveSkillArray[i].SetActive(false);
        }

        for(int i = 0; i< charIconUI.Length ; i++) {
            charIconUI[i] = GameObject.Find("UI").transform.Find("StatusToggleScreen/Info_Player/Icon_Character").GetChild(i).gameObject;
            charIconUI[i].SetActive(false);
        }

        for(int i=0; i< charStatUI.Length ;i++)
        {
            charStatUI[i] = GameObject.Find("UI").transform.Find("StatusToggleScreen/Info_Player").GetChild(i + 1).gameObject;
        }



    }
    void Start()
    {
        gameManager = totalManager.transform.Find("GMForPhoton").GetComponent<GMForPhoton>();

        playerObject = GameManager.Instance.player;
        //playerStat = playerObject.transform.GetComponent<PlayerStatForPhoton>();
        passiveSkillArray[0].SetActive(true);

        updateCurrentGold(gameManager.Gold.ToString());

    }

    // Update is called once per frame  
    void Update()
    {

    }

    public float getTotalStack(int order)
    {
        if (order == 102) // 타워 공격속도만 줄어드는 연산 적용
        {
            return 1 - ((float)totalStackArray[order] / 100);
        }

        return 1 + ((float)totalStackArray[order] / 100);

    }
    // 아이템으로 강화하기
    public int enhanceStackByItem(int enhanceItem, int stack) // 강화아이템을 스택만큼 강화하고 총 스택을 반환한다.
    {
        if (enhanceStackArray[enhanceItem] >= MAX_ENHANCE_STACK) // 최대 강화스택보다 크다면
        {
            Debug.Log("최대 강화스택 도달!");
        }
        else if (enhanceStackArray[enhanceItem] + stack >= MAX_ENHANCE_STACK)
        {
            enhanceStackArray[enhanceItem] = MAX_ENHANCE_STACK;
            Debug.Log("최대 강화스택 도달!");
        }
        else
        {
            enhanceStackArray[enhanceItem] += stack;
            Debug.Log(enhanceItem + " : " + stack + " 강 강화 성공!");
        }
        
        return updateStack(enhanceItem);
    }

    public int updateStack(int order)
    {

        totalStackArray[order] = enhanceStackArray[order] + buffStackArray[order] - debuffStackArray[order] + accountStackArray[order];

        updatePlayerStacks();
        return totalStackArray[order];
    }
    public void updateAllStack(int start, int end)
    {
        for (int i = start; i < end;)
        {
            totalStackArray[i] = enhanceStackArray[i] + buffStackArray[i] - debuffStackArray[i] + accountStackArray[i];
        }
        updatePlayerStacks();
    }

    public void buffByItem(bool isBuff, int buffItemOrder, float buffDuration, int buffValue)
    {
        // Type
        // isBuff : 버프, 디버프

        // Buff Item Order
        // 1,2,3 - 캐릭터 공격력, 이동속도, 사거리
        // 4,5,6 - 타워 공격력, 공격속도, 사거리
        // 7,8,9 - 몬스터 방어력, 이동속도, HP리젠

        // ex) 캐릭터 디버프로 5초간 이동속도 -100%를 주고싶다 : buffByItem(2,2,5,100);

        Dictionary<int, Coroutine> coroutineDic = isBuff ? buffCoroutineDic : debuffCoroutineDic;
        int[] BuffsArray = isBuff ? buffStackArray : debuffStackArray;

        if (coroutineDic.ContainsKey(buffItemOrder)) // 동일한 버프, 디버프가 있을 시, 새로운 디버프로 갈아끼운다
        {
            int previousBuff = BuffsArray[buffItemOrder];
            clearBuff(buffItemOrder, isBuff);
            buffValue += previousBuff; // 이전 버프능력치를 합쳐주기
        }

        int indexNum;
        switch (buffItemOrder)
        {
            case 1:
                indexNum = isBuff ? 0 : 2;
                buffUIArray[indexNum].SetActive(true);
                buffUIArray[indexNum].transform.GetChild(3).GetComponent<TextMeshProUGUI>().text = buffValue.ToString();
                break;
            case 2:
                indexNum = isBuff ? 1 : 3;
                buffUIArray[indexNum].SetActive(true);
                buffUIArray[indexNum].transform.GetChild(3).GetComponent<TextMeshProUGUI>().text = buffValue.ToString();
                break;
        }

        Coroutine coroutine = StartCoroutine(buffItemCoroutine(buffItemOrder, buffDuration, buffValue, isBuff));
        coroutineDic.Add(buffItemOrder, coroutine);


        // 키고


        Debug.Log($"{(ItemEnums.BuffItemEnums)buffItemOrder} 버프 적용! {buffDuration} 초, {buffValue} 만큼 진행");
    }


    public IEnumerator buffItemCoroutine(int buffItemOrder, float buffDuration, int buffValue, bool isBuff)
    {
        Dictionary<int, Coroutine> coroutineDic = isBuff ? buffCoroutineDic : debuffCoroutineDic;
        int[] buffArray = isBuff ? buffStackArray : debuffStackArray;
        // 일정 시간동안 버프 유지
        buffArray[buffItemOrder] += buffValue;
        updateStack(buffItemOrder);
        updatePlayerStacks();
        // 일정 시간동안 대기 후, 버프 해제

        yield return new WaitForSeconds(buffDuration);

        buffArray[buffItemOrder] -= buffValue;
        updateStack(buffItemOrder);
        coroutineDic.Remove(buffItemOrder);

        int indexNum;
        switch (buffItemOrder)
        {
            case 1:
                indexNum = isBuff ? 0 : 2;
                buffUIArray[indexNum].SetActive(false);
                break;
            case 2:
                indexNum = isBuff ? 1 : 3;
                buffUIArray[indexNum].SetActive(false);
                break;
        }
        updatePlayerStacks();

    }

    public void clearBuff(int buffItemOrder, bool isBuff)
    {
        Dictionary<int, Coroutine> bufCourtDic = isBuff ? buffCoroutineDic : debuffCoroutineDic;
        int[] buffArray = isBuff ? buffStackArray : debuffStackArray;
        if (bufCourtDic.ContainsKey(buffItemOrder))
        {
            Debug.Log($"{buffItemOrder}번 버프 {isBuff} 가지고있음");
            StopCoroutine(bufCourtDic[buffItemOrder]);
            bufCourtDic.Remove(buffItemOrder);
            buffArray[buffItemOrder] = 0;
            Debug.Log("버프, 디버프 해제 완료!");
        }
        else
        {
            Debug.Log("적용된 버프, 디버프가 없습니다!");
        }


    }

    public void clearAllBuffs(int type)
    {
        int start; int end; bool isBuff;
        switch (type)
        {
            case 1:
                start = 1; end = 4; isBuff = true;
                break;
            case 2:
                start = 1; end = 4; isBuff = false;
                break;
            case 3:
                start = 101; end = 104; isBuff = true;
                break;
            case 4:
                start = 101; end = 104; isBuff = false;
                break;
            case 5:
                start = 201; end = 204; isBuff = true;
                break;
            case 6:
                start = 201; end = 204; isBuff = false;
                break;
            default:
                start = 0; end = 0; isBuff = true;
                break;
        }
        for (int i = start; i < end; i++)
        {
            clearBuff(i, isBuff);
            updateStack(i);
        }
        buffUIArray[2].SetActive(false);
        buffUIArray[3].SetActive(false);

        updatePlayerStacks();
        
    }

    public void getAccountStacks(int charType, int exp)

    {

        charIconUI[charType].SetActive(true);

        if (exp >= 100) { gameManager.userLv = 1;  passiveSkillArray[1].SetActive(true); accountStackArray[1] += 20; }
        if (exp >= 250) { gameManager.userLv = 2;  passiveSkillArray[2].SetActive(true); gameManager.Withdraw(500, true); }
        if (exp >= 400) { gameManager.userLv = 3; passiveSkillArray[3].SetActive(true); accountStackArray[2] += 20; }
        if (exp >= 600) { gameManager.userLv = 4; passiveSkillArray[4].SetActive(true); gameManager.UpdateStocks(10); }
        if (exp >= 800)
        {
            gameManager.userLv = 5; passiveSkillArray[5].SetActive(true);
            accountStackArray[101] += 10; accountStackArray[102] += 10; accountStackArray[103] += 10;
            gameManager.towerCntUpdate(1);

        }
        if (exp >= 1000) { gameManager.userLv = 6; passiveSkillArray[6].SetActive(true); enhanceStackArray[3] += 10; }
        if (exp >= 2000)
        {

            passiveSkillArray[7].SetActive(true);
            gameManager.userLv = 7;
            accountStackArray[1] += 20;
            gameManager.Withdraw(500, true);
            accountStackArray[2] += 20;
            gameManager.UpdateStocks(10);
            accountStackArray[101] += 10; accountStackArray[102] += 10; accountStackArray[103] += 10;
            gameManager.towerCntUpdate(1);
            enhanceStackArray[3] += 10;
        }

        GameObject.Find("UI").transform.Find("StatusToggleScreen/Text_Level").GetComponent<TextMeshProUGUI>().text = "Lv : " + gameManager.userLv.ToString();
        GameObject.Find("UI").transform.Find("StatusToggleScreen/Text_Exp").GetComponent<TextMeshProUGUI>().text = "Exp : " + gameManager.userExp.ToString();

        updateCurrentGold(gameManager.Gold.ToString());
    }

    public void updatePlayerStacks()
    {
        charStatUI[0].transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = "+" + totalStackArray[1].ToString() ;
        charStatUI[0].transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = (gameManagerObject.playerStat.CurrentStatus(1) + 10f).ToString();
        charStatUI[1].transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = "+" + totalStackArray[2].ToString();
        charStatUI[1].transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = gameManagerObject.playerStat.CurrentStatus(2).ToString();
        charStatUI[2].transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = "+" + totalStackArray[3].ToString();
    }   

    public void getGameManger()
    {
        gameManagerObject = GameObject.Find("Manager").transform.Find("GameManager").GetComponent<GameManager>();
        


    }

    public void updateCurrentGold(string msg)
    {
        for(int i = 0; i < currentGoldTexts.Length; i++)
        {
            currentGoldTexts[i].text = msg;
        }
    }

    
    public void getuserInfo()
    {
        string url = MyAPIClient.BACKEND_URL + $"api/member/info/{PhotonNetwork.NickName}";
        Debug.Log("url : " + url);
        StartCoroutine(myApiClient.getRequest(url, getInfoSuccess, getInfoFailed)); 
    }

    public void getInfoSuccess(MyAPIClient.ResponseDTO response)
    {
           if(response.httpStatus == "OK")
        {
            Debug.Log("계정 정보 가져오기 성공!");
            InfoResponseDto infoResponseDto = JsonConvert.DeserializeObject<InfoResponseDto>(JsonConvert.SerializeObject(response.data));
            Debug.Log(infoResponseDto.isHammerUnlocked);
            Debug.Log(infoResponseDto.isAlyakOK);
            if(infoResponseDto.isAlyakOK)
            {
                // 알약이 20원으로 줄이기
                GameObject.Find("UI").transform.Find("MidCenter_EventUI/UI_Hostpital/UI_Shop/Stuffs/Item_No7_Medicine/Text_Cost").GetComponent<TextMeshProUGUI>().text = 20.ToString();
            }
            gameManager.userExp = infoResponseDto.exp;
            Debug.Log(gameManager.userExp);

            isHammerUnlocked = infoResponseDto.isHammerUnlocked;

            if (isHammerUnlocked)
            {
                hammerUI.SetActive(true);
                Debug.Log("해머딘 언락");
            }
            else
            {
                lockedUI.SetActive(true);
                Debug.Log("해머딘 못씀");
            }
        }
    }

    public void getInfoFailed(MyAPIClient.ResponseDTO response)
    {

    }

    public int returnTotalStacks(int type)
    {
        if (type == 1)
        {
            return totalStackArray[1] + totalStackArray[2] + totalStackArray[3];
        }
        else if (type == 101)
        {
            return totalStackArray[101] + totalStackArray[102] + totalStackArray[103];
        }
        else return 0;
        
    }

    public void submitResult(int exp, int coin)
    {
        string url = MyAPIClient.BACKEND_URL + "api/member/addExpCoin";
        string data = "{\"nickname\":\"" + PhotonNetwork.NickName + "\", \"coin\":\"" + coin + "\", \"exp\":\"" + exp + "\"}";

        StartCoroutine(myApiClient.PostRequest(url, data, submitAPISuccess, submitAPIFailed));
    }

    public void submitAPISuccess(ResponseDTO response)
    {
        if(response.httpStatus == "OK")
        {
        Debug.Log("결과 API 전송 성공!!"); 
            Debug.Log(response.msg);
        }
    }

    public void submitAPIFailed(ResponseDTO response)
    {
        Debug.Log("결과 API 전송 실패!");
    }

}
