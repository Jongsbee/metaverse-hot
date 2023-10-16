using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Enemy))]
public class EnemyHP : MonoBehaviour
{
    // 몬스터 status
    public float mobHP;
    public float armor = 1;
    public bool isBoss;
    public int lifePenalty;
    public float speed;

    [Tooltip("리스폰 시 상승할 체력지수")]
    // [SerializeField] int difficulty = 1;
    [SerializeField] private GameObject HitDamage;
    [SerializeField] private GameObject TextPos;

    [SerializeField] GameObject bar_Prefab = null;

    void OnEnable()
    {
        // 체력 설정
        currentHP = mobHP;
    }
    [SerializeField] GameObject target = null;
    GameObject hpbar;

    private EnemySpawner enemySpawner;
    
    // 웨이브 정보
    // Bank waveInfo = GameObject.Find("Bank").GetComponent<Bank>();
    public float currentHP;

    Enemy enemy;
    [SerializeField] Camera m_cam;
    [SerializeField] GameObject UI;

    void Start()
    {
        m_cam = Camera.main;
        currentHP = mobHP;
        //hpbar = Instantiate(bar_Prefab, target.transform.position, Quaternion.identity, target.GetComponentInChildren<Canvas>().transform);
        hpbar = Instantiate(bar_Prefab, target.transform.position, Quaternion.identity, m_cam.GetComponentInChildren<Canvas>().transform);
    }

    void OnParticleCollision(GameObject other)
    {
        if (other.name == "canon")
        {

        }

        if (other.name == "character")
        {
            // Debug.Log("character hit");
            ProcessHit(2 - armor);
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        Debug.Log(collision.gameObject.name);

        }
    public void ProcessHit(float Damage)
    {
        // 체력 감소
        currentHP -= Damage;

        // 테스트용 체력 감소


        if (currentHP <= 0)
        {
            Destroy(hpbar);
            Destroy(target);

            // enemy.Death();

        }
    }
    void Update()
    {
        //hpbar.transform.position = m_cam.WorldToScreenPoint(target.transform.position + new Vector3(0, 5f, 0));
        hpbar.transform.position = new Vector3(0, 5f, 0);
        GameObject GreenHP = hpbar.transform.GetChild(0).gameObject;
        GreenHP.GetComponent<Image>().fillAmount = currentHP / mobHP;
    }
}