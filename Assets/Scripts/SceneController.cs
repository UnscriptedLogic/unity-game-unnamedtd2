using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum SceneIndexes
{
    SCENE_CONTROLLER = 0,
    TITLE = 1,
    GAME = 2
}

public class LoadProcess
{
    public string processName;
    public Action<LoadProcess> method;
    public float progress; // from zero to one

    public void Done() => progress = 1f;

    public LoadProcess(string processName, Action<LoadProcess> method)
    {
        this.processName = processName;
        this.method = method;
    }
}

public class SceneController : MonoBehaviour
{
    [SerializeField] private LoadingScreen loadingScreen;
    [SerializeField] private float loadDelay = 5f;

    private float currentLoadDelay;
    private List<AsyncOperation> scenesLoading = new List<AsyncOperation>();

    private List<LoadProcess> loadProcesses;

    public static SceneController instance { get; private set; }

    private void Awake()
    {
        instance = this;

        loadingScreen.ToggleLoadingScreen(false);
        loadingScreen.FadeOut();
        scenesLoading.Add(SceneManager.LoadSceneAsync((int)SceneIndexes.TITLE, LoadSceneMode.Additive));
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

    public void LoadGameFromTitle()
    {
        loadingScreen.ToggleScreen(true).onComplete += () =>
        {
            Time.timeScale = 0f;
            LoadProcess gameSceneLoad = new LoadProcess("Loading Scenes", process => StartCoroutine(GameScene_LoadProcess(process)));
            LoadProcess delayLoad = new LoadProcess("Prepping Environment", process => StartCoroutine(DelayLoad_LoadProcess(process)));

            loadProcesses = new List<LoadProcess>
            {
                gameSceneLoad,
                delayLoad
            };

            StartCoroutine(LoadAllProcess(() =>
            {
                loadingScreen.ShowProceedButton();
            }));
        };
    }

    private IEnumerator GameScene_LoadProcess(LoadProcess process)
    {
        float totalProgress = 0f;
        scenesLoading.Add(SceneManager.UnloadSceneAsync((int)SceneIndexes.TITLE));
        scenesLoading.Add(SceneManager.LoadSceneAsync((int)SceneIndexes.GAME, LoadSceneMode.Additive));

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

    private IEnumerator DelayLoad_LoadProcess(LoadProcess process)
    {
        currentLoadDelay = loadDelay;

        while (currentLoadDelay > 0f)
        {
            currentLoadDelay -= Time.unscaledDeltaTime;
            process.progress = 1 - (currentLoadDelay / loadDelay);
            yield return null;
        }

        process.Done();
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
}
