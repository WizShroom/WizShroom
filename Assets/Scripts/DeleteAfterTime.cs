using UnityEngine;
using UnityEngine.AI;

public class DeleteAfterTime : MonoBehaviour
{
    public float timer = 1f;
    public bool reloadNavmeshOnDestroy = false;
    public bool destroyOnAwake = false;

    private void Awake()
    {
        if (!destroyOnAwake)
        {
            return;
        }
        Destroy(gameObject, timer);
    }

    public void CallDestroy(float timerToDestroy)
    {
        Destroy(gameObject, timerToDestroy);
    }

    private void OnDestroy()
    {
        if (reloadNavmeshOnDestroy)
        {
            NavMeshSurface navMeshSurface = NavMeshSurfaceSingleton.Instance.navMeshSurface;
            navMeshSurface.UpdateNavMesh(navMeshSurface.navMeshData);
        }
    }
}