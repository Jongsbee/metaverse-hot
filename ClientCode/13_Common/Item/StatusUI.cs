using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class StatusUI : MonoBehaviour
{
    // Start is called before the first frame update
    public TextMeshProUGUI[] playerStatusTexts;
    public TextMeshProUGUI[][] towerStatusTexts;


    private GameObject totalManager;
    private GameObject uiManager;
    private GameObject [] playerStatsUI;
    private GameObject [] towerStatsUI;

    private ItemController itemController;
    private PlayerStat playerStat;


    private void Awake()
    {
        towerStatsUI = new GameObject[6];
        playerStatsUI = new GameObject[3];
        playerStatusTexts = new TextMeshProUGUI[100];
        towerStatusTexts = new TextMeshProUGUI[7][];


        playerStat = GameObject.Find("Player").GetComponent<PlayerStat>();
        itemController = GameObject.Find("Manager").transform.Find("ItemManager").GetComponent<ItemController>();
        totalManager = GameObject.Find("Manager").gameObject;
        uiManager = GameObject.Find("UI").gameObject;
        for (int i = 0; i < playerStatsUI.Length; i++)
        {
            playerStatsUI[i] = uiManager.transform.Find("StatusToggleScreen/PlayerStatus").GetChild(i+1).gameObject;
        } // 0, 1, 2 -  데미지, 사정거리, 공격속도
        
        for (int i = 0; i < 6; i++) // 하드코딩
        {
            towerStatsUI[i] = uiManager.transform.Find("StatusToggleScreen/TowerStatusSet").GetChild(i).gameObject;
            Debug.Log($"이름 : + {towerStatsUI[i]} ");
            towerStatsUI[i].transform.GetChild(0).gameObject.SetActive(false); // 처음에는 타워 끄기
        } // 0~5 : 각각 타워

        Debug.Log("Awake complete");
        

    }
    void Start()
    {

        for (int j = 0; j < 5; j++)
        {
            playerStatusTexts[j+1] = playerStatsUI[0].transform.GetChild(j+3).GetComponent<TextMeshProUGUI>(); // 하드코딩
            playerStatusTexts[11+j] = playerStatsUI[1].transform.GetChild(j + 3).GetComponent<TextMeshProUGUI>(); // 하드코딩
            playerStatusTexts[21+j] = playerStatsUI[2].transform.GetChild(j + 3).GetComponent<TextMeshProUGUI>(); // 하드코딩
        }


        for(int i = 0; i < 30; i++)
        {
            updateStatus(i);
        }


    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void updateStatus(int order)
    {
        switch(order)
        {
           case 1: playerStatusTexts[1].text = (playerStat.CurrentStatus(1) + 10f).ToString(); break; // 무기 데미지 나중업데이트
            case 2:
                playerStatusTexts[2].text = "+" + itemController.getStackCounts(1).ToString(); break;
            case 3:
                playerStatusTexts[3].text = (playerStat.CurrentStatus(1) * itemController.getTotalStack(1)).ToString(); break;
            case 4:
                playerStatusTexts[4].text = 0.ToString(); break;
            case 5:
                playerStatusTexts[5].text = 10.ToString(); break; // 무기 데미지 나중업데이트

            case 11:
                playerStatusTexts[11].text = playerStat.CurrentStatus(2).ToString(); break;
            case 12:
                playerStatusTexts[12].text = "+" + itemController.getStackCounts(2).ToString(); break;
            case 13:
                playerStatusTexts[13].text = (playerStat.CurrentStatus(2) * itemController.getTotalStack(2)).ToString(); break;
            case 14:
                playerStatusTexts[14].text = 0.ToString(); break;
            case 15:
                playerStatusTexts[15].text = "X"; break;

            case 21:
                playerStatusTexts[21].text = playerStat.CurrentStatus(3).ToString(); break;
            case 22:
                playerStatusTexts[22].text = "+" + itemController.getStackCounts(3).ToString(); break;
            case 23:
                playerStatusTexts[23].text = (playerStat.CurrentStatus(3) * itemController.getTotalStack(3)).ToString(); break;
            case 24:
                playerStatusTexts[24].text = 0.ToString(); break;
            case 25: playerStatusTexts[25].text = "X"; break;
            default: break;
        }
        
    }
}
