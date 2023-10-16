using UnityEngine.UI;
using UnityEngine;
using Photon.Pun;
using Photon.Pun.Demo.PunBasics;
using Cinemachine;
using TMPro;
using System.Collections;

public class PlayerMoverForMainWithPhoton : MonoBehaviourPunCallbacks
{
    // 발사체 프리팹
    //[SerializeField] GameObject laser;

    // 이동 속도
    [SerializeField] private float moveSpeed;
    [SerializeField] private float jumpPower;


    // 회전 속도
    //[SerializeField] float rotateSpeed = 10.0f;

    Animator anim;
    Rigidbody rigid;
    PlayerStatForPhoton playerStat;

    float h, v;

    bool fDown;
    //bool isFireReady = true;
    bool isSkillReady = true;
    bool skillDown;
    bool isBorder;
    bool shiftPressed;
    bool jDown;

    bool isJump;
    bool isRun;

   


    Weapon equipWeapon;

    Vector3 moveVec;


    float fireDelay;
    float skillDelay = 10f;
    public float skillCoolTime = 4f; // 스킬 쿨타임 정하는 곳 
    int swingCnt = 10;
    public Image skillFilter;
    public TextMeshProUGUI coolTimeCounter;
    private float currentCoolTime;
    void Awake()
    {
        anim = GetComponent<Animator>();
        equipWeapon = GetComponentInChildren<Weapon>();
        rigid = GetComponent<Rigidbody>();
        playerStat = GetComponent<PlayerStatForPhoton>();
        jumpPower = 20f;
        CinemachineVirtualCamera _mycam = GameObject.Find("Follow Cam").GetComponentInChildren<CinemachineVirtualCamera>();
        
            if (photonView.IsMine)
            {
                PlayerManager.LocalPlayerInstance = this.gameObject;
                _mycam.m_Priority = 100; //higher number means high priority to get 
            }
            else
            {
                CameraWork _cameraWork = this.gameObject.GetComponent<CameraWork>();
                Destroy(_cameraWork);
            }
        
    }

    void Update()
    {
        if (photonView.IsMine)
        {
            if (anim.GetCurrentAnimatorStateInfo(0).IsName("Idle"))
            {
                anim.ResetTrigger("sword_combo");
            }

            GetInput();

            Attack();
            Move();
            Jump();
            Rotate();

        }

    }

    // 이동 관련 함수를 짤 때는 Update보다 FixedUpdate가 더 효율이 좋다고 한다. 그래서 사용했다.
    void FixedUpdate()
    {
        FreezeRotation();
        StopToWall();
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

    void StopToWall()
    {
        Debug.DrawRay(transform.position, transform.forward * 5, Color.green);
        isBorder = Physics.Raycast(transform.position, transform.forward, 5, LayerMask.GetMask("BGobject"));
    }



    void Attack()
    {
        if (equipWeapon == null || isRun)
            return;

        //fireDelay += Time.deltaTime;
        //isFireReady = equipWeapon.rate < fireDelay;

        skillDelay += Time.deltaTime;
        isSkillReady = skillCoolTime <= skillDelay;

        if (fDown)
        {

            anim.SetTrigger("sword_combo");
            //equipWeapon.Use();
            //fireDelay = 0;
        }

        if (skillDown && isSkillReady && !fDown)
        {
            anim.SetTrigger("Whirlwind");
            swingCnt = 10;
            skillFilter.fillAmount = 1;
            //anim.SetBool("IsWhirlwind", true);
            //equipWeapon.UseSkill();
            skillDelay = 0;
        }


    }

    void Move()
    {
        if (fDown) return;
        if (anim.GetCurrentAnimatorStateInfo(0).IsName("Combo1")
            || anim.GetCurrentAnimatorStateInfo(0).IsName("Combo2")
            || anim.GetCurrentAnimatorStateInfo(0).IsName("Combo3"))
        {

            return;
        }

        moveSpeed = playerStat.CurrentStatus(2);
        Debug.Log("현재 속도는 .. " + moveSpeed);
        float local_moveSpeed = moveSpeed;

        moveVec = new Vector3(h, 0, v);


        //if (!isFireReady)
        //    moveVec = Vector3.zero;

        if (fDown)
            moveVec = Vector3.zero;

        if (shiftPressed)
        {
            local_moveSpeed = local_moveSpeed * 2.0f;
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
            anim.SetBool("isJump", true);
            //anim.SetTrigger("IsJump");


            isJump = true;
        }
    }


    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Terrain"))
        {

            //anim.SetTrigger("IsLand");
            anim.SetBool("isJump", false);


            isJump = false;
        }
    }

    public void Swing1_Start()
    {
        equipWeapon.SwingAnimation();
    }

    public void Swing1_End()
    {
        equipWeapon.SwingEndAnimation();
    }

    public void Whirlwind_Start()
    {
        equipWeapon.WhirlwindAnimation();
    }

    public void Whirlwind_End(int cnt)
    {
        swingCnt -= cnt;
        Debug.Log("swingCnt: " + swingCnt);
        if (swingCnt <= 0)
        {

            anim.SetBool("IsWhirlwind", false);
            StartCoroutine("Cooltime");
            currentCoolTime = skillCoolTime;
            coolTimeCounter.text = "" + currentCoolTime;
            StartCoroutine("CoolTimeCounter");
            skillDelay = 0;
            swingCnt = 10;
            equipWeapon.WhirlwindEndAnimation();

        }


    }

    IEnumerator Cooltime()
    {
        while (skillFilter.fillAmount > 0)
        {
            skillFilter.fillAmount -= 1 * Time.smoothDeltaTime / skillCoolTime;

            yield return null;
        }

        yield break;

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

}
