using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawGizmoPoint : MonoBehaviour
{

    public Color color = Color.red;
    public float radius = 0.1f;
    public shape ourShape;

    private void OnDrawGizmos()
    {
        Gizmos.color = color;
        switch (ourShape)
        {
            case shape.SPHERE:
                Gizmos.DrawWireSphere(transform.position, radius);
                break;
            case shape.CUBE:
                Gizmos.DrawWireCube(transform.position, new Vector3(radius, radius, radius));
                break;
        }
    }

    public enum shape
    {
        SPHERE,
        CUBE,
    }
}
