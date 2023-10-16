using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ObjectOpacityCtrl : MonoBehaviour
{
    public string playerTag = "Player";
    public float transparencyValue = 0.5f;
    public LayerMask playerLayer;

    private Material[] materials;
    private Color[] originalColors;
    private bool isTransparent;

    private void Start()
    {
        Renderer renderer = GetComponent<Renderer>();
        materials = renderer.materials;
        originalColors = new Color[materials.Length];

        for (int i = 0; i < materials.Length; i++)
        {
            originalColors[i] = materials[i].color;
        }

        isTransparent = false;
    }

    private void Update()
    {

        if (Physics.Raycast(transform.position, -transform.up, out RaycastHit hit, Mathf.Infinity, playerLayer))
        {
            Debug.Log(transform.position);
            Debug.Log(-transform.up);
            

            if (hit.collider.CompareTag(playerTag) && !isTransparent)
            {Debug.Log(transform.position);
            Debug.Log(transform.position);
                SetTransparency(transparencyValue);
            }
        }
        else if (isTransparent)
        {
            ResetTransparency();
        }
    }

    private void SetTransparency(float transparency)
    {
        for (int i = 0; i < materials.Length; i++)
        {
            Color newColor = originalColors[i];
            newColor.a = transparency;
            materials[i].color = newColor;
        }

        isTransparent = true;
    }

    private void ResetTransparency()
    {
        for (int i = 0; i < materials.Length; i++)
        {
            materials[i].color = originalColors[i];
        }

        isTransparent = false;
    }
}
