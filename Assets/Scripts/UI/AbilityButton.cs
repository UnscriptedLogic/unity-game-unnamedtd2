using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnscriptedLogic.Currency;
using UnscriptedLogic.MathUtils;

public class AbilityButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [Header("Levelling Up")]
    [SerializeField] private Button levelUpButton;

    [Header("Base")]
    [SerializeField] private Image iconImg;
    [SerializeField] private Image borderImg;

    [Header("Level Knobs")]
    [SerializeField] private Color obtainedLevelColor;
    [SerializeField] private Color unobtainedLevelColor;
    [SerializeField] private GameObject levelKnobPrefab;
    [SerializeField] private Transform levelKnobParent;

    [Header("ToolTip")]
    [SerializeField] private GameObject tooltipObj;
    [SerializeField] private TextMeshProUGUI headerTMP;
    [SerializeField] private TextMeshProUGUI descriptionTMP;
    [SerializeField] private TextMeshProUGUI loreDescriptionTMP;

    [Header("Cooldown")]
    [SerializeField] private Transform cooldownParent;
    [SerializeField] private TextMeshProUGUI cooldownTMP;
    [SerializeField] private Image cooldownFill;

    public Button LevelUpButton => levelUpButton;

    private Ability ability;
    private UnitAbility unitAbility;
    private AbilityInfo abilityInfo;
    private TowerLevelHandler levelHandler;

    public void Initialize(AbilityInfo abilityInfo, Ability ability, TowerLevelHandler levelHandler)
    {
        this.ability = ability;
        this.abilityInfo = abilityInfo;
        this.levelHandler = levelHandler;

        iconImg.sprite = abilityInfo.IconSpr;

        for (int i = 0; i < ability.MaxLevel; i++)
        {
            Color color = i < ability.CurrentLevel ? obtainedLevelColor : unobtainedLevelColor;

            GameObject levelKnob = Instantiate(levelKnobPrefab, levelKnobParent);
            levelKnob.GetComponent<Image>().color = color;
        }

        SetLevelUpButtonActive(null, new CurrencyEventArgs());
        levelHandler.PointsHandler.OnModified += SetLevelUpButtonActive;

        levelUpButton.onClick.AddListener(() =>
        {
            ability.LevelUp();
            levelHandler.PointsHandler.Modify(ModifyType.Subtract, 1);
        });

        HideTooltip();
        SetCooldown(0f, 1f);
    }

    public void Initialize(AbilityInfo abilityInfo, UnitAbility unitAbility)
    {
        this.unitAbility = unitAbility;
        this.abilityInfo = abilityInfo;

        iconImg.sprite = abilityInfo.IconSpr;

        levelUpButton.gameObject.SetActive(false);
        
        
        HideTooltip();
        SetCooldown(0f, 1f);
    }

    public void SetLevelUpButtonActive(object sender, CurrencyEventArgs e)
    {
        if (e.modifyType == ModifyType.Add)
        {
            if (ability.CurrentLevel == ability.MaxLevel)
            {
                levelUpButton.gameObject.SetActive(false);
                return;
            }

            levelUpButton.gameObject.SetActive(levelHandler.Level + 1 >= ability.NextLevel && levelHandler.PointsHandler.Current > 0f);
        }
    }

    public void SetCooldown(float value, float maxValue)
    {
        cooldownParent.gameObject.SetActive(value > 0f);

        cooldownFill.fillAmount = value / maxValue;
        cooldownTMP.text = $"{Mathf.RoundToInt(value)}";
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        headerTMP.text = $"{abilityInfo.Name} [{abilityInfo.AbilityType}]";
        descriptionTMP.text = abilityInfo.Description;

        loreDescriptionTMP.gameObject.SetActive(abilityInfo.LoreDescription != null);
        loreDescriptionTMP.text = abilityInfo.LoreDescription;

        ShowTooltip();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        HideTooltip();
    }

    private void ShowTooltip()
    {
        tooltipObj.SetActive(true);
    }

    private void HideTooltip()
    {
        tooltipObj.SetActive(false);
    }

    private void OnDestroy()
    {
        if (levelHandler != null)
            levelHandler.PointsHandler.OnModified -= SetLevelUpButtonActive;
    }
}
