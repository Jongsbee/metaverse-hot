using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Photon;
using Photon.Pun;
using Photon.Realtime;
using System.Runtime.InteropServices;

public class EventEnemyForPhoton : MonoBehaviourPunCallbacks
{

    // 몬스터 잡았을 시 상금
    [SerializeField] int goldReward = 200;


    public GMForPhoton gm;

    //애니메이션 부여
    Animator anim;
    // wavesystem 참조로 변경중...


    // 몬스터 속도
    public float speed = 0;
    bool deBuffedSpeed;
    public bool DeBuffedSpeed { get { return deBuffedSpeed; } }
    bool deBuffedArmor;
    public bool DeBuffedArmor { get { return deBuffedArmor; } }

    private EnemySpawnerForPhoton enemySpawner;

    // 몬스터 status
    public float mobHP;
    public float armor;
    private float currentHP;
    public float CurrentHP { get { return currentHP; } set { currentHP = value; } }


    private float currentSpeed;
    private float currentArmor;

    [SerializeField] GameObject greenHP;
    [SerializeField] GameObject damageText;
    [SerializeField] GameObject hpbar;

    // 피격이펙트
    ParticleSystem HitEffects;

    // 보스 status
    public bool isBoss = false;
    [SerializeField] public int skillIdx = 0;
    [SerializeField] public float coolDown = 3;
    [SerializeField] public int power = 50;
    [SerializeField] public int range = 100;

    // 스킬이펙트
    [SerializeField] ParticleSystem redCircle;

    [SerializeField] ParticleSystem greenCircle;
    [SerializeField] ParticleSystem Heel;
    [SerializeField] ParticleSystem groundCrush;

    private PhotonView PV;
    private int photonViewID;


    new Collider collider;

    

    private void Awake()
    {
        PV = GetComponent<PhotonView>();
        photonViewID = photonView.ViewID;
    }
    void Start()
    {
        // Bank 오브젝트를 하이어라키에서 찾아 bank에 할당
        gm = FindObjectOfType<GMForPhoton>();
        //anim = GetComponentInChildren<Animator>();

        currentHP = mobHP;
        currentSpeed = speed;
        currentArmor = armor;
        anim = this.transform.GetChild(0).GetComponent<Animator>();
        collider = gameObject.GetComponent<Collider>();
        isBoss = true;

        // 스킬 쓰고싶게한다면..
        //StartCoroutine(WarningSkill(skillIdx, coolDown));
    }

    // Update is called once per frame
    void Update()
    {
        greenHP.GetComponent<Image>().fillAmount = currentHP / mobHP;
    }



    public void Death(bool isKill = true)
    {

        if (isKill)
        {
            gm.Withdraw(goldReward, true);
            Destroy(gameObject, 0.8f);
        }

    }

    void OnParticleCollision(GameObject other)
    {
        if (other.name == "canon")
        {

        }

        //if (other.name == "character")
        //{
        //    Debug.Log("character hit");
        //    ProcessHit(2 - armor, null);
        //}
    }

    public void processHeel(float num)
    {
        if (currentHP + num > mobHP)
        {
            currentHP = mobHP;
        }
        else
        {
            currentHP += num;
        }
    }

