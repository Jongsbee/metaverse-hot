using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Photon.Pun;
public class UserEventControllerForPhoton : MonoBehaviourPun
{
    private GameObject totalManager;
    private GameObject uiManager;
    private GameObject stages;
    private GameObject statusToggleScreen;
    private GameObject miniMap;

    private GameObject enhanceItemShop; 
    private GameObject hostpital;
    private GameObject stockObject;
    private GameObject hamburgerShop;
    private GameObject jangSeung;

    public GameObject CharacterSelectPopUp;

    public TextMeshProUGUI actionText;
    private string eventMessage;
    private int eventNumber;
    private GMForPhoton gameManager;
    private BGM backGroundMusic;
    private Invest invest;
    private PhotonView PV;

    private TextMeshProUGUI textTemp;

    // Start is called before the first frame update
    private bool tabKeyPressed;
    private bool statusToggleActive;
    private bool mapKeyPressed;
    private bool miniMapActive;

    void Awake()
    {
        PV = GetComponent<PhotonView>();
        totalManager = GameObject.Find("Manager");
        uiManager = GameObject.Find("UI");
        stages = GameObject.Find("Stages");

        backGroundMusic = GameObject.Find("BGM").GetComponent<BGM>();

        gameManager = totalManager.transform.Find("GMForPhoton").GetComponent<GMForPhoton>();

        statusToggleScreen = uiManager.transform.Find("StatusToggleScreen").gameObject;
        miniMap = uiManager.transform.Find("MiniMap").gameObject;

        CharacterSelectPopUp = uiManager.transform.Find("MidCenter_PopUps/PopUp_CharacterSelect").gameObject;

        enhanceItemShop = uiManager.transform.Find("MidCenter_EventUI/UI_EnhanceItemShop").gameObject;
        hostpital = uiManager.transform.Find("MidCenter_EventUI/UI_Hostpital").gameObject;
        stockObject = uiManager.transform.Find("MidCenter_EventUI/UI_Stock").gameObject;
        hamburgerShop = uiManager.transform.Find("MidCenter_EventUI/UI_HamburgerShop").gameObject;
        jangSeung = uiManager.transform.Find("MidCenter_EventUI/UI_JangSeung").gameObject;

        actionText = uiManager.transform.Find("MidCenter_ShowText/actionText").GetComponent<TextMeshProUGUI>();
        
        textTemp = stockObject.transform.Find("UI_Shop/InvestGoldInfo/Text_InvestGoldInfo").gameObject.GetComponent<TextMeshProUGUI>();

        invest = stages.transform.Find("Stage2/EventBuildings/StockBuilding/Event_4_Stock").gameObject.GetComponent<Invest>();

    }

    void Start()
    {
        //actionText.gameObject.SetActive(false);

        enhanceItemShop.gameObject.SetActive(false);
        hostpital.gameObject.SetActive(false);
        stockObject.gameObject.SetActive(false);
        hamburgerShop.gameObject.SetActive(false);
        jangSeung.gameObject.SetActive(false);

        CharacterSelectPopUp.SetActive(true); // 캐릭터 셀렉창은 처음에 켜져있어야한다.


        // 스테이터스 창은 시작할 때 비활성화
        statusToggleScreen.SetActive(false);
        tabKeyPressed = false;
        statusToggleActive = false;

        // 미니맵은 시작할 때 활성화 상태로 시작
        mapKeyPressed = true;
        miniMapActive = true;
        // UI 텍스트 미리 할당해놓기

    }


    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            tabKeyPressed = true;
        }
        if (Input.GetKeyUp(KeyCode.Tab))
        {
            if (tabKeyPressed)
            {
                statusToggleActive = !statusToggleActive;
                statusToggleScreen.SetActive(statusToggleActive);
                // 스테이터스 창이 열릴 때는 미니맵을 비활성화, 닫을 때는 활성화
                miniMapActive = !statusToggleActive ? true : false;
                miniMap.SetActive(miniMapActive);
                backGroundMusic.TurnOnUI(statusToggleActive);
            }
            tabKeyPressed = false;
        }

        if (Input.GetKeyDown(KeyCode.M))
        {
            mapKeyPressed = true;
        }
        if (Input.GetKeyUp(KeyCode.M))
        {
            // 스테이터스 창이 안 닫혀 있을 때만 맵 토글 가능
            if (mapKeyPressed && !statusToggleActive)
            {
                miniMapActive = !miniMapActive;
                miniMap.SetActive(miniMapActive);
            }
            mapKeyPressed = false;
        }
    }

    public int EventInfoAppear(Collider other)
    {
        switch (other.gameObject.name)
        {
            case "Event_0_JangSeung": // 여신상 -> 장승
                eventMessage = "장승을 활성화";
                eventNumber = -2;
                break;
            case "Event_0_JangSeungForPhoton": // 여신상 -> 장승
                eventMessage = "장승을 활성화";
                eventNumber = 0;
                break;
            case "Event_1_Hospital": // 약국 -> 병원
                eventMessage = "알약이의 병원 입장";
                eventNumber = 1;
                break;
            case "Event_2_EnhanceItemShop":
                eventMessage = "스텟 강화 상점 입장";
                eventNumber = 2;
                break;
            case "Event_3_HamburgerShop":
                eventMessage = "위즐의 위스키 가게 입장";
                eventNumber = 3;
                break;
            case "Event_4_Stock":
                eventMessage = "싸피 주식 거래소 입장";
                eventNumber = 4;
                break;
            default:
                eventMessage = "알수없는 이벤트?";
                eventNumber = -1;
                break;
        }
        actionText.gameObject.SetActive(true);
        actionText.text = $"{eventMessage}" + "<color=yellow>" + "(E)" + "</color>";
        return eventNumber;

    }


    public void EventInfoDisappear()
    {
        actionText.gameObject.SetActive(false);
    }

    public void PlayEvent(int eventNumber)
    {
        switch (eventNumber)
        {
            case 0: // 장승
                jangSeung.gameObject.SetActive(true);

                break;
            case 1: // 알약이
                hostpital.gameObject.SetActive(true);

                break;
            case 2: // 스텟강화 상점
                enhanceItemShop.gameObject.SetActive(true);

                break;
            case 3: // 햄버거가게
                hamburgerShop.gameObject.SetActive(true);

                break;
            case 4: //  주식 거래소
                stockObject.gameObject.SetActive(true);

                break;
            case 5:
                break;
            case 6:
                break;
            case -1: // 알수없는 이벤트 코드
                break;
            default:
                break;

        }

        backGroundMusic.TurnOnUI(true);
    }

    public void updateCurrentGold()
    {
        textTemp.text = gameManager.Gold.ToString();
        Debug.Log(textTemp.text);
    }
}
