using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CloseButton : MonoBehaviour
{

    // 닫아야 할 팝업 오브젝트를 수동으로 할당한다. (프리팹마다 오브젝트 계층 구조가 다름)
    [SerializeField] private GameObject popUp;
    public HandlePopUps handlePopUps;

    BGM bgm;

    public void Start()
    {
        handlePopUps = FindObjectOfType<HandlePopUps>();
        GetComponent<Button>().onClick.AddListener(Close);
        bgm = GameObject.Find("BGM").GetComponent<BGM>();
    }

    public void Close()
    {
        handlePopUps.ClosePopUp(popUp);
        bgm.TurnOnUI(false);
    }
}
