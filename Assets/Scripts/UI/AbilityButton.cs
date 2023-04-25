using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
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

    public Button LevelUpButton => levelUpButton;

    private Ability ability;
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

        SetLevelUpButtonActive(ModifyType.Add, 0f, 0f);
        levelHandler.PointsHandler.OnModified += SetLevelUpButtonActive;

        Hide();
    }

    public void SetLevelUpButtonActive(ModifyType type, float current, float amount)
    {
        if (type == ModifyType.Add)
        {
            if (ability.CurrentLevel == ability.MaxLevel)
            {
                levelUpButton.gameObject.SetActive(false);
                return;
            }

            levelUpButton.gameObject.SetActive(levelHandler.Level >= ability.NextLevel && !(levelHandler.PointsHandler.Current <= 0f));

            levelUpButton.onClick.AddListener(() =>
            {
                ability.LevelUp();
                levelHandler.PointsHandler.Modify(ModifyType.Subtract, 1);
            });

        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        headerTMP.text = $"{abilityInfo.Name} [{abilityInfo.AbilityType}]";
        descriptionTMP.text = abilityInfo.Description;

        loreDescriptionTMP.gameObject.SetActive(abilityInfo.LoreDescription != null);
        loreDescriptionTMP.text = abilityInfo.LoreDescription;

        Show();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        Hide();
    }

    private void Show()
    {
        tooltipObj.SetActive(true);
    }

    private void Hide()
    {
        tooltipObj.SetActive(false);
    }

    private void OnDestroy()
    {
        levelHandler.PointsHandler.OnModified -= SetLevelUpButtonActive;
    }
}
