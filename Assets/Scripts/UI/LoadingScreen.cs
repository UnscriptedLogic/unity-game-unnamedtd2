using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using System;
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
    [SerializeField] private TextMeshProUGUI loadingTMP;
    [SerializeField] private Slider loadingSlider;
    
    [Header("Transition")]
    [SerializeField] private float transitionDelay = 1f;
    [SerializeField] private CanvasGroup transitionScreen;

    public event EventHandler OnFadeInCompleted;
    public event EventHandler OnFadeOutCompleted;

    private int currentIndex;
    private LoadingScreenSO.LoadScreen[] LoadScreens => loadingScreenSO.LoadingScreens;

    public static LoadingScreen instance { get; private set; }

    private void Awake()
    {
        instance = this;
    }

    public void InitializeLoadingScreen()
    {
        UnityEngine.Random.InitState(DateTime.Now.Month + DateTime.Now.Day * DateTime.Now.Minute * DateTime.Now.Second * DateTime.Now.Millisecond);
        RandomLogic.FromArray(LoadScreens, out currentIndex);

        splashArt.sprite = LoadScreens[currentIndex].SplashArt;
        titleTMP.text = LoadScreens[currentIndex].Title;
        descTMP.text = LoadScreens[currentIndex].Desc;
    }

    public Sequence ToggleScreen(bool visible)
    {
        Sequence sequence = DOTween.Sequence();

        Tween tween = transitionScreen.DOFade(1f, transitionDelay);
        tween.onComplete += () =>
        {
            ToggleLoadingScreen(visible);
            if (visible)
            {
                InitializeLoadingScreen();
                loadingSlider.value = 0f;
            }

            OnFadeInCompleted?.Invoke(this, EventArgs.Empty);
        };

        sequence.Append(tween);
        sequence.Append(transitionScreen.DOFade(0f, transitionDelay)).onComplete += () => OnFadeOutCompleted?.Invoke(this, EventArgs.Empty);
        return sequence;
    }

    public TweenerCore<float, float, FloatOptions> FadeIn()
    {
        return transitionScreen.DOFade(1f, transitionDelay);
    }

    public TweenerCore<float, float, FloatOptions> FadeOut()
    {
        return transitionScreen.DOFade(0f, transitionDelay);
    }

    public void ToggleLoadingScreen(bool visible)
    {
        loadingScreenParent.gameObject.SetActive(visible);
    }

    public void SetLoadingProgress(float amount, string loadingText = "")
    {
        loadingSlider.DOValue(amount, fillSmoothTime);
        loadingTMP.text = loadingText;
    }
}
