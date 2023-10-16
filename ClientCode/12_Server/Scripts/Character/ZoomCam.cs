using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZoomCam : MonoBehaviour
{
    CinemachineCameraOffset cameraPos;
    Vector3 afterCameraPos;
    float m_CameraSpeed = 0.1f;
    void Start()
    {
        cameraPos = GetComponent<CinemachineCameraOffset>();
        afterCameraPos = cameraPos.m_Offset;
    }
    
    // Update is called once per frame
    void Update()
    {
        
        if(Input.GetAxis("Mouse ScrollWheel") > 0)
        {

            afterCameraPos = new Vector3(cameraPos.m_Offset.x,
             cameraPos.m_Offset.y - 20f,
             cameraPos.m_Offset.z + 10f);


            if(afterCameraPos.y <= -40f)
            {
                Debug.Log("1");
                afterCameraPos.y = -40f;
            }

            if(afterCameraPos.z >= 140f)
            {
                Debug.Log("2");
                afterCameraPos.z = 140f;
            }

        }

        if (Input.GetAxis("Mouse ScrollWheel") < 0)
        {

            afterCameraPos = new Vector3(cameraPos.m_Offset.x,
             cameraPos.m_Offset.y + 20f,
             cameraPos.m_Offset.z - 40f);


            if (afterCameraPos.y >= 50f)
            {
                Debug.Log("3");
                afterCameraPos.y = 30f;
            }

            if (afterCameraPos.z <= -100f)
            {
                Debug.Log("4");
                afterCameraPos.z = -100f;
            }

        }


        cameraPos.m_Offset.y = Mathf.Lerp(cameraPos.m_Offset.y, afterCameraPos.y, Time.deltaTime / m_CameraSpeed);
        cameraPos.m_Offset.z = Mathf.Lerp(cameraPos.m_Offset.z, afterCameraPos.z, Time.deltaTime / m_CameraSpeed);


    }

    



}
