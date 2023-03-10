using UnityEngine;

public class MouseGroundPositionSingleton : SingletonMono<MouseGroundPositionSingleton>
{
    public Vector3 returnGroundPosition;
    public LayerMask groundLayer;

    private void Update()
    {
        Vector3 mousePos = Input.mousePosition;
        Ray ray = Camera.main.ScreenPointToRay(mousePos);
        RaycastHit hit;
        Vector3 hitPoint = default(Vector3);
        if (Physics.Raycast(ray, out hit, Mathf.Infinity, groundLayer))
        {
            hitPoint = hit.point;
        }

        returnGroundPosition = hitPoint;
    }
}