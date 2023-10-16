using UnityEngine.UI;
using UnityEngine;
using Photon.Pun;
using Photon.Pun.Demo.PunBasics;
using Cinemachine;
using TMPro;
using System.Collections;

public class SwordPlayerMoverForPhoton : MonoBehaviourPunCallbacks, IPlayer
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

    GMForPhoton gm;

    float h, v;

    bool fDown;
    //bool isFireReady = true;
    bool isSkillReady = true;
    bool skillDown;
    bool isBorder;
    bool shiftPressed;
    bool jDown;
    bool resetDown;

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
    private bool _isFocused = true;
    // 오디오
    public AudioSource audioSource;
    public AudioClip audioSwing1;
    public AudioClip audioSwing2;
    public AudioClip audioWhirlwind;
    public AudioClip audioLeftWalk;
    public AudioClip audioRightWalk;
    public AudioClip audioLeftRun;
    public AudioClip audioRightRun;
    Transform parent;

    // 영준 : 플레이어 마커 변수
    private GameObject myMarker;
    private GameObject otherMarker;

    public PhotonView GetPhotonView()
    {
        return photonView;
    }
    public void SetMovementEnabled(bool enabled)
    {
        this.enabled = enabled;
    }
   
    public GameObject GetCharacterChatPanel()
    {
        return parent.Find("SwordPlayerBody/ChatBalloonCanvas/TextBox").gameObject;
    }

    public Transform GetTransform()
    {
        return this.transform;
    }
    void Start()
    {
        //25 
        gm = FindObjectOfType<GMForPhoton>();
        parent = transform.parent;
        Transform playerCanvas = parent.GetChild(3);
        anim = GetComponent<Animator>();
        equipWeapon = GetComponentInChildren<Weapon>();
        rigid = GetComponent<Rigidbody>();
        playerStat = GetComponent<PlayerStatForPhoton>();
        jumpPower = 20f;
        // 영준 : 마커 할당
        myMarker = parent.Find("SwordPlayerBody/PlayerMarker").gameObject;
        otherMarker = parent.Find("SwordPlayerBody/OtherPlayerMarker").gameObject;
        TextMeshProUGUI nickText = parent.Find("SwordPlayerBody/NickNameCanvas/NickBox/Text_Nick").GetComponent<TextMeshProUGUI>();
        nickText.text = photonView.Owner.NickName;

        if (!photonView.IsMine)
        {
            //AudioListener audioListener = GetComponent<AudioListener>();
            //audioListener.enabled= false;
            Transform playerCamera = parent.GetChild(1);
            CinemachineVirtualCamera personalCamera = playerCamera.GetComponent<CinemachineVirtualCamera>();
            personalCamera.enabled = false;
            Transform playerCameraFollowScript = parent.GetChild(2);
            CameraFollow personalScript = playerCameraFollowScript.GetComponent<CameraFollow>();
            personalScript.enabled = false;
            Canvas canvas = playerCanvas.GetComponent<Canvas>();
            canvas.enabled = false;
            AudioListener audioListener = playerCameraFollowScript.GetComponent<AudioListener>();
            audioListener.enabled = false;
            GetCharacterChatPanel().SetActive(false);
            // 영준 : 다른 플레이어 마커 선택
            myMarker.SetActive(false);
        }
        else
        {
            ChatManager chatManager = FindObjectOfType<ChatManager>();
            chatManager.RegisterPlayer(this);
          
            // 영준 : 플레이어 마커 선택
            otherMarker.SetActive(false);
        }

     
    }

    void Update()
    {
        if (_isFocused)
        {
            if (!photonView.IsMine)
            {
                return;
            }

            if (anim.GetCurrentAnimatorStateInfo(0).IsName("Idle"))
            {
                anim.ResetTrigger("sword_combo");
            }

            GetInput();

            Attack();
            Move();
            Jump();
            Rotate();
            ResetPos();

        }




    }
    void OnApplicationFocus(bool focus)
    {
        _isFocused = focus;
    }
    // 이동 관련 함수를 짤 때는 Update보다 FixedUpdate가 더 효율이 좋다고 한다. 그래서 사용했다.
    void FixedUpdate()
    {
        if (!photonView.IsMine)
        {
            return;
        }

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
        resetDown = Input.GetKey(KeyCode.R);


    }
    void ResetPos()
    {
        if (resetDown)
        {
            transform.position = gm.nowStage.transform.position + new Vector3(0, 7, 0);
        }
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
        audioSource.PlayOneShot(audioSwing1);
        equipWeapon.SwingAnimation();
    }

    public void Swing1_End()
    {
        equipWeapon.SwingEndAnimation();
    }
    public void Swing2_Start()
    {
        audioSource.PlayOneShot(audioSwing2);
        equipWeapon.SwingAnimation();
    }


    public void Swing2_End()
    {
        equipWeapon.SwingEndAnimation();
    }


    public void Whirlwind_Start()
    {
        //audioSource.clip = audioWhirlwind;
        audioSource.PlayOneShot(audioWhirlwind);
        equipWeapon.WhirlwindAnimation();
    }

    public void Whirlwind_End(int cnt)
    {
        swingCnt -= cnt;
        if (swingCnt <= 0)
        {
            ////
            StartCoroutine("Cooltime");
            currentCoolTime = skillCoolTime;
            coolTimeCounter.text = "" + currentCoolTime;
            StartCoroutine("CoolTimeCounter");
            skillDelay = 0;
            swingCnt = 10;
            equipWeapon.WhirlwindEndAnimation();

        }


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
        equipWeapon.SwingEndAnimation();
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
