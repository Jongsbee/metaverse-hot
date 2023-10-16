using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageEffect : MonoBehaviour
{

    void Update()
    {
        string myname = this.name;
        //Debug.Log(myname);
        // Debug.Log(this.gameObject.transform.childCount);
        this.transform.position = Vector3.Lerp(this.transform.position, this.transform.position + new Vector3(0, (this.gameObject.transform.parent.childCount), 0), 0.25f);

    }
}
