using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class TEST2 : MonoBehaviour
{
    float currentHP;
    float maxHP;
    public Transform Mob;
    void Start()
    {

    }

    void Update()
    {
        transform.position = Mob.position + new Vector3(0, Mob.GetChild(0).localScale.y *2 /3, 0);
        transform.rotation = Quaternion.Euler(90f, 0, 0);
    }
}
