using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyWavesData : MonoBehaviour
{
    public EnemyWavesData(float spawnTime, int maxEnemyCount, int mobHP, string pathString, int lifePenalty, float speed, float armor) {
        this.spawnTime = spawnTime;
        this.maxEnemyCount = maxEnemyCount;
        this.mobHP = mobHP;
        this.pathString = pathString;
        this.lifePenalty = lifePenalty;
        this.speed = speed;
        this.armor = armor;
    }

    [SerializeField] public float spawnTime;
    [SerializeField]  public int maxEnemyCount;
    public int mobHP;
    public string pathString;
    public int lifePenalty;
    public float speed;
    public float armor;
}