    [PunRPC]
    public void ProcessHit(float Damage, ParticleSystem effect)
    {

        if (CurrentHP <= 0)
        {
            return;
        }



        if (effect)
        {
            HitEffects = effect;
            Instantiate(HitEffects, transform.position, Quaternion.identity);
        }

        if (isBoss && Damage > (float)(mobHP * 0.1))
        {
            Damage = (float)(mobHP * 0.1);
        }

        // 체력 감소
        if (Damage > currentArmor)
        {
            Damage -= currentArmor;
            if (isBoss && Damage > (float)(mobHP * 0.15))
            {
                Damage = (float)(mobHP * 0.15);
            }

            if (mobHP < Damage)
            {
                damageText.GetComponent<TMP_Text>().text = "OverKill!";
            }
            else
            {
                damageText.GetComponent<TMP_Text>().text = Damage.ToString();
            }
            currentHP -= Damage;
            if (currentHP > 0)
            {
                anim.SetTrigger("Hit");
            }
        }
        else
        {
            damageText.GetComponent<TMP_Text>().text = "Blocked!";
        }

        GameObject Hit = Instantiate(damageText, transform.position, Quaternion.identity, this.GetComponentInChildren<Canvas>().transform);
        Destroy(Hit, 0.6f);

        // 테스트용 체력감소
        // currentHP -= 1;


        if (currentHP <= 0)
        {
            collider.enabled = false;
            currentSpeed = 0;
            hpbar.SetActive(false);
            anim.SetTrigger("Death");
            Death();

        }


    }

    [PunRPC]
    public void ProcessHitWithNoParticle(float Damage)
    {
        if (CurrentHP <= 0)
        {
            return;
        }




        if (isBoss && Damage > (float)(mobHP * 0.1))
        {
            Damage = (float)(mobHP * 0.1);
        }

        // 체력 감소
        if (Damage > currentArmor)
        {
            Damage -= currentArmor;
            if (isBoss && Damage > (float)(mobHP * 0.15))
            {
                Damage = (float)(mobHP * 0.15);
            }

            if (mobHP < Damage)
            {
                damageText.GetComponent<TMP_Text>().text = "OverKill!";
            }
            else
            {
                damageText.GetComponent<TMP_Text>().text = Damage.ToString();
            }
            currentHP -= Damage;
            if (currentHP > 0)
            {
                anim.SetTrigger("Hit");
            }
        }
        else
        {
            damageText.GetComponent<TMP_Text>().text = "Blocked!";
        }

        GameObject Hit = Instantiate(damageText, transform.position, Quaternion.identity, this.GetComponentInChildren<Canvas>().transform);
        Destroy(Hit, 0.6f);

        // 테스트용 체력감소
        // currentHP -= 1;


        if (currentHP <= 0)
        {
            gm.EnemyKilled();
            //gm.MobCounter(false);
            collider.enabled = false;
            //Destroy(hpbar);
            hpbar.SetActive(false);
            //anim.SetBool("isDeath", true);
            anim.SetTrigger("Death");
            Death();
            
        }


    }

    [PunRPC]
    public void ProcessHitWithNoParticleNoDeath(float Damage)
    {
        if (CurrentHP <= 0)
        {
            return;
        }


        if (isBoss && Damage > (float)(mobHP * 0.1))
        {
            Damage = (float)(mobHP * 0.1);
        }

        // 체력 감소
        if (Damage > currentArmor)
        {
            Damage -= currentArmor;
            if (isBoss && Damage > (float)(mobHP * 0.15))
            {
                Damage = (float)(mobHP * 0.15);
            }

            if (mobHP < Damage)
            {
                damageText.GetComponent<TMP_Text>().text = "OverKill!";
            }
            else
            {
                damageText.GetComponent<TMP_Text>().text = Damage.ToString();
            }
            currentHP -= Damage;
            if (currentHP > 0)
            {
                anim.SetTrigger("Hit");
            }
        }
        else
        {
            damageText.GetComponent<TMP_Text>().text = "Blocked!";
        }

        GameObject Hit = Instantiate(damageText, transform.position, Quaternion.identity, this.GetComponentInChildren<Canvas>().transform);
        Destroy(Hit, 0.6f);

        // 테스트용 체력감소
        // currentHP -= 1;


        if (currentHP <= 0)
        {
            
            //gm.MobCounter(false);
            collider.enabled = false;
            currentSpeed = 0;
            //Destroy(hpbar);
            hpbar.SetActive(false);
            //anim.SetBool("isDeath", true);
            anim.SetTrigger("Death");
            Death();

        }


    }







