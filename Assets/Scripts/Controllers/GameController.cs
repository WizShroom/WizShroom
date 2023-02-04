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
        UIHandler.Instance.DisableUIByTypeList(new List<UIType>(){
            UIType.CharacterInfo,
            UIType.LoadingScreen,
            UIType.Inventory,
            UIType.Dialogue,
            UIType.MainMenu,
        });
        UIHandler.Instance.EnableUIByTypeList(new List<UIType>(){
            UIType.InGame,
        });
        //NavMeshSurfaceSingleton.Instance.navMeshSurface.BuildNavMesh();
    }

    public GameObject GetGameObjectFromID(string ID)
    {
        GameObject returnGameobject = null;

        if (entitiesID == null)
        {
            return null;
        }

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
        GameEventHandler.Instance.SendEvent(gameObject, EVENT.PAUSED);
        GameEventHandler.Instance.SendEvent(gameObject, EVENT.LOADINGLEVEL);
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
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        SceneInformation sceneInformation = GameObject.FindGameObjectWithTag("SceneInformation").GetComponent<SceneInformation>();
        StartCoroutine(FinishLoading(sceneInformation.sceneFlags));
    }

    public IEnumerator FinishLoading(SceneLoadingFlags sceneLoadingFlags)
    {
        float totalProgress = 0.5f;

        if ((sceneLoadingFlags & SceneLoadingFlags.RequireDungeon) == SceneLoadingFlags.RequireDungeon)
        {
            Task dungeonGen = new Task(DungeonGenerator.Instance.GenerateDungeon());
            while (dungeonGen.Running)
            {
                yield return null;
            }
        }

        totalProgress += 0.25f;
        UIHandler.Instance.UpdateLoadingScreen(totalProgress);
        if ((sceneLoadingFlags & SceneLoadingFlags.RequireNavMesh) == SceneLoadingFlags.RequireNavMesh)
        {
            Task navMeshBuild = new Task(BuildNavmesh());
            while (navMeshBuild.Running)
            {
                yield return null;
            }
        }

        totalProgress += 0.25f;
        UIHandler.Instance.UpdateLoadingScreen(totalProgress);

        if ((sceneLoadingFlags & SceneLoadingFlags.RequireDungeon) == SceneLoadingFlags.RequireDungeon)
        {
            StartCoroutine(DungeonGenerator.Instance.PopulateDungeon());
        }
        yield return new WaitForSeconds(0.5f);

        GameObject levelLanding = GameObject.FindGameObjectWithTag("LevelLanding");
        mush.navMeshAgent.Warp(levelLanding.transform.position);
        mush.navMeshAgent.SetDestination(mush.transform.position);
        Camera.main.transform.position = mush.transform.position + new Vector3(-5, 10, -5);
        UIHandler.Instance.DisableUIByType(UIType.LoadingScreen);
        GameEventHandler.Instance.SendEvent(gameObject, EVENT.RESUMED);
        GameEventHandler.Instance.SendEvent(gameObject, EVENT.LOADEDLEVEL);
    }

    public IEnumerator BuildNavmesh()
    {
        NavMeshSurfaceSingleton.Instance.navMeshSurface.BuildNavMesh();
        yield return null;
    }

}
