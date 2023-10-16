using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{


    [SerializeField]
    private GameObject go_InventoryBase;
    //[SerializeField]
    //private GameObject go_SlotsParent;
    [SerializeField]
    private GameObject go_QuickSlotParent;

    //private Slot[] slots; // �κ��丮 ���Ե� 
    private Slot[] quickslots; // �� ���ϵ�
    private bool isNotPut;

    void Start()
    {
        //slots = go_SlotsParent.GetComponentsInChildren<Slot>();
        quickslots = go_QuickSlotParent.GetComponentsInChildren<Slot>();


    }

    
    void Update()
    {
       
        
    }

    public void AcquireItem(Item _item, int _count = 1)
    {
        PutSlot(quickslots, _item, _count);

        if (isNotPut)
            Debug.Log("�����԰� �κ��丮�� ��á���ϴ�");

        //if (Item.ItemType.Equipment != _item.itemType)
        //{
        //    for(int i = 0; i< slots.Length; i++)
        //    {
        //        if(slots[i].item != null)
        //        {
        //            if(slots[i].item.itemName == _item.itemName)
        //            {
        //                slots[i].SetSlotCount(_count);
        //                return;
        //            }

        //        }
        //    }

        //}

        //for(int i = 0; i < slots.Length; i++)
        //{
        //    if(slots[i].item == null)
        //    {
        //        slots[i].AddItem(_item, _count);
        //        return;
        //    }
        //}
    }


    private void PutSlot(Slot[] _slots, Item _item, int _count)
    {
        if (Item.ItemType.Equipment != _item.itemType)
        {
            for (int i = 0; i < _slots.Length; i++)
            {
                if (_slots[i].item != null)
                {
                    if (_slots[i].item.itemName == _item.itemName)
                    {
                        _slots[i].SetSlotCount(_count);
                        isNotPut = false;
                        return;
                    }

                }
            }

        }

        for (int i = 0; i < _slots.Length; i++)
        {
            if (_slots[i].item == null)
            {
                _slots[i].AddItem(_item, _count);
                isNotPut = false;
                return;
            }
        }

        isNotPut = true;
    }



}