    void OnTriggerEnter(Collider other)
    {

        if (other.tag == "Weapon")
        {
            if (PV.IsMine)
            {
                Weapon weapon = other.GetComponent<Weapon>();
                PlayerStatForPhoton playerStat = other.GetComponentInParent<PlayerStatForPhoton>();
                float attackDamage = weapon.damage + playerStat.CurrentStatus(1);

                ProcessHit(attackDamage, weapon.hitEffect);
                PV.RPC("ProcessHitWithNoParticleNoDeath", RpcTarget.Others, attackDamage);
            }
            else
            {
                Weapon weapon = other.GetComponent<Weapon>();
                PlayerStatForPhoton playerStat = other.GetComponentInParent<PlayerStatForPhoton>();
                float attackDamage = weapon.damage + playerStat.CurrentStatus(1);

                ProcessHit(attackDamage, weapon.hitEffect);
                PV.RPC("ProcessHitWithNoParticleNoDeath", RpcTarget.Others, attackDamage);
            }
        }
        else if (other.tag == "Arrow")
        {
            if (PV.IsMine)
            {
                Arrow arrow = other.GetComponent<Collider>().GetComponent<Arrow>();
                float attackDamage = arrow.damage;
                ProcessHit(attackDamage, arrow.hitEffect);
                PV.RPC("ProcessHitWithNoParticleNoDeath", RpcTarget.Others, attackDamage);
                GameObject arrow01 = other.transform.Find("Arrow01").gameObject;
                Destroy(other.gameObject);
            }
            else
            {
                Arrow arrow = other.GetComponent<Collider>().GetComponent<Arrow>();
                float attackDamage = arrow.damage;
                ProcessHit(attackDamage, arrow.hitEffect);
                PV.RPC("ProcessHitWithNoParticleNoDeath", RpcTarget.Others, attackDamage);
                GameObject arrow01 = other.transform.Find("Arrow01").gameObject;
                Destroy(other.gameObject);
            }

        }
    }



    [PunRPC]
    public void ProcessReduceSpeed(float reduce, ParticleSystem effect)
    {
        if (currentSpeed == speed)
        {
            HitEffects = effect;
            Instantiate(HitEffects, transform.position, Quaternion.identity);
            currentSpeed = speed * (1 - reduce);
            deBuffedSpeed = true;
            StartCoroutine(isDeBuffedSpeed());
        }
    }

    [PunRPC]
    public void ProcessReduceArmor(float reduce, ParticleSystem effect)
    {
        if (currentArmor == armor)
        {
            HitEffects = effect;
            Instantiate(HitEffects, transform.position, Quaternion.identity);
            currentArmor = armor * (1 - reduce);
            deBuffedArmor = true;
            StartCoroutine(isDeBuffedArmor());
        }
    }

    [PunRPC]
    public void ProcessReduceSpeedWithNoParticle(float reduce)
    {
        if (currentSpeed == speed)
        {

            currentSpeed = speed * (1 - reduce);
            deBuffedSpeed = true;
            StartCoroutine(isDeBuffedSpeed());
        }
    }

    [PunRPC]
    public void ProcessReduceArmorWithNoParticle(float reduce)
    {
        if (currentArmor == armor)
        {

            currentArmor = armor * (1 - reduce);
            deBuffedArmor = true;
            StartCoroutine(isDeBuffedArmor());
        }
    }



    IEnumerator isDeBuffedSpeed()
    {
        yield return new WaitForSeconds(4f);
        deBuffedSpeed = false;
        if (currentSpeed != 0)
        {
            currentSpeed = speed;
        }
    }

    IEnumerator isDeBuffedArmor()
    {
        yield return new WaitForSeconds(4f);
        deBuffedArmor = false;
        currentArmor = armor;
    }

