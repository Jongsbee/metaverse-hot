using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuickSlotController : MonoBehaviour
{

    [SerializeField] private Slot[] quickSlots; // �����Ե�
    [SerializeField] private Transform tf_parent; // �������� �θ� ��ü

    private int selectedSlot;

    [SerializeField]
    private GameObject go_SelectedImage; // ���õ� �������� �̹���

    void Start()
    {
        quickSlots = tf_parent.GetComponentsInChildren<Slot>();
        selectedSlot = 0;
    }

    

    void Update()
    {
        TryInputNumber();
        
    }

    private void TryInputNumber()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            ChangeSlot(0);
        }else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            ChangeSlot(1);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            ChangeSlot(2);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            ChangeSlot(3);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            ChangeSlot(4);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha6))
        {
            ChangeSlot(5);
        }
       
    }

    private void ChangeSlot(int _num)
    {

        SelectedSlot(_num);

        Execute();
        
        
    }

    private void SelectedSlot(int _num)
    {
        // ���õ� ����
         selectedSlot = _num;
        // ���õ� �������� �̹��� �̵�
        go_SelectedImage.transform.position = quickSlots[selectedSlot].transform.position;

    }

    private void Execute()
    {
        if (quickSlots[selectedSlot].item != null)
        {

        }
        else
        {

        }
    }
}
