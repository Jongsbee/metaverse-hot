using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//[RequireComponent(typeof(EnemyHP))]
public class HpBar : MonoBehaviour
{
    [SerializeField] GameObject bar_Prefab = null;
    [SerializeField] GameObject HitDamage = null;
    public GameObject TextPos;

    List<GameObject> m_objectList = new List<GameObject>();
    List<GameObject> m_hpBarList = new List<GameObject>();
    
    Camera m_cam = null;
    // EnemyHP enemyHP;

    void Start()
    {
        m_cam = Camera.main;

        GameObject[] t_objects = GameObject.FindGameObjectsWithTag("Enemy");
        for(int i = 0; i < t_objects.Length; i++)
        {
            m_objectList.Add(t_objects[i]);
            GameObject t_hpbar = Instantiate(bar_Prefab, t_objects[i].transform.position, Quaternion.identity, transform);
            m_hpBarList.Add(t_hpbar);
        }
    }

    public void GetDamage()
    {   
        GameObject dmgText = Instantiate(HitDamage, TextPos.transform.position, Quaternion.identity);
        dmgText.GetComponent<Text>().text = 3.ToString();
        Destroy(dmgText, 0.5f);
    }

    void Update()
    {
        if (m_objectList != null)
        {
            for (int i = 0; i < m_objectList.Count; i++)
            {

            m_hpBarList[i].transform.position = m_cam.WorldToScreenPoint(m_objectList[i].transform.position + new Vector3(0, 12f, 0));
            EnemyHP HP = m_objectList[i].GetComponent<EnemyHP>();
            float maxHP = HP.mobHP;
            float currentHP = HP.currentHP;
            GameObject GreenHp = m_hpBarList[i].transform.GetChild(0).gameObject;
            float is_death = GreenHp.GetComponent<Image>().fillAmount = currentHP / maxHP;
            if (is_death <= 0)
            {
                m_hpBarList[i].SetActive(false);
            }
        }


    }
    }
}
