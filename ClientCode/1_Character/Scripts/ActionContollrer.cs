using UnityEngine;
using TMPro;

public class ActionContollrer : MonoBehaviour
{
    [SerializeField]
    private float range; //���� ������ �ִ� �Ÿ�.

    private bool pickupActivated = false; // ���� ������ �� true

    //private RaycastHit hitInfo; // �浹ü ���� ����

    [SerializeField]
    private LayerMask layerMask; // ������ ���̾�� �����ؾ� �Ѵ�.

    [SerializeField]
    private TextMeshProUGUI actionText;

    [SerializeField]
    private Inventory theInventory;

    private UserEventController userEventController;

    private BGM bgmController;

    private MiniMapController miniMapController;

    void Start()
    {
        userEventController = GameObject.Find("EventManager").GetComponent<UserEventController>();
        bgmController = GameObject.Find("BGM").GetComponent<BGM>();
        miniMapController = GameObject.Find("MiniMapCamera").GetComponent<MiniMapController>();
    }

    void Update()
    {

    }

    

    private void CanPickUp(Collider other)
    {
        if (pickupActivated)
        {
            if (other.transform != null)
            {
                Debug.Log(other.transform.GetComponent<ItemPickUp>()
            .item.itemName + "ȹ���߽��ϴ�.");
                theInventory.AcquireItem(other.transform.GetComponent<ItemPickUp>().item);
                Destroy(other.transform.gameObject);
                InfoDisappear();
            }
        }
    }

    

    void OnTriggerStay(Collider other)
    {
        // 종섭 수정 - switch 문으로 수정, eventTags 등록
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


        }

        //if(other.gameObject.tag == "Item")
        //{   
        //    ItemInfoAppear(other);

        //    if (Input.GetKey(KeyCode.Z))
        //    {
        //        CanPickUp(other);
        //   }

        //}
        
    }

    private void OnTriggerExit(Collider other)
    {
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
                bgmController.ChangeBgm(stageInfo);
                miniMapController.ChangeCam(stageInfo);
                break;



        }
    }

    private void ItemInfoAppear(Collider other)
    {
        pickupActivated = true;
        actionText.gameObject.SetActive(true);
        actionText.text = other.transform.GetComponent<ItemPickUp>()
            .item.itemName + "아이템 획득 " + "<color=yellow>" + "(Z)" + "</color>";
    }

    private void InfoDisappear()
    {
        pickupActivated = false;
        actionText.gameObject.SetActive(false);
    }
}