    private IEnumerator Skill_KnockBack(float range = 100, float power = 50, float coolDown = 3)
    {
        yield return new WaitForSeconds(0.667f);
        anim.SetBool("isSkill", false);
        redCircle.Stop();
        currentSpeed = speed;
        GameObject[] players;
        players = GameObject.FindGameObjectsWithTag("Player");
        foreach (GameObject player in players)
        {
            float d = Vector3.Distance(transform.position, player.transform.position);

            if (d < range)
            {
                Vector3 dir = (player.transform.position - transform.position).normalized;
                player.GetComponent<Rigidbody>().velocity = new Vector3(dir.x * power, power / 2, dir.z * power);
                player.GetComponent<PlayerStatForPhoton>().StopP(power);
            }

        }
        //Destroy(skillRange, 0.5f);
        yield return new WaitForSeconds(coolDown);
    }
    private IEnumerator Skill_Heeling(float range = 100, float power = 1000, float coolDown = 3)
    {
        yield return new WaitForSeconds(0.667f);
        anim.SetBool("isSkill", false);
        GameObject[] enemies;
        enemies = GameObject.FindGameObjectsWithTag("Enemy");
        foreach (GameObject enemy in enemies)
        {

            float d = Vector3.Distance(transform.position, enemy.transform.position);
            if (d < range)
            {
                if (enemy.gameObject.transform.childCount == 4)
                {
                    ParticleSystem heelingEffect = Instantiate(Heel, enemy.transform);
                    heelingEffect.transform.localScale = enemy.transform.GetChild(0).transform.localScale;
                    heelingEffect.Play();
                }
                enemy.GetComponent<Enemy>().processHeel(power);
            }

        }
        yield return new WaitForSeconds(coolDown);
    }
    private IEnumerator Skill_Stun(float range = 100, float power = 50, float coolDown = 3)
    {
        yield return new WaitForSeconds(0.667f);
        anim.SetBool("isSkill", false);
        redCircle.Stop();
        currentSpeed = speed;
        GameObject[] players;
       
        players = GameObject.FindGameObjectsWithTag("Player");
       
        foreach (GameObject player in players)
        {
            float d = Vector3.Distance(transform.position, player.transform.position);

            if (d < range)
            {
                
                player.transform.localScale = new Vector3(player.transform.localScale.x, 1, player.transform.localScale.z);
                // 움직임 멈추는 로직 발현
                player.GetComponent<PlayerStatForPhoton>().StopP(power);
                StartCoroutine(ResetScale(player, power));

            }

        }
        yield return new WaitForSeconds(coolDown);
    }

    private IEnumerator WarningSkill(int skillIdx, float coolDown)
    {
        while (true)
        {

            /*
            0 : 넉백
            1 : 힐링
            2 : 스턴(짜부)
            */
            if (skillIdx == 0)
            {
                currentSpeed = 0;
                redCircle.transform.localScale = new Vector3(range / 2, 1, range / 2);
                redCircle.Play();
                yield return new WaitForSeconds(3f);
                anim.SetBool("isSkill", true);
                StartCoroutine(Skill_KnockBack(range, power, coolDown));
            }
            else if (skillIdx == 1)
            {
                greenCircle.transform.localScale = new Vector3(range / 2, 1, range / 2);
                greenCircle.Play();
                //yield return new WaitForSeconds(3f);
                anim.SetBool("isSkill", true);
                StartCoroutine(Skill_Heeling(range, power, coolDown));
            }
            else if (skillIdx == 2)
            {
                currentSpeed = 0;
                redCircle.transform.localScale = new Vector3(range / 2, 1, range / 2);
                redCircle.Play();
                yield return new WaitForSeconds(3f);
                anim.SetBool("isSkill", true);
                StartCoroutine(Skill_Stun(range, power, coolDown));
            }
            yield return new WaitForSeconds(coolDown);
        }
    }
    IEnumerator ResetScale(GameObject tar, float power)
    {
        yield return new WaitForSeconds(power / 25);
        tar.transform.localScale = new Vector3(tar.transform.localScale.x, tar.transform.localScale.x, tar.transform.localScale.x);
    }
}
