using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class Missile : MonoBehaviour
{
    Rigidbody rigidBody = null;
    [SerializeField] private Transform thisTarget = null;
    Enemy targetEnemy;
    float thisDamage;

    [SerializeField]  private float thisMissileSpeed = 0f;
    float currentSpeed = 0f;
    float thisMissileWaitSecond = 0f;
    [SerializeField] ParticleSystem[] thisEffect = null;
    public ParticleSystem missileHitEffect;
    AudioClip missileHitSound;

    AudioSource audioSource;

    TargetLocator missileTurret;
    bool fire = false;
    int thisDeBuff;

    public void SetUp(Transform attackTarget, float damage, float missileSpeed, float missileWaitSecond, int deBuff, AudioClip hitSound)
    {
        this.thisTarget = attackTarget;
        this.thisDamage = damage;
        this.thisMissileSpeed = missileSpeed;
        this.thisMissileWaitSecond = missileWaitSecond;
        this.thisDeBuff = deBuff;
        this.missileHitSound = hitSound;
    }

    IEnumerator LaunchDelay()
    {
        yield return new WaitUntil(() => rigidBody.velocity.y < 0f);
        yield return new WaitForSeconds(thisMissileWaitSecond);
        fire = Aimweapon();

        yield return new WaitForSeconds(2f);
        Destroy(gameObject);

    }

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        targetEnemy = thisTarget.GetComponent<Enemy>();
        rigidBody = GetComponent<Rigidbody>();
        if (thisEffect != null)
        {
            foreach (ParticleSystem effect in thisEffect)
            {
                effect.Play();
            }
        }
        StartCoroutine(LaunchDelay());
    }

    void Update()
    {   
        if (thisTarget && targetEnemy.CurrentHP > 0)
        {
            if (currentSpeed <= thisMissileSpeed)   
                currentSpeed += thisMissileSpeed * Time.deltaTime;

            transform.position += transform.up * currentSpeed * Time.deltaTime;

            Vector3 t_dir = (thisTarget.position - transform.position).normalized;
            transform.up = Vector3.Lerp(transform.up, t_dir, 0.25f);

        }
        else
        {
            Instantiate(missileHitEffect, transform.position, Quaternion.identity);
            Destroy(gameObject);
        }

    }

    bool Aimweapon()
    {
        if (thisTarget)
        {
            
            float targetDistance = Vector3.Distance(transform.position, thisTarget.position);

            missileTurret = FindObjectOfType<TargetLocator>();
            if (targetDistance < missileTurret.attackRange)
            {
                return true;
            }
            return false;
        }
        return false;

    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Enemy")
        {
            Destroy(gameObject);
            Enemy enemy = collision.gameObject.GetComponent<Enemy>();
            if (thisDeBuff > 0)
            {
                DeBuffAttack(enemy);
            }   
            else
            {
                enemy.ProcessHit(thisDamage, missileHitEffect);
            }
        }
    }

    void DeBuffAttack(Enemy enemy)
    {
        switch (thisDeBuff)
        {
            case 1:
                enemy.ProcessReduceSpeed(thisDamage, missileHitEffect);
                break;

            case 2:
                //enemy.ProcessReduceArmor(towerDamage);
                break;
        }
    }

}
