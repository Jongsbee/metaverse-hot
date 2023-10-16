using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Photon.Pun;
using Cinemachine;

public class HammerPlayerMover : MonoBehaviourPun, IPlayer
{
    // 이동 속도
    [SerializeField] private float moveSpeed;
    [SerializeField] private float jumpPower;

    GMForPhoton gm;


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
    bool resetDown;

    bool isJump;
    bool isRun;

    Weapon []equipWeapon;

    Vector3 moveVec;


    float fireDelay;
    float skillDelay = 10f;
    public float skillCoolTime; // 스킬 쿨타임 정하는 곳 
    int swingCnt = 10;
    public Image skillFilter;
    public TextMeshProUGUI coolTimeCounter;
    private float currentCoolTime;

    // 오디오
    public AudioSource audioSource;
    public AudioClip audioSwingLeft;
    public AudioClip audioSwingRight;
    public AudioClip audioWhirlwind;
    public AudioClip audioCrash;
    public AudioClip audioLeftWalk;
    public AudioClip audioRightWalk;
    public AudioClip audioLeftRun;
    public AudioClip audioRightRun;
    public AudioClip audioSkillLeft;
    public AudioClip audioSkillRight;
    public AudioClip audioWhirlwind2;
    public AudioClip audiojump;
    public GameObject crashEffect;
    public Transform crashPos;

