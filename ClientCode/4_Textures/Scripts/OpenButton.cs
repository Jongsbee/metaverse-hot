using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OpenButton : MonoBehaviour
{
    // ���� �� �˾� ������Ʈ�� �������� �Ҵ��Ѵ�.
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
