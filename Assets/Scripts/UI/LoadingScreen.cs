using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnscriptedLogic.MathUtils;

public class LoadingScreen : MonoBehaviour
{
    [Header("Load Screen Details")]
    [SerializeField] private LoadingScreenSO loadingScreenSO;
    [SerializeField] private Transform loadingScreenParent;
    [SerializeField] private Image splashArt;
    [SerializeField] private TextMeshProUGUI titleTMP;
    [SerializeField] private TextMeshProUGUI descTMP;
    
    [Header("Load Screen Bar")]
    [SerializeField] private float fillSmoothTime = 1f;
    [SerializeField] private Transform loadBarParent;
    [SerializeField] private TextMeshProUGUI loadingTMP;
    [SerializeField] private Slider loadingSlider;

    [Header("Proceed Section")]
    [SerializeField] private Transform buttonParent;
    [SerializeField] private Button proceedBtn;

    [Header("Transition")]
    [SerializeField] private float transitionDelay = 1f;
    [SerializeField] private CanvasGroup transitionScreen;

    public event EventHandler OnFadeInCompleted;
    public event EventHandler OnFadeOutCompleted;

    private bool firstLoad;
    private int currentIndex;
    private LoadingScreenSO.LoadScreen[] LoadScreens => loadingScreenSO.LoadingScreens;

    public static LoadingScreen instance { get; private set; }

    private void Awake()
    {
        instance = this;
        firstLoad = true;
    }

    public void InitializeLoadingScreen()
    {
        if (!firstLoad)
        {
            List<int> splashArtIndex = new List<int>();
            for (int i = 0; i < LoadScreens.Length; i++)
            {
                splashArtIndex.Add(i);
            }
            splashArtIndex.RemoveAt(currentIndex);
            currentIndex = RandomLogic.FromList(splashArtIndex);

        } else
        {
            firstLoad = false;
            RandomLogic.FromArray(LoadScreens, out currentIndex);
        }

        splashArt.sprite = LoadScreens[currentIndex].SplashArt;
        titleTMP.text = LoadScreens[currentIndex].Title;
        descTMP.text = LoadScreens[currentIndex].Desc;

        ToggleProceedButton(false);
    }

    /// <summary>
    /// Toggles the loading screen with fade in and out.
    /// </summary>
    /// <param name="loadingScreenVisible">Displays the loading screen or not</param>
    /// <returns>A DoTween sequence of the entire process</returns>
    public Sequence ToggleScreen(bool loadingScreenVisible)
    {
        Sequence sequence = DOTween.Sequence();
        sequence.SetUpdate(true);

        Tween fadeInTween = FadeIn();
        fadeInTween.onComplete += () =>
        {
            ToggleLoadingScreenImmediate(loadingScreenVisible);
            if (loadingScreenVisible)
            {
                InitializeLoadingScreen();
                loadingSlider.value = 0f;
            }

            OnFadeInCompleted?.Invoke(this, EventArgs.Empty);
        };

        sequence.Append(fadeInTween);
        sequence.Append(FadeOut()).onComplete += () => OnFadeOutCompleted?.Invoke(this, EventArgs.Empty);
        return sequence;
    }

    /// <summary>
    /// Toggles the proceed button. This also disables or enables the loading progress bar in its place.
    /// </summary>
    /// <param name="value"></param>
    public void ToggleProceedButton(bool value)
    {
        buttonParent.gameObject.SetActive(value);
        loadBarParent.gameObject.SetActive(!value);
        loadingTMP.gameObject.SetActive(!value);
    }

    public TweenerCore<float, float, FloatOptions> FadeIn()
    {
        TweenerCore<float, float, FloatOptions> tween = transitionScreen.DOFade(1f, transitionDelay);
        tween.SetUpdate(true);
        return tween;
    }

    public TweenerCore<float, float, FloatOptions> FadeOut()
    {
        TweenerCore<float, float, FloatOptions> tween = transitionScreen.DOFade(0f, transitionDelay);
        tween.SetUpdate(true);
        return tween;
    }

    public void ToggleLoadingScreenImmediate(bool visible)
    {
        loadingScreenParent.gameObject.SetActive(visible);
    }

    public void SetLoadingProgress(float amount, string loadingText = "")
    {
        loadingSlider.DOValue(amount, fillSmoothTime).SetUpdate(true);
        loadingTMP.text = loadingText;
    }

    public void ShowProceedButton()
    {
        ToggleProceedButton(true);

        proceedBtn.onClick.AddListener(() =>
        {
            ToggleScreen(false);
            Time.timeScale = 1f;
        });
    }
}