    public Transform parent;

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
        return parent.Find("HammerPlayerBody/ChatBalloonCanvas/TextBox").gameObject;
    }

    public Transform GetTransform()
    {
        return this.transform;
    }
    void Start()
    {
        gm = FindObjectOfType<GMForPhoton>();
        parent = transform.parent;
        Transform playerCanvas = parent.GetChild(3);
        anim = GetComponent<Animator>();
        equipWeapon = GetComponentsInChildren<Weapon>();
        rigid = GetComponent<Rigidbody>();
        playerStat = GetComponent<PlayerStatForPhoton>();
        jumpPower = 20f;
        Debug.Log(equipWeapon[0]);
        Debug.Log(equipWeapon[1]);
        // 영준 : 마커 할당
        myMarker = parent.Find("HammerPlayerBody/PlayerMarker").gameObject;
        otherMarker = parent.Find("HammerPlayerBody/OtherPlayerMarker").gameObject;
        TextMeshProUGUI nickText = parent.Find("HammerPlayerBody/NickNameCanvas/NickBox/Text_Nick").GetComponent<TextMeshProUGUI>();
        nickText.text = photonView.Owner.NickName;

        if (!photonView.IsMine)
        {
            //AudioListener audioListener = GetComponent<AudioListener>();
            //audioListener.enabled = false;
            Transform playerCamera = parent.GetChild(1);
            CinemachineVirtualCamera personalCamera = playerCamera.GetComponent<CinemachineVirtualCamera>();
            personalCamera.enabled = false;
            Transform playerCameraFollowScript = parent.GetChild(2);
            CameraFollow personalScript = playerCameraFollowScript.GetComponent<CameraFollow>();
            personalScript.enabled = false;
            AudioListener audioListener = playerCameraFollowScript.GetComponent<AudioListener>();
            audioListener.enabled = false;
            Canvas canvas = playerCanvas.GetComponent<Canvas>();
            canvas.enabled = false;
            GetCharacterChatPanel().SetActive(false);
            // 영준 : 다른 플레이어 마커 선택
            myMarker.SetActive(false);
        }
        else
        {
            ChatManager chatManager = FindObjectOfType<ChatManager>();
            chatManager.RegisterPlayer(this);
            NickManager nickManager = FindObjectOfType<NickManager>();
            nickManager.RegisterPlayer(this);
           
            // 영준 : 플레이어 마커 선택
            otherMarker.SetActive(false);
        }

    }

   

    void Update()
    {
        //if (anim.GetCurrentAnimatorStateInfo(0).IsName("Idle"))
        //{
        //    anim.ResetTrigger("hammerCombo");
        //}
        if (!photonView.IsMine)
        {
            return;
        }

        GetInput();

        Attack();
        Move();
        Jump();
        Rotate();
        ResetPos();



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

            anim.SetTrigger("hammerCombo");
            //equipWeapon.Use();

            //fireDelay = 0;
        }

        if (skillDown && isSkillReady && !fDown)
        {
            anim.applyRootMotion = true;
            anim.SetTrigger("hammerSkill");
            skillFilter.fillAmount = 1;
            skillDelay = 0;
        }



    }

    void Move()
    {

        if (fDown) return;
        if (anim.GetCurrentAnimatorStateInfo(0).IsName("Combo1")
            || anim.GetCurrentAnimatorStateInfo(0).IsName("Combo2")
            || anim.GetCurrentAnimatorStateInfo(0).IsName("Combo3")
            || anim.GetCurrentAnimatorStateInfo(0).IsName("Combo4"))
        {

            return;
        }


        moveSpeed = playerStat.CurrentStatus(2);
        float local_moveSpeed = moveSpeed;

        moveVec = new Vector3(h, 0, v);


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


    public void Hammer1_Left()
    {
        
        audioSource.PlayOneShot(audioSwingLeft);
        equipWeapon[1].HammerComboAnimation();

    }

    public void Hammer1_Right()
    {
        audioSource.PlayOneShot(audioSwingRight);
        equipWeapon[0].HammerComboAnimation();
    }


    public void Hammer1_LeftEnd()
    {
        equipWeapon[1].HammerComboEndAnimation();
    }

    public void Hammer1_RIghtEnd()
    {
        equipWeapon[0].HammerComboEndAnimation();
    }


    public void Hammer2_Start()
    {
        //audioSource.clip = audioSwing2;
        audioSource.PlayOneShot(audioSwingLeft);
        equipWeapon[0].HammerComboAnimation();
        equipWeapon[1].HammerComboAnimation();
    }


    public void Hammer2_End()
    {
        equipWeapon[0].HammerComboEndAnimation();
        equipWeapon[1].HammerComboEndAnimation();
    }

    public void Hammer3_Start()
    {
        //audioSource.clip = audioSwing2;
        audioSource.PlayOneShot(audioSwingRight);
        equipWeapon[0].HammerComboAnimation();
        equipWeapon[1].HammerComboAnimation();
    }

    public void Hammer3_LeftHit()
    {
        //audioSource.clip = audioSwing2;
        audioSource.PlayOneShot(audioSwingLeft);
       
    }

    public void Hammer3_RightHit()
    {
        //audioSource.clip = audioSwing2;
        audioSource.PlayOneShot(audioSwingRight);

    }


    public void Hammer3_End()
    {
        audioSource.PlayOneShot(audioSwingLeft);
        equipWeapon[0].HammerComboEndAnimation();
        equipWeapon[1].HammerComboEndAnimation();
    }

    public void Hammer4_Start()
    {
        //audioSource.clip = audioSwing2;
        audioSource.PlayOneShot(audioWhirlwind);
        equipWeapon[0].HammerComboAnimation();
        equipWeapon[1].HammerComboAnimation();
    }


    public void Hammer4_End()
    {
        equipWeapon[0].HammerComboEndAnimation();
        equipWeapon[1].HammerComboEndAnimation();
    }

    public void HammerSkill1_Start()
    {
        anim.SetBool("IsSkill", true);
        audioSource.PlayOneShot(audioSwingRight);
        equipWeapon[0].HammerSkillStartAnimation();
    }


    public void HammerSkill1_End()
    {
        equipWeapon[0].HammerSkillEndAnimation();
    }

    public void HammerSkill2_Start()
    {
        audioSource.PlayOneShot(audioSkillRight);
        equipWeapon[0].HammerSkillStartAnimation();
    }


    public void HammerSkill2_End()
    {
        equipWeapon[0].HammerSkillEndAnimation();

    }

    public void HammerSkill3_Start()
    {
        audioSource.PlayOneShot(audioSkillLeft);
        audioSource.PlayOneShot(audioSwingRight);
        equipWeapon[0].HammerSkillStartAnimation();
        equipWeapon[1].HammerSkillStartAnimation();
    }


    public void HammerSkill3_End()
    {
        equipWeapon[0].HammerSkillEndAnimation();
        equipWeapon[1].HammerSkillEndAnimation();
    }


    public void HammerSkill4_Start()
    {

        audioSource.PlayOneShot(audioWhirlwind2);
        equipWeapon[0].HammerSkillStartAnimation();
        equipWeapon[1].HammerSkillStartAnimation();
    }


    public void HammerSkill4_End()
    {
        equipWeapon[0].HammerSkillEndAnimation();
        equipWeapon[1].HammerSkillEndAnimation();
    }

    public void Jump_Start()
    {
        audioSource.PlayOneShot(audiojump);
    }
    public void HammerSkill5_Start()
    {

        audioSource.PlayOneShot(audioCrash);
        Instantiate(crashEffect, crashPos.position, Quaternion.identity);
        equipWeapon[0].HammerSkillFinalAnimation();
        equipWeapon[1].HammerSkillFinalAnimation();
    }


    public void HammerSkill5_End()
    {
         
        StartCoroutine("Cooltime");
        currentCoolTime = skillCoolTime;
        coolTimeCounter.text = "" + currentCoolTime;
        StartCoroutine("CoolTimeCounter");
        skillDelay = 0;
        equipWeapon[0].HammerSkillFinalEndAnimation();
        equipWeapon[1].HammerSkillFinalEndAnimation();
    }

    

    public void Walking_Left()
    {
        audioSource.clip = audioLeftWalk;
        audioSource.Play();
    }

    public void Walking_Right()
    {
        audioSource.clip = audioRightWalk;
        audioSource.Play();
    }

    public void Running_Left()
    {
        audioSource.clip = audioLeftRun;
        audioSource.Play();
    }

    public void Running_Right()
    {
        audioSource.clip = audioRightRun;
        audioSource.Play();
    }

    public void Idle_Start()
    {
        equipWeapon[0].HammerSkillEndAnimation();
        equipWeapon[1].HammerSkillEndAnimation();
        anim.applyRootMotion = false;
        //audioSource.Pause();
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
