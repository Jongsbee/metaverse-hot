using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InputFieldTabSignUp : MonoBehaviour
{
    public TMP_InputField inputName;
    public TMP_InputField inputID;
    public TMP_InputField inputPW;
    public TMP_InputField inputCPW;
   
   
   
    private void Awake()
    {
      

    }

  

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    private void Update()
    {
        if (Input.GetKeyUp(KeyCode.Tab))
        {
            if (EventSystem.current.currentSelectedGameObject == inputName.gameObject)
            {
                
                inputID.Select();
                inputID.ActivateInputField();
            }
            else if (EventSystem.current.currentSelectedGameObject == inputID.gameObject)
            {
                
                inputPW.Select();
                inputPW.ActivateInputField();
            }
            else if (EventSystem.current.currentSelectedGameObject == inputPW.gameObject)
            {
                inputCPW.Select();
                inputCPW.ActivateInputField();
            }
            else
            {
                inputName.Select();
                inputName.ActivateInputField();
            }
        }
    }

}