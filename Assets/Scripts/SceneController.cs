using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Sockets;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum SceneIndexes
{
    SCENE_CONTROLLER = 0,
    TITLE = 1,
    LEVEL1 = 2,
}

public enum MapIndexes
{
    RUINS = 3
}

public class LoadProcess
{
    public string processName;
    public Action<LoadProcess> method;
    public float progress; // from zero to one

    /// <summary>
    /// Sets progress to 1, effectively making this process complete.
    /// </summary>
    public void Done() => progress = 1f;

    public LoadProcess(string processName, Action<LoadProcess> method)
    {
        this.processName = processName;
        this.method = method;
    }
}

public class LevelLoadEventArgs : EventArgs
{
    public SceneIndexes scene;

    public LevelLoadEventArgs(SceneIndexes level, MapIndexes map)
    {
        this.scene = level;
    }
}

public class LoadSettings
{
    public SceneIndexes sceneIndex;
    public MapIndexes mapIndex;
    public bool showsProceedButton;
    public List<LoadProcess> additionalLoadProcesses = new List<LoadProcess>();
    public Action OnComplete;
}

public class SceneController : MonoBehaviour
{
    [SerializeField] private LoadingScreen loadingScreen;

    private float currentLoadDelay;
    private List<AsyncOperation> scenesLoading = new List<AsyncOperation>();

    private List<LoadProcess> loadProcesses;

    public static SceneController instance { get; private set; }
    public static event EventHandler<LevelLoadEventArgs> OnLevelFinishedLoading;

    private void Awake()
    {
        instance = this;

        GenericLoad(new LoadSettings()
        {
            sceneIndex = SceneIndexes.TITLE,
            mapIndex = MapIndexes.RUINS,
        });

        Application.targetFrameRate = 120;
    }

    /// <summary>
    /// This function encapsulates the entire process of creating a constructor for LoadSettings and simple requires you to input only the scene and map indexes.
    /// For additional process to load as well as an OnComplete callback, please use GenericLoad()
    /// </summary>
    /// <param name="sceneIndex">The scene index you wish to load into</param>
    /// <param name="mapIndex">The map index you wish to load into</param>
    public void LoadScene(SceneIndexes sceneIndex, MapIndexes mapIndex, bool showsProceedButton = false)
    {
        GenericLoad(new LoadSettings() 
        { 
            sceneIndex = sceneIndex,
            mapIndex = mapIndex,
            showsProceedButton = showsProceedButton
        });
    }

    /// <summary>
    /// The main function that prepares the scenes and actions to preform while loading. It's in "additionalLoadProcessse" that allows you to add more
    /// things to load during this process, otherwise it only requires you to input the scene and the map you wish to load into. Unloads all extra scenes
    /// before proceeding to perform the loading of the new scenes
    /// </summary>
    /// <param name="loadSettings">The loading settings requried for this function to operate properly</param>
    public void GenericLoad(LoadSettings loadSettings)
    {
        loadingScreen.ToggleScreen(true).onComplete += () =>
        {
            Time.timeScale = 0f;

            UnloadAllScenes();
            
            loadProcesses = new List<LoadProcess>()
            {
                new LoadProcess("Loading Scene", process => StartCoroutine(LoadScene(loadSettings.sceneIndex, process))),
                new LoadProcess("Loading Map", process => StartCoroutine(LoadMap(loadSettings.mapIndex, process))),
            };

            if (loadSettings.additionalLoadProcesses.Count > 0)
            {
                loadProcesses.AddRange(loadSettings.additionalLoadProcesses);
            }

            StartCoroutine(LoadAllProcess(() =>
            {
                if (loadSettings.showsProceedButton)
                {
                    loadingScreen.ShowProceedButton();

                } else
                {
                    loadingScreen.ToggleScreen(false);
                    Time.timeScale = 1f;
                }

                loadSettings.OnComplete?.Invoke();
                OnLevelFinishedLoading?.Invoke(this, new LevelLoadEventArgs(loadSettings.sceneIndex, loadSettings.mapIndex));
            }));
        };
    }

