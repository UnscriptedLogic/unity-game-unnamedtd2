using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ResultsPageUI : MonoBehaviour
{
    [SerializeField] private Button[] homeBtn;
    [SerializeField] private Button[] quitBtn;

    private void Start()
    {
        for (int i = 0; i < homeBtn.Length; i++)
        {
            homeBtn[i].onClick.AddListener(() =>
            {
                SceneController.instance.LoadScene(SceneIndexes.TITLE, MapIndexes.RUINS);
            });
        }

        for (int i = 0; i < quitBtn.Length; i++)
        {
            quitBtn[i].onClick.AddListener(() => Application.Quit());
        }
    }
}
