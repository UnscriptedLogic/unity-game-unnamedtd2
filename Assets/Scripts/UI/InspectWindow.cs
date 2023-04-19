using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnscriptedLogic.Raycast;

public class InspectWindow : MonoBehaviour
{
    [Header("Inspect Window")]
    [SerializeField] private Transform openPos;
    [SerializeField] private Transform closePos;
    [SerializeField] private float tweenTime = 0.25f;
    [SerializeField] private LeanTweenType easeType;
    [SerializeField] private LayerMask unitLayer;
    [SerializeField] private LayerMask towerLayer;

    [Header("Stats")]
    [SerializeField] private GameObject statPrefab;
    [SerializeField] private Transform statParent;

    [Header("Avatar Section")]
    [SerializeField] private Image icon;
    [SerializeField] private TextMeshProUGUI nameTMP;
    [SerializeField] private Image levelSlider;
    [SerializeField] private TextMeshProUGUI levelTMP;

    [Header("Ability Section")]
    [SerializeField] private GameObject abilityButtonPrefab;
    [SerializeField] private Transform abilityParent;

    private List<AbilityButton> abilityButtons;

    [Header("Upgrade Section")]
    [SerializeField] private GameObject upgradeButtonPrefab;
    [SerializeField] private Transform upgradeParent;

    private List<UpgradeButton> upgradeButtons;

    //Others
    private InputManager inputManager;
    private bool isOpen = true;
    private GameObject inspectedObject;

    private void Start()
    {
        Hide();

        inputManager = InputManager.instance;

        inputManager.OnMouseDown += OnMouseDown;
    }

    private void OnMouseDown(Vector2 mousePos, bool isOverUI)
    {
        if (isOverUI) return;

        if (RaycastLogic.FromMousePos3D(Camera.main, out RaycastHit unitHit, towerLayer))
        {
            IInspectable inspectable = unitHit.collider.gameObject.GetComponent<IInspectable>();
            if (inspectable != null)
            {
                Tower tower = inspectable as Tower;
                TowerSO towerSO = TowerDefenseManager.instance.AllTowerList.GetSOFromTower(tower);

                DisplayTowerAvatar(towerSO);

                DisplayTowerStat(tower);

                inspectedObject = unitHit.collider.gameObject;

                Show();
                return;
            }
        }

        Hide();
    }

    private void DisplayTowerAvatar(TowerSO towerSO)
    {
        icon.sprite = towerSO.IconSpr;
        nameTMP.text = towerSO.TowerName;
    }

    private void DisplayTowerStat(Tower tower)
    {
        float damage = tower.Damage;
        float range = tower.Range;
        float rate = 60f / tower.ReloadTime / 60f;

        Clear(statParent);

        GameObject attStat = Instantiate(statPrefab, statParent);
        attStat.GetComponent<StatView>().Initialized("DMG", damage.ToString());

        GameObject rangeStat = Instantiate(statPrefab, statParent);
        rangeStat.GetComponent<StatView>().Initialized("RNG", range.ToString());

        GameObject rateStat = Instantiate(statPrefab, statParent);
        rateStat.GetComponent<StatView>().Initialized("RATE", $"{rate}/s");
    }

    private void Clear(Transform parent)
    {
        for (int i = parent.childCount - 1; i >= 0; i--)
        {
            Destroy(parent.GetChild(i).gameObject);
        }
    }

    private void Show() 
    {
        if (isOpen) return;

        LeanTween.move(gameObject, openPos.position, tweenTime).setEase(easeType).setOnComplete(() => transform.position = openPos.position); 
        isOpen = true;
    }
    
    private void Hide()
    {
        if (!isOpen) return;

        LeanTween.move(gameObject, closePos.position, tweenTime).setOnComplete(() =>
        {
            Clear(statParent);
        });

        isOpen = false;
    }
}
