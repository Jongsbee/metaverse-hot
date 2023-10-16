using System.Collections;
using System.Collections.Generic;
using UnityEngine;



[CreateAssetMenu(fileName = "New Item", menuName = "New Item/item")]
public class Item : ScriptableObject
{
    public string itemName; // 아이템 이름
    public Sprite itemImage; // ������ �̹���
    public GameObject itemPrefab; // ������ ������
    public ItemType itemType; // ������ ����

    public string weaponType; // ���� ����

    public enum ItemType
    {
        Equipment,
        Used,
        ETC
    }




}
