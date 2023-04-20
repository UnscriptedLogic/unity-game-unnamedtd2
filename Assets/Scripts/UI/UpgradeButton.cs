using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

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

        StartCoroutine(ShowWithDelay());
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        Hide();
    }

    private IEnumerator ShowWithDelay()
    {
        yield return new WaitForSeconds(0.5f);
        Show();
    }

    private void Show()
    {
        tooltipObj.SetActive(true);
    }

    private void Hide()
    {
        StopAllCoroutines();
        tooltipObj.SetActive(false);
    }

}
