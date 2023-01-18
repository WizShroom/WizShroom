using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameController : SingletonMono<GameController>
{

    public PlayerController mush;

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

    public void LoadScene(string sceneName)
    {
        mush.navMeshAgent.isStopped = true;
        StartCoroutine(LoadSceneAsync(sceneName));
    }

    public IEnumerator LoadSceneAsync(string sceneName)
    {
        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneName);
        UIHandler.Instance.EnableUIByType(UIType.LoadingScreen);
        while (!operation.isDone)
        {
            float progress = Mathf.Clamp01(operation.progress / .09f);
            UIHandler.Instance.UpdateLoadingScreen(progress);
            yield return null;
        }
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        mush.navMeshAgent.Warp(new Vector3(0, 0, 0));
        mush.navMeshAgent.SetDestination(mush.transform.position);
        mush.navMeshAgent.isStopped = false;

        NavMeshSurfaceSingleton.Instance.navMeshSurface.BuildNavMesh();

        UIHandler.Instance.DisableUIByType(UIType.LoadingScreen);
    }

}
