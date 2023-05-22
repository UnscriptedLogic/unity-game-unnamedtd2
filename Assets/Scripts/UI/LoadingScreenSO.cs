using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Loading Screen", menuName = "ScriptableObjects/Create New Loading Screen")]
public class LoadingScreenSO : ScriptableObject
{
    [System.Serializable]
    public struct LoadScreen
    {
        [SerializeField] private Sprite splashart;
        [TextArea(1,1)] [SerializeField] private string title;
        [TextArea(5,5)] [SerializeField] private string tip;

        public Sprite SplashArt => splashart;
        public string Title => title;
        public string Desc => tip;
    }

    [SerializeField] private LoadScreen[] loadingScreens;
    
    public LoadScreen[] LoadingScreens => loadingScreens;
}
