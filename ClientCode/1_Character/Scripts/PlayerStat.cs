using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStat : MonoBehaviour
{
    GameObject totalManager;
    ItemController itemController;

    [SerializeField] private float[] playerDamage;
    [SerializeField] private float[] playerMoveSpeed;
    [SerializeField] private float[] playerAttackRange;

    [SerializeField] private float[] weaponDamage;



    private float[] statusArray;

    private void Awake()
    {
        playerDamage = new float[] {10f, 5f };
        playerMoveSpeed = new float[] { 20f, 30f };
        playerAttackRange = new float[] { 10f, 10f };

        weaponDamage = new float[] { };

        totalManager = GameObject.Find("Manager").gameObject;
        itemController = totalManager.transform.Find("ItemManager").GetComponent<ItemController>();

        statusArray = new float[101];
        statusArray[1] = playerDamage[0];
        statusArray[2] = playerMoveSpeed[0];
    }
    private void Start()
    {
        // 시작하면 직업에 따라서 갖고온다. 지금은 임시
        
        Debug.Log("statusArray : " + statusArray[2]);
    }

    public float CurrentStatus(int order)
    {

        return statusArray[order] * itemController.getTotalStack(order);

    }



}
