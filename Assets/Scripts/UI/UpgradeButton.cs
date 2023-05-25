using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnscriptedLogic.MathUtils;

public class UpgradeButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [Header("Basic Settings")]
    [SerializeField] private Image iconImg;
    [SerializeField] private Image bgImg;
    [SerializeField] private Image borderImg;
    [SerializeField] private TextMeshProUGUI costTMP;
    [SerializeField] private Button upgradeBtn;

    [Header("ToolTip")]
    [SerializeField] private GameObject tooltipObj;
    [SerializeField] private TextMeshProUGUI headerTMP;
    [SerializeField] private TextMeshProUGUI descriptionTMP;
    [SerializeField] private TextMeshProUGUI loreDescriptionTMP;

    private UpgradeOption upgradeOption;

    public Button UpgradeBtn => upgradeBtn;

    public static event EventHandler<bool> OnAnyUpgradeButtonClicked;

    public void Initalize(UpgradeOption upgradeOption)
    {
        Hide();

        this.upgradeOption = upgradeOption;

        iconImg.sprite = upgradeOption.IconSprite;
        costTMP.text = $"${upgradeOption.Cost}";
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        headerTMP.text = upgradeOption.Name;
        descriptionTMP.text = upgradeOption.Description;
        loreDescriptionTMP.text = upgradeOption.LoreDescription;

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

    public void InvokeEvent(bool value)
    {
        OnAnyUpgradeButtonClicked?.Invoke(this, value);
    }
}
