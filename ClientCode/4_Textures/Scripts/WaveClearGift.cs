using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class WaveClearGift : MonoBehaviour
{
    [SerializeField] private GameObject popUp;
    private HandlePopUps handlePopUps;
    private GMForPhoton gM;
    public GameObject waveClearPopup, giftSet;
    private ItemControllerForPhoton itemController;
    // Start is called before the first frame update
    private void Start()
    {
        handlePopUps = FindObjectOfType<HandlePopUps>();
        gM = GameObject.Find("Manager").transform.Find("GMForPhoton").GetComponent<GMForPhoton>();
        waveClearPopup = GameObject.Find("UI").transform.Find("MidCenter_PopUps/PopUp_WaveClear").gameObject;
        giftSet = waveClearPopup.transform.Find("Popup/GiftSet").gameObject;
        itemController = GameObject.Find("Manager").transform.Find("ItemManager").GetComponent<ItemControllerForPhoton>();

        waveClearPopup.SetActive(false);

    }   

    // Update is called once per frame
    void Update()
    {
        
    }
    public void Close()
    {
        Debug.Log("보상 창을 닫습니다.");
        handlePopUps.ClosePopUp(popUp);
    }
    public void GiftGet(GameObject gameObject)
    {
        switch (gameObject.name)
        {
            case "GiftBox_1": 
                int randomGold = Random.Range(100, 201);
                gM.Withdraw(randomGold, true);
                Close();
                break;

            case "GiftBox_2":

                int randomTowerStack = Random.Range(101, 104);
                int randomTowerEnhance = Random.Range(1, 4);
                

                itemController.enhanceStackByItem(randomTowerStack, randomTowerEnhance);
                
                string towerMsg;
                switch(randomTowerStack)
                {
                    case 101: towerMsg = "타워 공격력"; break;
                    case 102: towerMsg = "타워 공격속도"; break;
                    case 103: towerMsg = "타워 사거리"; break;
                    default: towerMsg = "타워 속성"; break;
                }
                Close();


                break;

            case "GiftBox_3":

                int randomPlayerStack = Random.Range(1, 3);
                int randomPlayerEnhance = Random.Range(1, 4);

                itemController.enhanceStackByItem(randomPlayerStack, randomPlayerEnhance);
                string playerMsg;
                switch (randomPlayerStack)
                {
                    case 1: playerMsg = "플레이어 공격력"; break;
                    case 2: playerMsg = "플레이어 이동속도"; break;
                    default: playerMsg = "플레이어 속성"; break;
                }
                Close();

                break;

            case "StageGiftBox_1":
                int randomStageGold = Random.Range(200, 501);
                gM.Withdraw(randomStageGold, true);
                Close();
                break;

            case "StageGiftBox_2":

                int randomStageTowerStack = Random.Range(101, 104);
                int randomStageTowerEnhance = Random.Range(3, 6);


                itemController.enhanceStackByItem(randomStageTowerStack, randomStageTowerEnhance);


                switch (randomStageTowerStack)
                {
                    case 101: towerMsg = "타워 공격력"; break;
                    case 102: towerMsg = "타워 공격속도"; break;
                    case 103: towerMsg = "타워 사거리"; break;
                    default: towerMsg = "타워 속성"; break;
                }
                Close();


                break;

            case "StageGiftBox_3":

                int randomStagePlayerStack = Random.Range(1, 3);
                int randomStagePlayerEnhance = Random.Range(1, 4);

                itemController.enhanceStackByItem(randomStagePlayerStack, randomStagePlayerEnhance);
                switch (randomStagePlayerStack)
                {
                    case 1: playerMsg = "플레이어 공격력"; break;
                    case 2: playerMsg = "플레이어 이동속도"; break;
                    default: playerMsg = "플레이어 속성"; break;
                }
                Close();

                break;

        }
    }
}
