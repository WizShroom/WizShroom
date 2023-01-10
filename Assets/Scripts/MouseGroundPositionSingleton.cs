using UnityEngine;

public class MouseGroundPositionSingleton : MonoBehaviour
{
    private static MouseGroundPositionSingleton instance;
    public static MouseGroundPositionSingleton Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<MouseGroundPositionSingleton>();
            }
            return instance;
        }
    }

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