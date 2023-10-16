using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiniMapController : MonoBehaviour
{
    public void ChangeCam(string stage)
    {
        switch (stage)
        {
            case "EnterStage1":
                transform.position = new Vector3(0, 900, 0);
                break;

            case "EnterStage2":
                transform.position = new Vector3(0, 900, -1000);
                break;

            case "EnterStage3":
                transform.position = new Vector3(0, 900, -2000);
                break;
        }

    }
}
