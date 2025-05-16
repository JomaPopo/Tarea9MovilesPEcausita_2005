using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

public class GlobalSceneManager : MonoBehaviour
{
    public static GlobalSceneManager Instance { get; private set; }

    private List<string> loadedScenes = new List<string>();

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void LoadScene(string sceneName, bool setActive = false)
    {
        if (!SceneManager.GetSceneByName(sceneName).isLoaded)
        {
            SceneManager.LoadScene(sceneName, LoadSceneMode.Additive);
            loadedScenes.Add(sceneName);

            if (setActive)
            {
                SceneManager.SetActiveScene(SceneManager.GetSceneByName(sceneName));
            }
        }
        else
        {
            Debug.LogWarning("Scene already loaded: " + sceneName);
        }
    }

    public void UnloadScene(string sceneName)
    {
        if (loadedScenes.Contains(sceneName))
        {
            SceneManager.UnloadSceneAsync(sceneName);
            loadedScenes.Remove(sceneName);
        }
        else
        {
            Debug.LogWarning("Scene not found or not loaded: " + sceneName);
        }
    }

    public void SetActiveScene(string sceneName)
    {
        Scene scene = SceneManager.GetSceneByName(sceneName);
        if (scene.isLoaded)
        {
            SceneManager.SetActiveScene(scene);
        }
        else
        {
            Debug.LogWarning("Scene not loaded: " + sceneName);
        }
    }

    public void ReloadScene(string sceneName)
    {
        if (loadedScenes.Contains(sceneName))
        {
            SceneManager.UnloadSceneAsync(sceneName);
            SceneManager.LoadScene(sceneName, LoadSceneMode.Additive);
        }
        else
        {
            Debug.LogWarning("Scene not loaded: " + sceneName);
        }
    }
}