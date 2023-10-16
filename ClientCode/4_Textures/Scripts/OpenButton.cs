using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OpenButton : MonoBehaviour
{
    // 새로 열 팝업 오브젝트를 수동으로 할당한다.
    [SerializeField] private GameObject popUp;
    public HandlePopUps handlePopUps;

    private void Start()
    {
        handlePopUps = FindObjectOfType<HandlePopUps>();
        GetComponent<Button>().onClick.AddListener(Open);
    }

    private void Open()
    {
        handlePopUps.ClosePopUp(popUp);
    }
}
