using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Cinemachine;
using Photon.Pun;
using Photon.Pun.Demo.PunBasics;

public class ArcherPlayerMoverForPhoton : MonoBehaviourPunCallbacks, IPlayer
{
    // 발사체 프리팹
    //[SerializeField] GameObject laser;

    // 이동 속도
    [SerializeField] private float moveSpeed;
    [SerializeField] private float jumpPower;


    public Image skillFilter;

    private PlayerStatForPhoton playerStat;

    GMForPhoton gm;


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
    bool resetDown;

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
        return parent.Find("ArcherPlayerBody/ChatBalloonCanvas/TextBox").gameObject;
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
        equipWeapon = GetComponentInChildren<Weapon>();
        rigid = GetComponent<Rigidbody>();
        playerStat = GetComponent<PlayerStatForPhoton>();
        audioSource = GetComponent<AudioSource>();
        jumpPower = 20f;
        skillFilter.fillAmount = 0;
        // 영준 : 마커 할당
        myMarker = parent.Find("ArcherPlayerBody/PlayerMarker").gameObject;
        otherMarker = parent.Find("ArcherPlayerBody/OtherPlayerMarker").gameObject;

        TextMeshProUGUI nickText = parent.Find("ArcherPlayerBody/NickNameCanvas/NickBox/Text_Nick").GetComponent<TextMeshProUGUI>();
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
            // 영준 : 플레이어 마커 선택
            otherMarker.SetActive(false);
          
        }


    }

    void Update()
    {
        if (!photonView.IsMine)
        {
            return;
        }
        GetInput();
        Attack(); 
        Move();
        Jump();
        ResetPos();
        //Rotate();
    }

    // 이동 관련 함수를 짤 때는 Update보다 FixedUpdate가 더 효율이 좋다고 한다. 그래서 사용했다.
    void FixedUpdate()
    {
        if (!photonView.IsMine)
        {
            return;
        }
        MouseTracking();
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

    void MouseTracking()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, Mathf.Infinity))
        {
            //Debug.Log(this.name);
            transform.LookAt(new Vector3(hit.point.x, transform.position.y, hit.point.z));
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
        if (equipWeapon == null || isRun || anim.GetBool("IsMultiShot"))
            return;

        fireDelay += Time.deltaTime;
        isFireReady = equipWeapon.rate < fireDelay;

        skillDelay += Time.deltaTime;
        isSkillReady = skillCoolTime <= skillDelay;

        if (fDown && isFireReady)
        {
            anim.SetBool("IsFire", true);
            if (equipWeapon.type == Weapon.Type.Range)
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

    public void MultiShotAnim_Start()
    {
        audioSource.PlayOneShot(audioArrowSkill);
    }

    public void MultiShotAnim_End(int cnt)
    {


        shotCnt -= cnt;
        if (shotCnt <= 0)
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
