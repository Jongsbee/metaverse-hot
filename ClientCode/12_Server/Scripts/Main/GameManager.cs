using Photon.Pun;
using Photon.Pun.Demo.PunBasics;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using Cinemachine;

namespace Com.MyTutorial.Photon
{
    public class GameManager : MonoBehaviourPunCallbacks
    {
        #region Pubilc Fields
        public static GameManager Instance;
        [SerializeField] private GameObject popUp;
        public GameObject[] playerPrefabs;
        public GameObject player;
        public GameObject trashBox;
        private AudioListener sceneAudioListener;
        private HandlePopUps handlePopUps;
        private string toggleSelected;
        private GMForPhoton gM;
        private ToggleGroup characterSelectToggleGroup;
        private Button characterSelectButton;
        #endregion
        private bool isPlayerCreated = false;
        private GMForPhoton gMForPhoton;
        private ItemControllerForPhoton itemController;
        public PlayerStatForPhoton playerStat;




        private void Awake()
        {
            Instance = this;
            GameObject ui = GameObject.Find("UI");
            GameObject midCenterPopUps = ui.transform.Find("MidCenter_PopUps").gameObject;
            GameObject popUpCharacterSelect = midCenterPopUps.transform.Find("PopUp_CharacterSelect").gameObject;
            GameObject popup = popUpCharacterSelect.transform.Find("Popup").gameObject;
            characterSelectToggleGroup = popup.transform.Find("Character_Toggle").GetComponent<ToggleGroup>();
            characterSelectButton = popup.transform.Find("CharacterSubmitButton").GetComponent<Button>();
            GameObject sceneMainCamera = GameObject.Find("Camera");
            sceneAudioListener = sceneMainCamera.GetComponent<AudioListener>();
            gMForPhoton = GameObject.Find("Manager").transform.Find("GMForPhoton").GetComponent<GMForPhoton>();


        }

        void Start()
        {
            handlePopUps = FindObjectOfType<HandlePopUps>();
            itemController = GameObject.Find("Manager").transform.Find("ItemManager").GetComponent<ItemControllerForPhoton>();

            foreach (Toggle toggle in characterSelectToggleGroup.GetComponentsInChildren<Toggle>())
            {
                toggle.onValueChanged.AddListener(isOn => OnToggleClicked(toggle));
            }

            characterSelectButton.onClick.AddListener(Submit);
            gM = GameObject.Find("GMForPhoton").GetComponent<GMForPhoton>();
            toggleSelected = "Character_0";

            if (playerPrefabs.Length == 0)
            {
                Debug.LogError("<Color=Red><a>Missing</a></Color> playerPrefab Reference. Please set it up in GameObject 'Game Manager'", this);
            }
            else
            {
                // Check if player instance is already created
                if (PlayerManager.LocalPlayerInstance == null && !isPlayerCreated)
                {
                    isPlayerCreated = true;

                    // we're in a room. spawn a character for the local player. it gets synced by using PhotonNetwork.Instantiate

                    //PhotonNetwork.Instantiate("PlayerFromMain", new Vector3(46.4f, 20f, -313.97f), Quaternion.identity, 0);
                    Debug.Log("이곳입니다.");
                    
                }
                else
                {
                    Debug.LogFormat("Ignoring scene load for {0}", SceneManagerHelper.ActiveSceneName);
                }
            }

        }

        public void OnToggleClicked(Toggle toggle)
        {
            if (toggle.isOn)
            {
                toggleSelected = toggle.gameObject.name;
                Debug.Log("Selected " + toggleSelected.Substring(10));
            }
        }



        private void Submit()
        {

            gMForPhoton.userCharType = int.Parse(toggleSelected.Substring(10)); // 캐릭터를 할당해준다
            itemController.getAccountStacks(gMForPhoton.userCharType, gMForPhoton.userExp);

            switch (gMForPhoton.userCharType)
            {
                case 0:
                case 1:
                    InstanciatePlayer(); break;
                case 2: if(itemController.isHammerUnlocked)
                    {
                        InstanciatePlayer(); break;
                    } else break;
                case 3: break;
            }




            //// 캐릭터의 자식 중 태그가 "MainCamera"인 게임 오브젝트를 찾습니다.
            //    GameObject playerCameraObject = null;
            //    foreach (Transform child in player.transform)
            //    {
            //        if (child.gameObject.CompareTag("MainCamera"))
            //        {
            //            playerCameraObject = child.gameObject;
            //            break;
            //        }
            //    }

            // 해당 게임 오브젝트에서 카메라 컴포넌트를 가져옵니다.
            // 해당 게임 오브젝트에서 카메라 컴포넌트를 가져옵니다.


            // 해당 게임 오브젝트에서 카메라 컴포넌트를 가져옵니다.

            
        }

        private void Update()
        {
          
        }

        private void InstanciatePlayer()
        {

            player = PhotonNetwork.Instantiate(playerPrefabs[int.Parse(toggleSelected.Substring(10))].name, new Vector3(46.4f, 20f, -313.97f), Quaternion.identity, 0);
            handlePopUps.ClosePopUp(popUp);
            PhotonView playerPhotonView = player.GetComponentInChildren<PhotonView>();
            if (playerPhotonView.IsMine)
            {

                CinemachineVirtualCamera _mycam = GameObject.Find("Follow Cam").GetComponentInChildren<CinemachineVirtualCamera>();
                _mycam.m_Priority = 100; //higher number means high priority to get 

                PlayerManager.LocalPlayerInstance = this.gameObject;

                Transform parent = player.gameObject.transform;
                Transform playerCamera = parent.GetChild(2);
                playerStat = parent.GetChild(0).GetComponent<PlayerStatForPhoton>();
                playerStat.charIndex = int.Parse(toggleSelected.Substring(10));
                playerStat.setUserDamage();
                itemController.getGameManger();
                itemController.updateStack(1); itemController.updateStack(2);
                itemController.updatePlayerStacks();
                Camera personalCamera = playerCamera.GetComponent<Camera>();
                trashBox.transform.rotation = new Quaternion(0, personalCamera.transform.rotation.y - 180, 0, trashBox.transform.rotation.w);
            }


            if (sceneAudioListener.gameObject.activeSelf)
            {
                sceneAudioListener.gameObject.SetActive(false);
                Debug.Log("메인씬의 오디오 리스너를 종료했다!!");
            }
        }



    }
}
