using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpecialEnemyWaveData : MonoBehaviour
{
    public SpecialEnemyWaveData(float spawnTime,
        int maxEnemyCount,
        int mobHP,
        string pathString,
        int lifePenalty,
        float speed,
        float armor, int skillIdx, float coolDown, int range, int power)
    {
        this.spawnTime = spawnTime;
        this.maxEnemyCount = maxEnemyCount;
        this.mobHP = mobHP;
        this.pathString = pathString;
        this.lifePenalty = lifePenalty;
        this.speed = speed;
        this.armor = armor;
        this.skillIdx = skillIdx;
        this.coolDown = coolDown;
        this.range = range;
        this.power = power;
    }

    public float spawnTime;
    public int maxEnemyCount;
    public int mobHP;
    public string pathString;
    public int lifePenalty;
    public float speed;
    public float armor;

    /* 스킬
    0 : 넉백
    1 : 힐링
    2 : 스턴(짜부)
    */
    public int skillIdx;
    public float coolDown;
    public int range;
    public int power;
    
}
