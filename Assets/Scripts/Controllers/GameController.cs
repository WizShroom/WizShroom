using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.AI;

public class GameController : SingletonMono<GameController>
{

    public PlayerController mush;

    public List<AssignID> entitiesID;

    private void Awake()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void Start()
    {
        UIHandler.Instance.EnableUIByTypeList(new List<UIType>(){
            UIType.InGame,
        });
        //NavMeshSurfaceSingleton.Instance.navMeshSurface.BuildNavMesh();
    }

    public GameObject GetGameObjectFromID(string ID)
    {
        GameObject returnGameobject = null;

        foreach (AssignID idToCheck in entitiesID)
        {
            if (idToCheck.ID == ID)
            {
                returnGameobject = idToCheck.gameObject;
                break;
            }
        }

        return returnGameobject;
    }

    public void LoadScene(string sceneName)
    {
        mush.navMeshAgent.isStopped = true;
        StartCoroutine(LoadSceneAsync(sceneName));
    }

    public IEnumerator LoadSceneAsync(string sceneName)
    {
        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneName);
        UIHandler.Instance.EnableUIByType(UIType.LoadingScreen);

        float totalProgress = 0;
        while (!operation.isDone)
        {
            float sceneLoadProgress = Mathf.Clamp01(operation.progress / .09f);
            totalProgress = sceneLoadProgress / 2;
            UIHandler.Instance.UpdateLoadingScreen(totalProgress);
            yield return null;
        }

        totalProgress = 0.5f;

        Task dungeonGen = new Task(DungeonGenerator.Instance.GenerateDungeon());
        while (dungeonGen.Running)
        {
            yield return null;
        }

        totalProgress += 0.25f;
        UIHandler.Instance.UpdateLoadingScreen(totalProgress);

        Task navMeshBuild = new Task(BuildNavmesh());
        while (navMeshBuild.Running)
        {
            yield return null;
        }
        totalProgress += 0.25f;
        UIHandler.Instance.UpdateLoadingScreen(totalProgress);
        StartCoroutine(DungeonGenerator.Instance.PopulateDungeon());
        yield return new WaitForSeconds(0.5f);
        UIHandler.Instance.DisableUIByType(UIType.LoadingScreen);
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        mush.navMeshAgent.Warp(new Vector3(0, 0, 0));
        mush.navMeshAgent.SetDestination(mush.transform.position);
        mush.navMeshAgent.isStopped = false;
        Camera.main.transform.position = mush.transform.position + new Vector3(-5, 10, -5);
    }

    public IEnumerator BuildNavmesh()
    {
        NavMeshSurfaceSingleton.Instance.navMeshSurface.BuildNavMesh();
        yield return null;
    }

}
