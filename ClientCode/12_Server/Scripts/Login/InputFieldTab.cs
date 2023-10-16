using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InputFieldTab : MonoBehaviour
{
    private TMP_InputField inputID;
    private TMP_InputField inputPW;
   
    private void Awake()
    {
        
        inputID = GameObject.Find("Email_Field").GetComponent<TMP_InputField>();
        inputPW = GameObject.Find("Password_Field").GetComponent<TMP_InputField>();
       
        
    }



    // Update is called once per frame
    private void Update()
    {
        if (Input.GetKeyUp(KeyCode.Tab))
        {
            if (EventSystem.current.currentSelectedGameObject == inputID.gameObject)
            {
                
                inputPW.Select();
                inputPW.ActivateInputField();
            }
            else if (EventSystem.current.currentSelectedGameObject == inputPW.gameObject)
            {
              
                inputID.Select();
                inputID.ActivateInputField();
            }
           
        }
        
    }
   
}