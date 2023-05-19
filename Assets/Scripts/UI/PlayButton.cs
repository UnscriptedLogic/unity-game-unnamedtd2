using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayButton : MonoBehaviour
{
    [SerializeField] private Button playBtn;

    private void Start()
    {
        playBtn.onClick.AddListener(() => SceneController.instance.LoadGameFromTitle());
    }
}