    /// <summary>
    /// Unloads all scenes EXCEPT the persistant scene. Persistant scene should never be unloaded
    /// </summary>
    public void UnloadAllScenes()
    {
        for (int i = 0; i < SceneManager.sceneCount; i++)
        {
            if (SceneManager.GetSceneAt(i).buildIndex == (int)SceneIndexes.SCENE_CONTROLLER) continue;

            SceneManager.UnloadSceneAsync(SceneManager.GetSceneAt(i).buildIndex);
        }
    }

    public IEnumerator LoadAllProcess(Action OnComplete = null)
    {
        for (int i = 0; i < loadProcesses.Count; i++)
        {
            loadProcesses[i].method(loadProcesses[i]);
            while (loadProcesses[i].progress < 1f)
            {
                float progress = CalculateProgress();
                //loadingScreen.SetLoadingProgress(progress, $"{loadProcesses[i].processName}({Mathf.RoundToInt(loadProcesses[i].progress / 1f * 100f)}%)... [{Mathf.RoundToInt(progress * 100)}%]");
                loadingScreen.SetLoadingProgress(progress, $"{loadProcesses[i].processName}... [{Mathf.RoundToInt(progress * 100)}%]");
                yield return null;
            }
        }

        loadingScreen.SetLoadingProgress(1f, $"Loading Completed! [100%]");

        yield return new WaitForSecondsRealtime(1f);

        OnComplete?.Invoke();
        scenesLoading.Clear();
    }

    private float CalculateProgress()
    {
        float sum = 0f;
        for (int i = 0; i < loadProcesses.Count; i++)
        {
            sum += loadProcesses[i].progress;
        }

        return sum / loadProcesses.Count;
    }

    public void QuitGame()
    {
        loadingScreen.FadeIn().onComplete += () =>
        {
            Application.Quit();
        };
    }

    #region Loading Processes

    private IEnumerator LoadScene(SceneIndexes sceneIndex, LoadProcess process)
    {
        scenesLoading.Add(SceneManager.LoadSceneAsync((int)sceneIndex, LoadSceneMode.Additive));

        float totalProgress = 0f;
        for (int i = 0; i < scenesLoading.Count; i++)
        {
            while (!scenesLoading[i].isDone)
            {
                totalProgress = 0f;
                foreach (AsyncOperation operation in scenesLoading)
                {
                    totalProgress += operation.progress;
                }

                process.progress = totalProgress / scenesLoading.Count;
                yield return null;
            }
        }

        SceneManager.SetActiveScene(SceneManager.GetSceneByBuildIndex((int)sceneIndex));
        process.Done();
    }

    private IEnumerator LoadMap(MapIndexes mapIndex, LoadProcess process)
    {
        scenesLoading.Add(SceneManager.LoadSceneAsync((int)mapIndex, LoadSceneMode.Additive));

        float totalProgress = 0f;
        for (int i = 0; i < scenesLoading.Count; i++)
        {
            while (!scenesLoading[i].isDone)
            {
                totalProgress = 0f;
                foreach (AsyncOperation operation in scenesLoading)
                {
                    totalProgress += operation.progress;
                }

                process.progress = totalProgress / scenesLoading.Count;
                yield return null;
            }
        }

        process.Done();
    }

    /// <summary>
    /// A coroutine that delays the entry to the scene for a while to give the scene some realtime seconds to load.
    /// Scale the delay time based on how much time you'd like to give the scene to load.
    /// Too long and you'll piss the player off. I'd recommend less than 5 seconds
    /// </summary>
    /// <param name="process">The process tied to this coroutine</param>
    /// <param name="time">The delay time</param>
    /// <returns></returns>
    private IEnumerator DelayLoad_LoadProcess(LoadProcess process, float time)
    {
        currentLoadDelay = time;

        while (currentLoadDelay > 0f)
        {
            currentLoadDelay -= Time.unscaledDeltaTime;
            process.progress = 1 - (currentLoadDelay / time);
            yield return null;
        }

        process.Done();
    }
    
    #endregion

}
