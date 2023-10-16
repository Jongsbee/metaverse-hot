using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 팝업 창을 열고 닫는 것을 관리하는 스크립트
public class HandlePopUps : MonoBehaviour
{
    // 활성화된 팝업 창을 담을 Stack
    public List<GameObject> popUps;

    // Scene이 로드될 때 자식 팝업 오브젝트 중 활성화된 것만 Push
    private void Awake()
    {
        popUps = new List<GameObject>();
        foreach (Transform child in transform)
        {
            if (child.gameObject.activeSelf)
            {
                popUps.Add(child.gameObject);
            }
        }
    }

    // 창을 열고 오브젝트를 스택에 Push
    public void OpenPopUp(GameObject popUp)
    {
        popUp.SetActive(true);
        popUps.Add(popUp);
    }

    // 창을 닫고 스택 Pop
    public void ClosePopUp(GameObject popUp)
    {
        popUp.SetActive(false);
        
        popUps.Remove(popUp);
    }

    // 모든 창을 닫고 스택이 전부 빌 때까지 Pop
    public void AllClosePopUp()
    {
        foreach (GameObject popUp in popUps)
        {
            popUp.SetActive(false);
        }
        popUps.Clear();
    }
}