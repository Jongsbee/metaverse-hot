using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AllCloseButton : MonoBehaviour
{
    public HandlePopUps handlePopUps;

    private void Start()
    {
        handlePopUps = FindObjectOfType<HandlePopUps>();
        GetComponent<Button>().onClick.AddListener(AllClose);
    }

    public void AllClose()
    {
        handlePopUps.AllClosePopUp();
    }
}
