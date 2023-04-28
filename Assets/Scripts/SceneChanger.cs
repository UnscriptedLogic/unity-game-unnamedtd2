using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum GameScenes
{
    Home,
    Game
}

public class SceneChanger : MonoBehaviour
{
    [SerializeField] private float defaultDelay = 2f;
    public static SceneChanger instance { get; private set; }

    private void Awake()
    {
        instance = this;
    }

    public void ChangeScene(GameScenes gameScenes)
    {
        ChangeScene(gameScenes, defaultDelay);
    }

    public void ChangeScene(GameScenes gameScenes, float delay = 2f)
    {
        StartCoroutine(MoveToScene(GetSceneByEnum(gameScenes), delay));
    }

    public int GetSceneByEnum(GameScenes gameScenes)
    {
        int index = 0;

        switch (gameScenes)
        {
            case GameScenes.Home:
                index = 0;
                break;
            case GameScenes.Game:
                index = 1;
                break;
            default:
                break;
        }

        return index;
    }

    private IEnumerator MoveToScene(int index, float delay)
    {
        yield return new WaitForSeconds(delay);
        SceneManager.LoadScene(index);
    }
}
