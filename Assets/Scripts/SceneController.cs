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
    public MapIndexes map;

    public LevelLoadEventArgs(SceneIndexes level, MapIndexes map)
    {
        this.scene = level;
        this.map = map;
    }
}

public class SceneController : MonoBehaviour
{
    [SerializeField] private LoadingScreen loadingScreen;
    [SerializeField] private float gameLoadDelay = 4f;
    [SerializeField] private float homeLoadDelay = 2f;
    [SerializeField] private float startDelay = 1f;

    private float currentLoadDelay;
    private List<AsyncOperation> scenesLoading = new List<AsyncOperation>();

    private List<LoadProcess> loadProcesses;

    public static SceneController instance { get; private set; }
    public static event EventHandler<LevelLoadEventArgs> OnLevelFinishedLoading;

    private void Awake()
    {
        instance = this;

        LoadTitleFromStart();

        Application.targetFrameRate = 60;
    }

    public void LoadTitleFromStart()
    {
        loadingScreen.ToggleScreen(true).onComplete += () =>
        {
            Time.timeScale = 0f;
            LoadProcess loadTitle = new LoadProcess("Loading Scene", process => StartCoroutine(LoadTitleAtStart_LoadProcess(process)));
            LoadProcess delayLoad = new LoadProcess("Entering game", process => StartCoroutine(DelayLoad_LoadProcess(process, startDelay)));

            loadProcesses = new List<LoadProcess>
            {
                loadTitle,
                delayLoad
            };

            StartCoroutine(LoadAllProcess(() =>
            {
                Time.timeScale = 1f;
                loadingScreen.ToggleScreen(false);

                OnLevelFinishedLoading?.Invoke(this, new LevelLoadEventArgs(SceneIndexes.TITLE, MapIndexes.RUINS));
            }));
        };
    }

    public void LoadGameFromTitle()
    {
        loadingScreen.ToggleScreen(true).onComplete += () =>
        {
            Time.timeScale = 0f;
            LoadProcess gameSceneLoad = new LoadProcess("Loading Scenes", process => StartCoroutine(GameScene_LoadProcess(process)));
            LoadProcess delayLoad = new LoadProcess("Prepping Environment", process => StartCoroutine(DelayLoad_LoadProcess(process, gameLoadDelay)));

            loadProcesses = new List<LoadProcess>
            {
                gameSceneLoad,
                delayLoad
            };

            StartCoroutine(LoadAllProcess(() =>
            {
                loadingScreen.ShowProceedButton();
                OnLevelFinishedLoading?.Invoke(this, new LevelLoadEventArgs(SceneIndexes.LEVEL1, MapIndexes.RUINS));

            }));
        };
    }

    public void LoadTitleFromGame()
    {
        loadingScreen.ToggleScreen(true).onComplete += () =>
        {
            LoadProcess cleanUp = new LoadProcess("Cleaning up the field", process => StartCoroutine(GameCleanUp_LoadProcess(process)));
            LoadProcess sceneLoad = new LoadProcess("Loading Scene", process => StartCoroutine(TitleScene_LoadProcess(process)));
            LoadProcess delayLoad = new LoadProcess("Returning home", process => StartCoroutine(DelayLoad_LoadProcess(process, homeLoadDelay)));

            loadProcesses = new List<LoadProcess>()
            {
                cleanUp,
                sceneLoad,
                delayLoad,
            };

            StartCoroutine(LoadAllProcess(() =>
            {
                loadingScreen.ToggleScreen(false);
                OnLevelFinishedLoading?.Invoke(this, new LevelLoadEventArgs(SceneIndexes.TITLE, MapIndexes.RUINS));
            }));
        };
    }

    public void QuitGame()
    {
        loadingScreen.FadeIn().onComplete += () =>
        {
            Application.Quit();
        };
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

    #region Loading Processes

    private IEnumerator LoadTitleAtStart_LoadProcess(LoadProcess process)
    {
        scenesLoading.Add(SceneManager.LoadSceneAsync((int)SceneIndexes.TITLE, LoadSceneMode.Additive));
        scenesLoading.Add(SceneManager.LoadSceneAsync((int)MapIndexes.RUINS, LoadSceneMode.Additive));

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

        SceneManager.SetActiveScene(SceneManager.GetSceneByBuildIndex((int)SceneIndexes.TITLE));
        process.Done();
    }

    private IEnumerator GameCleanUp_LoadProcess(LoadProcess process)
    {
        yield return StartCoroutine(TowerDefenseManager.instance.SceneExitCleanUp_Coroutine());
        yield return new WaitForSecondsRealtime(1f);
        process.Done();
    }

    private IEnumerator TitleScene_LoadProcess(LoadProcess process)
    {
        SceneManager.SetActiveScene(SceneManager.GetSceneByBuildIndex((int)SceneIndexes.SCENE_CONTROLLER));

        float totalProgress = 0f;
        scenesLoading.Add(SceneManager.UnloadSceneAsync((int)SceneIndexes.LEVEL1));
        scenesLoading.Add(SceneManager.UnloadSceneAsync((int)MapIndexes.RUINS));
        scenesLoading.Add(SceneManager.LoadSceneAsync((int)SceneIndexes.TITLE, LoadSceneMode.Additive));

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

        SceneManager.SetActiveScene(SceneManager.GetSceneByBuildIndex((int)SceneIndexes.TITLE));
        process.Done();
    }

    private IEnumerator GameScene_LoadProcess(LoadProcess process)
    {
        SceneManager.SetActiveScene(SceneManager.GetSceneByBuildIndex((int)SceneIndexes.SCENE_CONTROLLER));

        float totalProgress = 0f;
        scenesLoading.Add(SceneManager.UnloadSceneAsync((int)SceneIndexes.TITLE));
        scenesLoading.Add(SceneManager.UnloadSceneAsync((int)MapIndexes.RUINS));
        scenesLoading.Add(SceneManager.LoadSceneAsync((int)MapIndexes.RUINS, LoadSceneMode.Additive));
        scenesLoading.Add(SceneManager.LoadSceneAsync((int)SceneIndexes.LEVEL1, LoadSceneMode.Additive));

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

        SceneManager.SetActiveScene(SceneManager.GetSceneByBuildIndex((int)SceneIndexes.LEVEL1));
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
