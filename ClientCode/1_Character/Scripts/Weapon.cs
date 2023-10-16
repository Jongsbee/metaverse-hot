using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{

    public enum Type { Melee, Range,Hammer};
    public Type type;

    public int damage;
    public float rate;
    public BoxCollider meleeArea;
    public TrailRenderer trailEffect;
    Animator anim; // Melee 일 경우: 검, Range일 경우: 화살
    Animator anim2;// Range일 경우: 활
    GameObject bow;
    GameObject arrows;
    public ParticleSystem hitEffect;
    public Transform arrowPos;
    public Transform[] multiShotPos;
    public GameObject arrow;

    // 적 피격음
    AudioSource audioSource;
    public AudioClip audiohit;


    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        if(type == Type.Range)
        {
            arrows = GameObject.Find("Arrows");
            anim = arrows.GetComponent<Animator>();
            bow = GameObject.Find("Bows");
            anim2 = bow.GetComponent<Animator>();
        }else if(type == Type.Melee)
        {
            anim = GetComponentInParent<Animator>();
            meleeArea = GetComponent<BoxCollider>();
        }else if(type == Type.Hammer)
        {
            anim = GetComponentInParent<Animator>();
            meleeArea = GetComponent<BoxCollider>();
        }
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Enemy")
        {

            //audioSource.clip = audiohit;
            audioSource.PlayOneShot(audiohit);
        }
    }


    public void Use()
    {
        if (type == Type.Melee)
        {
        }

        if(type == Type.Range)
        {
            
        }
            
    }

    public void UseSkill()
    {
        if(type == Type.Melee)
        {
            anim.SetBool("IsWhirlwind", true);
        }else if(type == Type.Range)
        {
            
        }else if(type == Type.Hammer)
        {
            anim.SetBool("IsSkill", true);
        }
    }

    // Use() ���η�ƾ -> Swing() �����ƾ -> Use() ���η�ƾ
    // Use() ���η�ƾ + Swing() �ڷ�ƾ

    IEnumerator Shot()
    {

        //#1 총알 발사

        GameObject instantArrow = Instantiate(arrow, arrowPos.position, arrowPos.rotation);
        Arrow arrowScript = instantArrow.GetComponent<Arrow>();
        arrowScript.player = this.gameObject;
        yield return new WaitForSeconds(3f); // 3초 뒤에 자동으로 파괴됨

        Destroy(instantArrow);

    }

    IEnumerator MultiShot()
    {
        GameObject[] multiShotArrow = new GameObject[7];
        Arrow arrowScript;
       
        for (int pos = 0; pos < multiShotPos.Length; pos++)
        {

            multiShotArrow[pos] = Instantiate(arrow, multiShotPos[pos].position, multiShotPos[pos].rotation);
            arrowScript = multiShotArrow[pos].GetComponent<Arrow>();
            arrowScript.player = this.gameObject;
           

        }

        yield return new WaitForSeconds(1f);

        for (int pos = 0; pos < multiShotPos.Length; pos++)
        {
            Destroy(multiShotArrow[pos]); 
        }

    }


    // 궁수 애니메이션
    public void fireAnimation()
    {
        StartCoroutine("Shot");
        anim.SetBool("IsFire", true);
        anim2.SetBool("IsFire", true);
    }

    public void fireEndAnimation()
    {
        anim.SetBool("IsFire", false);
        anim2.SetBool("IsFire", false);
    }


    public void MultiShotAnim_Start()
    {
        StartCoroutine("MultiShot");
    }


    


    // 소드맨 애니메이션
    public void SwingAnimation()
    {
        meleeArea.enabled = true;
        trailEffect.enabled = true;

    }

    public void SwingEndAnimation()
    {
        
        meleeArea.enabled = false;
        trailEffect.enabled = false;
    }

    public void WhirlwindAnimation()
    {
        meleeArea.enabled = true;
        trailEffect.enabled = true;
    }

    public void WhirlwindEndAnimation()
    {
        meleeArea.enabled = false;
        trailEffect.enabled = false;
        anim.SetBool("IsWhirlwind", false);
    }

    // 해머맨 애니메이션

    //해머맨 콤보 공격
    public void HammerComboAnimation()
    {
        meleeArea.enabled = true;
        trailEffect.enabled = true;

    }

    public void HammerComboEndAnimation()
    {
        meleeArea.enabled = false;
        trailEffect.enabled = false;
    }


    //해머맨 스킬
    public void HammerSkillStartAnimation()
    {
        meleeArea.enabled = true;
        trailEffect.enabled = true;
    }

    public void HammerSkillEndAnimation()
    {
        meleeArea.enabled = false;
        trailEffect.enabled = false;
    }

    public void HammerSkillFinalAnimation()
    {
        meleeArea.enabled = true;
        trailEffect.enabled = true;
        trailEffect.startWidth = 10;
        meleeArea.size = meleeArea.size * 5;
    }

    


    public void HammerSkillFinalEndAnimation()
    {
        trailEffect.startWidth = 1;
        meleeArea.size = meleeArea.size / 5;
        meleeArea.enabled = false;
        trailEffect.enabled = false;
        anim.SetBool("IsSkill", false);
    }



}
