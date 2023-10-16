using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill : MonoBehaviour
{
    Enemy enemy;
    void Start()
    {
        enemy = gameObject.GetComponentInParent<Enemy>();
    }
    public void Good()
    {
        enemy.Skill();
    }
}
