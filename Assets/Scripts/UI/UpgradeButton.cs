using GameManagement;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static TowerManagement.TowerSO;

namespace UserInterfaceManagement
{
    public class UpgradeButton : MonoBehaviour, IListensToCurrency, IModifiesCurrency
    {
        [SerializeField] private Image icon;
        [SerializeField] private TextMeshProUGUI desc;
        [SerializeField] private TextMeshProUGUI costTMP;
        [SerializeField] private Button button;

        private CurrencyManager currencyManager;
        private float cost;
        public Button UpgradeBtn => button;

        public void InitButton(UpgradeOption upgradeOption)
        {
            cost = upgradeOption.nextUpgradeCost;

            icon.sprite = upgradeOption.nextUpgradeIcon;
            desc.text = upgradeOption.nextUpgradeDesc;
            costTMP.text = $"${upgradeOption.nextUpgradeCost}";

            button.interactable = currencyManager.Cash >= cost;

            button.onClick.AddListener(() =>
            {
                currencyManager.ModifyCurrency(ModificationType.Subtract, cost);
            });
        }

        public void ModifyCash(CurrencyManager currencyManager)
        {
            this.currencyManager = currencyManager;
        }

        public void OnCurrencyChanged(ModificationType modificationType, float modificationAmount, float currentTotal)
        {
            button.interactable = currentTotal >= cost;
        }

        private void OnDisable()
        {
            button.onClick.RemoveAllListeners();
        }
    }
}