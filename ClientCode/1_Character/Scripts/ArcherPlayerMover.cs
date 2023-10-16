using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ArcherPlayerMover : MonoBehaviour
{
    [SerializeField] Camera characterCam;

    // 발사체 프리팹
    //[SerializeField] GameObject laser;

    // 이동 속도
    [SerializeField] private float moveSpeed;
    [SerializeField] private float jumpPower;


    public Image skillFilter;

    private PlayerStat playerStat;
   

    
    // 회전 속도
    //[SerializeField] float rotateSpeed = 10.0f;

    Animator anim;
    Rigidbody rigid;

    float h, v;


    bool fDown;
    bool isFireReady = true;
    bool isSkillReady = true;
    bool skillDown;
    bool isBorder;
    bool shiftPressed;
    bool jDown;

    bool isJump;
    bool isRun;

    Weapon equipWeapon;

    Vector3 moveVec;


    float fireDelay = 1000f; // 이 값은 큰값으로만 정하면 된다.
                              // 공격속도는 Weapon에서 조절  
                                
    float skillDelay = 10f;
    int shotCnt = 10;
    public float skillCoolTime = 4f; // 스킬 쿨타임 정하는 곳
    public TextMeshProUGUI coolTimeCounter;

    private float currentCoolTime;


    // 오디오
    AudioSource audioSource;
    public AudioClip audioShoot;
    public AudioClip audioArrowSkill;
    public AudioClip audioLeftWalk;
    public AudioClip audioRightWalk;
    public AudioClip audioLeftRun;
    public AudioClip audioRightRun;

    void Start()
    {
        characterCam = Camera.main;

        anim = GetComponent<Animator>();
        equipWeapon = GetComponentInChildren<Weapon>();
        rigid = GetComponent<Rigidbody>();
        playerStat = GameObject.Find("Player").GetComponent<PlayerStat>();
        jumpPower = 20f;
        skillFilter.fillAmount = 0;
        audioSource = GetComponent<AudioSource>();


    }

    void Update()
    {
        //MouseTracking();
        GetInput();
        Attack();
        Move();
        Jump();
        Rotate();
    }

    // 이동 관련 함수를 짤 때는 Update보다 FixedUpdate가 더 효율이 좋다고 한다. 그래서 사용했다.
    void FixedUpdate()
    {
        FreezeRotation();
        StopToWallI();
    }

    void GetInput()
    {
        v = Input.GetAxis("Vertical"); // 앞 뒤 움직임
        h = Input.GetAxis("Horizontal"); // 좌 우 회전
        shiftPressed = Input.GetKey(KeyCode.LeftShift);
        fDown = Input.GetButtonDown("Fire1");
        jDown = Input.GetButtonDown("Jump");
        skillDown = Input.GetButtonDown("Fire2");
        
    }

    void FreezeRotation()
    {
        rigid.angularVelocity = Vector3.zero;
    }

    void StopToWallI()
    {
        Debug.DrawRay(transform.position, transform.forward * 5, Color.green);
        isBorder = Physics.Raycast(transform.position, transform.forward, 5, LayerMask.GetMask("BGobject"));
    }



    void Attack()
    {
        if (equipWeapon == null || isRun || anim.GetBool("IsMultiShot"))
            return;
        
        fireDelay += Time.deltaTime;
        isFireReady = equipWeapon.rate < fireDelay;

        skillDelay += Time.deltaTime;
        isSkillReady = skillCoolTime <= skillDelay;

        if (fDown && isFireReady)
        {
            //MouseTracking();
            anim.SetBool("IsFire", true);
            if(equipWeapon.type == Weapon.Type.Range)
            {
                equipWeapon.Use();
            }
            
            fireDelay = 0;
        }

        if (skillDown && isSkillReady && !fDown)
        {
            shotCnt = 10;
            skillFilter.fillAmount = 1;
            anim.SetTrigger("MultiShot");
            anim.SetBool("IsMultiShot", true);
            equipWeapon.UseSkill();
        }

    }

    void Move()
    {

        if (fDown) return;


        //anim.SetBool("IsFire", false);

        moveSpeed = playerStat.CurrentStatus(2);
        float local_moveSpeed = moveSpeed;

        moveVec = new Vector3(h, 0, v);


        //if (!isFireReady)
        //    moveVec = Vector3.zero;

        if (fDown)
            moveVec = Vector3.zero;

        if (shiftPressed)
        {
            local_moveSpeed = local_moveSpeed * 4.0f;
            isRun = true;
        }
        else
        {
            isRun = false;
        }
        

        if (!isBorder)
            transform.position += moveVec * local_moveSpeed * Time.deltaTime;

            anim.SetBool("IsMove", moveVec != Vector3.zero);
            anim.SetBool("IsRun", isRun);
        
    }

    void Rotate()
    {

        transform.LookAt(transform.position + moveVec);

    }

    void Jump()
    {
        if (jDown && !isJump)
        {
            
            rigid.AddForce(Vector3.up * jumpPower, ForceMode.Impulse);

            anim.SetBool("IsFire", false);



            isJump = true;
        }
    }


    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Terrain"))
        {
            isJump = false;
        }
    }

    public void ArrowFire()
    {
        audioSource.PlayOneShot(audioShoot);
        equipWeapon.fireAnimation();
        
    }

    public void AnimEvent_FireEnd()
    {
        anim.SetBool("IsFire", false);
        equipWeapon.fireEndAnimation();
    }



    public void MultiShotAnim_End(int cnt)
    {


        shotCnt -= cnt;
        if(shotCnt <= 0)
        {
            
            anim.SetBool("IsMultiShot", false);
            StartCoroutine("Cooltime");
            currentCoolTime = skillCoolTime;
            coolTimeCounter.text = "" + currentCoolTime;
            StartCoroutine("CoolTimeCounter");
            skillDelay = 0;
            shotCnt = 10;
            
        }
    }



    public void MultiShotAnim_Start()
    {
        //audioSource.clip = audioArrowSkill;
        audioSource.PlayOneShot(audioArrowSkill);
    }


    IEnumerator Cooltime()
    {
        while(skillFilter.fillAmount > 0)
        {
            skillFilter.fillAmount -= 1 * Time.smoothDeltaTime / skillCoolTime;

            yield return null;
        }

        yield break;

    }

    public void Walk_Leftfootstep()
    {
        audioSource.clip = audioLeftWalk;
        audioSource.Play();
    }

    public void Walk_Rightfootstep()
    {
        audioSource.clip = audioRightWalk;
        audioSource.Play();
    }

    public void Running_Leftfootstep()
    {
        audioSource.clip = audioLeftRun;
        audioSource.Play();
    }

    public void Running_Rightfootstep()
    {
        audioSource.clip = audioRightRun;
        audioSource.Play();
    }

    public void Idle_Start()
    {

        
    }


    IEnumerator CoolTimeCounter()
    {
        while (currentCoolTime > 0)
        {
            yield return new WaitForSeconds(1.0f);
            currentCoolTime -= 1.0f;
            coolTimeCounter.text = "" + currentCoolTime;
        }

        coolTimeCounter.text = "";
        yield break;
    }
    
    void MouseTracking()
    {
        Ray ray = characterCam.ScreenPointToRay(Input.mousePosition);
        Debug.Log((ray.direction - ray.origin).normalized);
        Vector3 test = ray.direction - ray.origin;
        Vector3 testXZ = new Vector3(test.x * 100, transform.position.y, test.z * 100);
        transform.rotation = Quaternion.LookRotation(testXZ);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, 1000000000f))
        {
            Vector3 targetPos = hit.point;
            Debug.Log(targetPos);
            Vector3 dir = targetPos - transform.localPosition;
            Vector3 dirXZ = new Vector3(dir.x, transform.localPosition.y, dir.z);
            Quaternion tarRotation = Quaternion.LookRotation(dirXZ);
            rigid.rotation = Quaternion.RotateTowards(transform.localRotation, tarRotation, 550.0f * Time.deltaTime);

        }

    }
}
