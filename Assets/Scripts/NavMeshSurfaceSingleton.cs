using UnityEngine;
using UnityEngine.AI;

public class NavMeshSurfaceSingleton : MonoBehaviour
{
    private static NavMeshSurfaceSingleton instance;
    public static NavMeshSurfaceSingleton Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<NavMeshSurfaceSingleton>();
            }
            return instance;
        }
    }

    public NavMeshSurface navMeshSurface;
}