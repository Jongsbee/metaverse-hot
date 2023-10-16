using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStatForPhoton : MonoBehaviour
{
    GameObject totalManager;
    ItemControllerForPhoton itemController;

    [SerializeField] public float playerDamage;
    [SerializeField] public float playerMoveSpeed;
    [SerializeField] public float playerAttackRange;

    private float[] defaultDamage;
    private float[] defaultMoveSpeed;
    public int charIndex;

    private float[] statusArray;

    private void Awake()
    {
        defaultDamage = new float[] {12f, 8f, 20f };
        defaultMoveSpeed = new float[] { 18f, 20f, 15f };


        totalManager = GameObject.Find("Manager").gameObject;
        itemController = totalManager.transform.Find("ItemManager").GetComponent<ItemControllerForPhoton>();

        //playerDamage = 10f;
        //playerMoveSpeed = 20f;

        statusArray = new float[101];
        //statusArray[1] = playerDamage;
        //statusArray[2] = playerMoveSpeed;
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
    public void StopP(float power)
    {
        StartCoroutine(StopPlayer(power));
    }
    private IEnumerator StopPlayer(float power)
    {
        statusArray[2] = 0;
        yield return new WaitForSeconds(power / 25);
        statusArray[2] = defaultMoveSpeed[charIndex];
    }

    public void setUserDamage()
    {
        statusArray[1] = defaultDamage[charIndex];
        statusArray[2] = defaultMoveSpeed[charIndex];
    }
}