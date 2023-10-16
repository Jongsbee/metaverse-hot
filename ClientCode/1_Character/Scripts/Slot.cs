using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Slot : MonoBehaviour
{

    public Item item; // ȹ���� ������
    public int itemCount; // ȹ���� ������ ����
    public Image itemImage; // �������� �̹���

    // �ʿ��� ������Ʈ
    [SerializeField]
    private TextMeshProUGUI text_Count;
    [SerializeField]
    private GameObject go_CountImage;

    // �̹����� ����� ����
    private void SetColor(float _alpha)
    {
        Color color = itemImage.color;
        color.a = _alpha;
        itemImage.color = color;

    }

    //������ ȹ��
    public void AddItem(Item _item, int _count = 1)
    {
        item = _item;
        itemCount = _count;
        itemImage.sprite = item.itemImage;

        if(item.itemType != Item.ItemType.Equipment)
        {
            go_CountImage.SetActive(true);
            text_Count.text = "x" + itemCount.ToString();
        }
        else
        {
            text_Count.text = "x0";
            go_CountImage.SetActive(false);
        }
        

        // �������� ���� �κ�â�� �����ش�
        SetColor(1);
    }

    // ������ ���� ����
    public void SetSlotCount(int _count)
    {
        itemCount += _count;
        text_Count.text = "x" + itemCount.ToString();

        if(itemCount <= 0)
        {
            ClearSlot();
        }
    }

    // ���� �ʱ�ȭ
    private void ClearSlot()
    {
        item = null;
        itemCount = 0;
        itemImage.sprite = null;
        SetColor(0);

        
        text_Count.text = "x" + "0";
        go_CountImage.SetActive(false);

    }

    
}
