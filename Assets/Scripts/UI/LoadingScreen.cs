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
        sequence.SetUpdate(true);

        Tween fadeInTween = FadeIn();
        fadeInTween.onComplete += () =>
        {
            ToggleLoadingScreen(visible);
            if (visible)
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

    public void ToggleButton(bool value)
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

    public void ToggleLoadingScreen(bool visible)
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
        ToggleButton(true);

        proceedBtn.onClick.AddListener(() =>
        {
            ToggleScreen(false);
            Time.timeScale = 1f;
        });
    }
}
