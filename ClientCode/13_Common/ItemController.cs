using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class ItemController : MonoBehaviour
{
    public static readonly int MAX_ENHANCE_STACK = 30; // 최대 강화의 수 : 30
    public static readonly int MAX_BUFF_COUNT = 10; // 최대 버프의 수 : 10
    public static readonly int KIND_OF_STACK = 401; // 최대 강화종류의 수 : 301
                                                    // 유저강화 : 1~100, 타워강화 : 101~200, 몬스터강화 : 201 ~300, 계정 : 301 ~ 400

    private GameObject playerObject;
    private GameObject stageObject;
    private GameObject totalManager;


    private GM gameManager;
    private PlayerStat playerStat;
    private StatusUI statusUI;
    ItemEnums.EnhanceItemEnums enhanceItems;
    private Coroutine buffcoroutine, debuffcoroutine; // 버프 코루틴, 디버프 코루틴

    private int[] totalStackArray;
    private int[] enhanceStackArray;
    public int[] buffStackArray;
    public int[] debuffStackArray;
    private int[] accountStackArray;


    private int[] buffIndex;
    private int[] debuffIndex;
    private Dictionary<int, Coroutine> buffCoroutineDic;
    private Dictionary<int, Coroutine> debuffCoroutineDic;

    // 최대 강화 스택


    // 기본 강화스택



    // Start is called before the first frame update

    void Awake()
    {
        playerObject = GameObject.Find("Player").gameObject;
        stageObject = GameObject.Find("Stages").gameObject;
        totalManager = GameObject.Find("Manager").gameObject;

        playerStat = playerObject.GetComponent<PlayerStat>();
        statusUI = totalManager.transform.Find("EventManager").GetComponent<StatusUI>();
        gameManager = totalManager.transform.Find("GM").GetComponent<GM>();

        enhanceStackArray = new int[KIND_OF_STACK];
        buffStackArray = new int[KIND_OF_STACK];
        debuffStackArray = new int[KIND_OF_STACK];
        totalStackArray = new int[KIND_OF_STACK];
        accountStackArray = new int[KIND_OF_STACK];

        
    }
    public void Start()
    {
        buffCoroutineDic = new Dictionary<int, Coroutine>();
        debuffCoroutineDic = new Dictionary<int, Coroutine>();
        buffIndex = new int[KIND_OF_STACK];
        debuffIndex = new int[KIND_OF_STACK];


        if (gameManager.userExp >= 100) { gameManager.userLv = 1; accountStackArray[0] += 20; }
        if (gameManager.userExp >= 300) { gameManager.userLv = 2; gameManager.Withdraw(300, true); }
        if (gameManager.userExp >= 450) { gameManager.userLv = 3; accountStackArray[1] += 20; }
        if (gameManager.userExp >= 600) { gameManager.userLv = 4; accountStackArray[2] += 20; }
        if (gameManager.userExp >= 800) { gameManager.userLv = 5; gameManager.towerCntUpdate(1); }
        if (gameManager.userExp >= 1000) { gameManager.userLv = 6; accountStackArray[3] += 20; }
        if (gameManager.userExp >= 2000)
        {
            gameManager.userLv = 7;
            accountStackArray[0] += 20;
            gameManager.Withdraw(300, true);
            accountStackArray[1] += 20;
            accountStackArray[2] += 20;
            gameManager.towerCntUpdate(1);
            accountStackArray[3] += 20;
        }
    }



    // Getter Setter

    public float getTotalStack(int order)
    {
        if (order == 102)
        {
            return 1 - ((float)totalStackArray[order] / 100);
        }

        return 1 + ((float)totalStackArray[order] / 100);

    }

    public int getStackCounts(int order)
    {
        return totalStackArray[order];
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
        Debug.Log($"공격력스택 : {totalStackArray[1]} , 이동속도스택 :  {totalStackArray[2]}  , 공격범위스택 : {totalStackArray[3]}");
        Debug.Log($"타워공격력스택 : {totalStackArray[101]} , 타워속도스택 :  {totalStackArray[102]}  , 타워범위스택 : {totalStackArray[103]}");

        int j = 0;
        switch (order)
        {
            
            case 1:
                j = 1;
                break;
            case 2: j = 11; break;
            case 3: j = 21; break;

        }

        for (int i = j; i < j + 5; i++)
        {
            statusUI.updateStatus(i);
        }


        return totalStackArray[order];
    }

    public void updateAllStack(int start, int end)
    {
        for (int i = start; i < end;)
        {
            totalStackArray[i] = enhanceStackArray[i] + buffStackArray[i] - debuffStackArray[i] + accountStackArray[i];
        }
    }

    public void buffByItem(int type, int buffItemOrder, float buffDuration, int buffValue)
    {
        bool isBool = (type % 2 == 1) ? true : false;
        Dictionary<int, Coroutine> coroutineDic = isBool ? buffCoroutineDic : debuffCoroutineDic ;

        if (coroutineDic.ContainsKey(buffItemOrder)) // 동일한 버프, 디버프가 있을 시, 새로운 디버프로 갈아끼운다
        {
            clearBuff(buffItemOrder, isBool);
        }
        Coroutine coroutine = StartCoroutine(buffItemCoroutine(buffItemOrder, buffDuration, buffValue, isBool));
        coroutineDic.Add(buffItemOrder, coroutine);


        Debug.Log($"{(ItemEnums.BuffItemEnums)type} 버프 적용! {buffDuration} 초, {buffValue} 만큼 진행");
    }


    public IEnumerator buffItemCoroutine(int buffItemOrder, float buffDuration, int buffValue, bool isBuff)
    {
        Dictionary<int, Coroutine> coroutineDic = isBuff ? buffCoroutineDic : debuffCoroutineDic;
        int[] buffArray = isBuff ? buffStackArray : debuffStackArray;
        // 일정 시간동안 버프 유지
        buffArray[buffItemOrder] += buffValue;
        updateStack(buffItemOrder);
        // 일정 시간동안 대기 후, 버프 해제

        yield return new WaitForSeconds(buffDuration);

        Debug.Log("버프, 디버프 해제!");
        buffArray[buffItemOrder] -= buffValue;
        updateStack(buffItemOrder);
        coroutineDic.Remove(buffItemOrder);
    }

    public void clearBuff(int buffItemOrder, bool isBuff)
    {
        Dictionary<int, Coroutine> bufCourtDic = isBuff ? buffCoroutineDic : debuffCoroutineDic;
        int[] buffArray = isBuff ? buffStackArray : debuffStackArray;
        if (bufCourtDic.ContainsKey(buffItemOrder))
        {
            StopCoroutine(bufCourtDic[buffItemOrder]);
            bufCourtDic.Remove(buffItemOrder);
            buffArray[buffItemOrder] = 0;
            Debug.Log("버프, 디버프 해제 완료!");
        }else
        {
            Debug.Log("적용된 버프, 디버프가 없습니다!");
        }
        

    }

    public void clearAllBuffs(int type)
    {
        int start;int end; bool isBuff;
        switch (type)
        {
            case 1: start = 1; end = 4; isBuff = true;
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
            clearBuff(i,isBuff);
        }
    }
}
