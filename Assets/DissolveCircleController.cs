using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DissolveCircleController : MonoBehaviour
{
    Camera mainCamera;

    public Transform player;
    public LayerMask wallLayer;

    private void Start()
    {
        mainCamera = GetComponent<Camera>();
    }

    private void Update()
    {
        Vector2 cutoutPosition = mainCamera.WorldToViewportPoint(player.position);

        Vector3 mousePos = Input.mousePosition;
        Ray ray = Camera.main.ScreenPointToRay(mousePos);
        RaycastHit[] hits = Physics.RaycastAll(ray, Mathf.Infinity, wallLayer);

        foreach (RaycastHit hit in hits)
        {
            Material[] materials = hit.transform.GetComponent<Renderer>().materials;
            foreach (Material material in materials)
            {
                material.SetVector("_CutoutPosition", cutoutPosition);
            }
        }

    }
}
