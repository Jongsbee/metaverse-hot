using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UiEventHandeler : MonoBehaviour
{
    [SerializeField] private GM gameManager;
    [SerializeField] private Invest invest;
    private GameObject uiManager, uiHospital, stageObject, uiStock;
    private TextMeshProUGUI investGoldText;
    private Statue jangSeung;
    [SerializeField] private ItemController itemController;


    private GameObject jangSeungUI;
    [SerializeField] private UserEventController userEventController;
    private int currentGold;

    
    private void Awake()
    {
        uiManager = GameObject.Find("UI").gameObject;
        stageObject = GameObject.Find("Stages").gameObject;

        gameManager = GameObject.Find("GM").GetComponent<GM>();
        jangSeungUI = uiManager.transform.Find("MidCenter_EventUI/UI_JangSeung").gameObject;
        jangSeung = stageObject.transform.Find("Stage1/EventBuildings/Event_0_JangSeung").GetComponent<Statue>();
        uiHospital = uiManager.transform.Find("MidCenter_EventUI/UI_Hostpital").gameObject;

        itemController = GameObject.Find("ItemManager").GetComponent<ItemController>();
        userEventController = GameObject.Find("EventManager").GetComponent<UserEventController>();
        invest = GameObject.Find("Event_4_Stock").GetComponent<Invest>();


    }
    void Start()
    {


    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void buyItems(GameObject gameObject)
    {
        int enhanceCost = int.Parse(gameObject.transform.Find("Text_Cost").GetComponent<TextMeshProUGUI>().text);
        Debug.Log("ItemCost : " + enhanceCost);
        if (gameManager.Gold < enhanceCost)
        {
            Debug.Log("돈부족!! 돈을 더갖고와!!");
        }
        else
        {
            gameManager.Withdraw(enhanceCost, false); // 가격만큼 돈을 차감한다.
            if (gameObject.name.Contains("Item_EH"))
            {
                switch (gameObject.name)
                {
                    case "Item_EHPlayerAttack": itemController.enhanceStackByItem(1, Random.Range(0, 3)); break;
                    case "Item_EHPlayerMoveSpeed": itemController.enhanceStackByItem(2, Random.Range(0, 3)); break;
                    case "Item_EHPlayerRange": itemController.enhanceStackByItem(3, Random.Range(0, 3)); break;
                    case "Item_EHTowerAttack": itemController.enhanceStackByItem(101, Random.Range(0, 3)); break;
                    case "Item_EHTowerAttackSpeed": itemController.enhanceStackByItem(102, Random.Range(0, 3)); break;
                    case "Item_EHTowerAttackRange": itemController.enhanceStackByItem(103, Random.Range(0, 3)); break;
                }   
            }
            else
            {
                switch (gameObject.name)
                {
                    case "Item_No7_Medicine": 
                        itemController.clearAllBuffs(2); // 2 : Player 모든 디버프 해제
                        uiHospital.SetActive(false); 
                        break;
                    case "Item_No8_Stock": 
                        invest.DoInvest() ;  // 주식 하나 사기
                        break; 
                    case "Item_No9_Hamburger": 
                        itemController.buffByItem(1, Random.Range(1, 4), 10,Random.Range(1,31));
                        itemController.buffByItem(2, 2, 10, 20);    
                        break;
                }
            }

        }




    }

    public void PlayEvent(GameObject gameObject)
    {
        Debug.Log(gameObject.name);
        
        switch (gameObject.name)
        {
            case "Button_Jangseung":
                Debug.Log("장승 이벤트!");
                jangSeung.Pray();

                gameManager.Punishable -= 1;
                jangSeung.StatueParticle.SetActive(gameManager.Punishable > 0);
                jangSeungUI.SetActive(gameManager.Punishable > 0);
                break;


            case "InvestGoldInfo":
                Debug.Log("주식 출금!");
                invest.GetResult() ;
                break;
            default:; break;
        }
        
    }

}
