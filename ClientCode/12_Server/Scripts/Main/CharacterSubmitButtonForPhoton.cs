using UnityEngine;
using UnityEngine.UI;

public class CharacterSubmitButtonForPhoton : MonoBehaviour
{
    [SerializeField] private GameObject popUp;
    private HandlePopUps handlePopUps;
    private GMForPhoton gM;
    private GameObject playerObject;
    private GameObject[] character;
    private string toggleSelected;
    private void Start()
    {
        handlePopUps = FindObjectOfType<HandlePopUps>();
        GetComponent<Button>().onClick.AddListener(Submit);
        // Memo : 추후에 GameManager 오브젝트 이름 확정 필요
        gM = GameObject.Find("GMForPhoton").GetComponent<GMForPhoton>();
        //playerObject = GameObject.Find("Player").gameObject;
        toggleSelected = "Character_0"; // 초기값 할당 필요

        //character = new GameObject[4];
        //for (int i = 0; i < 2; i++) // 지금은 두번만 돌게되어있음 (전사는 setActive true)
        //{
        //    character[i] = playerObject.transform.GetChild(i).gameObject;
        //    character[i].SetActive(false);
        //}
        //character[0].SetActive(true);

    }
    // CloseButton의 Close 함수를 오버라이드
    private void Submit()
    {
        Debug.Log("Submitted " + toggleSelected.Substring(10));

        gM.CharName = toggleSelected;
        Debug.Log(toggleSelected);
        //character[0].SetActive(false);
        //character[int.Parse(toggleSelected.Substring(10))].SetActive(true);
        handlePopUps.ClosePopUp(popUp);


    }
    public void OnToggleClicked(Toggle toggle)
    {
        if (toggle.isOn)
        {
            toggleSelected = toggle.gameObject.name;
            Debug.Log("Selected " + toggleSelected.Substring(10));
        }
    }
}
