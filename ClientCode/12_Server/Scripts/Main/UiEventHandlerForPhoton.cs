using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UiEventHandlerForPhoton : MonoBehaviour
{
    [SerializeField] private GMForPhoton gameManager;
    [SerializeField] private Invest invest;
    private GameObject uiManager, uiHospital, stageObject, uiStock;
    private TextMeshProUGUI investGoldText;
    private StatueForPhoton jangSeung;
    [SerializeField] private ItemControllerForPhoton itemController;

    GameObject[] topEventUI;
    GameObject TopEventUIObject;

    private GameObject jangSeungUI;
    [SerializeField] private UserEventControllerForPhoton userEventController;
    private Button[] UiButtons;
    private int currentGold;
    int enhanceCost;
    // Start is called before the first frame update
    void Awake()
    {
        UiButtons = new Button[100];
        uiManager = GameObject.Find("UI").gameObject;
        stageObject = GameObject.Find("Stages").gameObject;

        gameManager = GameObject.Find("GMForPhoton").GetComponent<GMForPhoton>();
        jangSeungUI = uiManager.transform.Find("MidCenter_EventUI/UI_JangSeung").gameObject;
        jangSeung = stageObject.transform.Find("Stage1/EventBuildings/Event_0_JangSeungForPhoton").GetComponent<StatueForPhoton>();
        uiHospital = uiManager.transform.Find("MidCenter_EventUI/UI_Hostpital").gameObject;
        TopEventUIObject = uiManager.transform.Find("TopCenter_EventText").gameObject;

        topEventUI = new GameObject[3];
        for(int i = 0; i < topEventUI.Length; i++) { 
            topEventUI[i] = TopEventUIObject.transform.GetChild(i).gameObject;
            Debug.Log($"topEventNAme : {topEventUI[i].name}");
            topEventUI[i].SetActive(false) ;
                }


        itemController = GameObject.Find("ItemManager").GetComponent<ItemControllerForPhoton>();
        userEventController = GameObject.Find("EventManager").GetComponent<UserEventControllerForPhoton>();
        invest = GameObject.Find("Event_4_Stock").GetComponent<Invest>();

        
    }

    private void Start()
    {

    }


    //}
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
            itemController.updateCurrentGold(gameManager.Gold.ToString());
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
                        invest.DoInvest();  // 주식 하나 사기
                        break;
                    case "Item_No9_Hamburger":
                        itemController.buffByItem(true, Random.Range(1, 3), 120, Random.Range(10, 31));
                        itemController.buffByItem(false, 2, 60, 20);
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
                int randomValue = Random.Range(1, 101);
                jangSeung.photonView.RPC("Pray", RpcTarget.All,randomValue);
                gameManager.Punishable -= 1;
                jangSeung.StatueParticle.SetActive(gameManager.Punishable > 0);
                jangSeungUI.SetActive(gameManager.Punishable > 0);
                break;


            case "InvestGoldInfo":
                Debug.Log("주식 출금!");
                invest.GetResult();
                break;
            default:; break;
        }

    }

    public IEnumerator noticeTopEventUI(bool isBool, string msg)
    {
        topEventUI[0].SetActive(true);
        if (isBool)
        {
            topEventUI[1].GetComponent<TextMeshProUGUI>().text = msg;
            topEventUI[1].SetActive(true);

        }
        else
        {
            topEventUI[2].GetComponent<TextMeshProUGUI>().text = msg;
            topEventUI[2].SetActive(true);

        }

        yield return new WaitForSeconds(3f);

        topEventUI[0].SetActive(false);
        if (isBool)
        {
            topEventUI[1].SetActive(false);
        }
        else
        {
            topEventUI[2].SetActive(false);
        }
    }

}
