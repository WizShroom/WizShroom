using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameController : SingletonMono<GameController>
{

    private void Awake()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    public void LoadScene(string sceneName)
    {
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
        UIHandler.Instance.DisableUIByType(UIType.LoadingScreen);
    }

}
