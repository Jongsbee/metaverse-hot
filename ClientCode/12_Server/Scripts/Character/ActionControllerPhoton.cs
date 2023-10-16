using LayerLab;
using Photon.Pun;
using TMPro;
using UnityEngine;

public class ActionControllerPhoton : MonoBehaviourPunCallbacks
{
    [SerializeField]
    private float range; //습득 가능한 최대 거리.

    private bool pickupActivated = false; // 습득 가능할 시 true

    [SerializeField]
    private LayerMask layerMask; // 아이템 레이어에만 반응해야 한다.

    [SerializeField]
    private Inventory theInventory;

    public PhotonView PV;

    private TextMeshProUGUI actionText;
   
    private UserEventControllerForPhoton userEventController;

    private BGM bgmController;

    private MiniMapController miniMapController;

   
    void Awake()
    {
        userEventController = GameObject.Find("EventManager").GetComponent<UserEventControllerForPhoton>();
        actionText = userEventController.actionText;
        if(actionText == null)
        {
            Debug.Log("할당되어있지않음");
        }
        else
        {
            actionText.gameObject.SetActive(false);
        }
        bgmController = GameObject.Find("BGM").GetComponent<BGM>();
        miniMapController = GameObject.Find("MiniMapCamera").GetComponent<MiniMapController>();

    }
    void Start()
    {
      
    }

    void Update()
    {
        
    }

    private void CanPickUp(Collider other)
    {
        if (pickupActivated && PV.IsMine)
        {
            if (other.transform != null)
            {
                Debug.Log(other.transform.GetComponent<ItemPickUp>()
            .item.itemName + "획득했습니다.");
                theInventory.AcquireItem(other.transform.GetComponent<ItemPickUp>().item);
                Destroy(other.transform.gameObject);
                InfoDisappear();
            }
        }
    }



    void OnTriggerStay(Collider other)
    {
        if (!PV.IsMine)
        {
            return;
        }

    

        switch (other.gameObject.tag)
        {
            case "Item":
                // Item UI 뜨게하기
                ItemInfoAppear(other);
                if (Input.GetKey(KeyCode.Z))
                {
                    CanPickUp(other);
                }
                break;

            case "EventTags":
                Debug.Log("이벤트 발생!");
                int eventNumber = userEventController.EventInfoAppear(other);
                if (Input.GetKey(KeyCode.E))
                {
                    userEventController.PlayEvent(eventNumber);
                    Debug.Log($" 이벤트 발생!!");
                }
                break;

            case "TowerNode":
                BuildInfoAppear();
                break;


        }

    }

    private void OnTriggerExit(Collider other)
    {
        if (!PV.IsMine) { return; }

        switch (other.gameObject.tag)
        {
            case "Item":
                InfoDisappear(); // item 사라지기

                break;

            case "EventTags":
                userEventController.EventInfoDisappear();

                break;

            case "BackgroundChange":
                Debug.Log("배경음악 Change");
                string stageInfo = other.name;
                Debug.Log($"스테이지 인포 : {stageInfo}");
                bgmController.ChangeBgm(stageInfo);
                miniMapController.ChangeCam(stageInfo);
                break;

            case "TowerNode":
                BuildInfoDisappear();
                break;

        }

       
    }

    private void ItemInfoAppear(Collider other)
    {
        pickupActivated = true;
        actionText.gameObject.SetActive(true);
        actionText.text = other.transform.GetComponent<ItemPickUp>()
            .item.itemName + "획득 " + "<color=yellow>" + "(Z)" + "</color>";
    }

    private void InfoDisappear()
    {
        pickupActivated = false;
        actionText.gameObject.SetActive(false);
    }

    private void BuildInfoAppear()
    {
        
        actionText.gameObject.SetActive(true);
        actionText.text = "타워 건설 " + "<color=yellow>" + "(E)" + "</color>";
    }

    private void BuildInfoDisappear()
    {
        actionText.text = "";
        actionText.gameObject.SetActive(false);
    }


}
