using Newtonsoft.Json.Bson;
using Photon.Pun;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Random = System.Random;

public class ClickNodeForPhoton : MonoBehaviourPunCallbacks
{
    public Color StartColor;

   
    public Color SelectColor;

   
    [SerializeField] int isBuilt = 0;

   
    [SerializeField] BuildingTemplate Building;
    [SerializeField] TowerTemplate[] TowerPrefabs;

    [SerializeField]
    private TextMeshProUGUI actionText;

  
    public Renderer rend;

  
    Random randomObj = new Random();
    int randomValue;

    GameObject totalManager;
    GMForPhoton gm;
    GameObject UIObject;


    private float buildDelay = 0.3f;
    GameObject arrow;

    bool buildActivated;
   


    void Awake()
    {
        UIObject = GameObject.Find("UI");
        totalManager = GameObject.Find("Manager");
        gm = totalManager.transform.Find("GMForPhoton").GetComponent<GMForPhoton>();
        rend = GetComponent<Renderer>();
        actionText = GameObject.Find("MidCenter_ShowText").gameObject.GetComponentInChildren<TextMeshProUGUI>();
        rend.material.color = StartColor;
        arrow = transform.GetChild(2).gameObject;
    }
    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {

            if (Input.GetKey(KeyCode.E))
            {
                buildActivated = true;
                BuildTower();
            }

            
        }

    }

    private void OnTriggerExit(Collider other)
    {
        buildActivated = false;
    }

    [PunRPC]
    public void TileOff()
    {
        gameObject.SetActive(false);
    }

    public bool CreateTower(int templateNumber, Vector3 position)
    {
        GameObject projectilePrefab = TowerPrefabs[templateNumber].weapon[0].Prefab;
        if (gm == null)
        {
            return false;
        }

        if (gm.TowerCnt > 0)
        {
            GameObject towerClone = PhotonNetwork.Instantiate(TowerPrefabs[templateNumber].weapon[0].Prefab.name, position, Quaternion.identity);
            gm.BuildTower();
            StartCoroutine(Build());
            // cost += inflation;

            towerClone.GetComponent<TowerWeaponForPhoton>().SetUp(TowerPrefabs[templateNumber], actionText, Building);
            photonView.RPC("TileOff", RpcTarget.All);
            return true;
        }
        return false;
    }


    IEnumerator Build()
    {
        foreach (Transform child in transform)
        {
            child.gameObject.SetActive(false);

            foreach (Transform grandChild in child)
            {
                grandChild.gameObject.SetActive(false);
            }
        }

        foreach (Transform child in transform)
        {
            child.gameObject.SetActive(true);
            yield return new WaitForSeconds(buildDelay);
            foreach (Transform grandChild in child)
            {
                grandChild.gameObject.SetActive(true);
            }
        }

    }


    public void BuildTower()
    {
        if (isBuilt == 0 && buildActivated)
        {
            bool isSuccessful;
            randomValue = randomObj.Next(0, 6);
            //Debug.Log(randomValue);
            rend.material.color = SelectColor;
          
            //photonView.RPC("CreateTower",RpcTarget.All,randomValue, transform.position);
            isSuccessful = CreateTower(randomValue, transform.position);
            isBuilt = randomValue;
            BuildInfoDisappear();
            
        }
       
    }


    private void BuildInfoAppear()
    {
        buildActivated = true;
        actionText.gameObject.SetActive(true);
        actionText.text = "Build Tower" + "<color=yellow>" + "(E)" + "</color>";
    }

    private void BuildInfoDisappear()
    {
        buildActivated = false;
        actionText.gameObject.SetActive(false);
        actionText.text = "";

    }

    public void Arrow(bool show)
    {
        arrow.SetActive(show);
    }

    
}
