using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using TowerManagement;
using System;
using GameManagement;

namespace UserInterfaceManagement
{
    public class TowerButton : MonoBehaviour, IListensToCurrency
    {
        [SerializeField] private Image towerIcon;
        [SerializeField] private Image bgSprite;
        [SerializeField] private Image borderSprite;
        [SerializeField] private TextMeshProUGUI costText;
        [SerializeField] private Button button;
        private TowerSO towerSO;

        public TowerSO TowerSO => towerSO;
        public Button BuyButton => button;

        public void Initialize(TowerSO towerSO, Action OnButtonPressed)
        {
            this.towerSO = towerSO;
            towerIcon.sprite = towerSO.Icon;
            bgSprite.color = towerSO.BGColor;
            borderSprite.color = towerSO.BRColor;
            costText.text = towerSO.Cost.ToString();

            button.onClick.AddListener(() => OnButtonPressed());
        }

        public void OnCurrencyChanged(ModificationType modificationType, float modAmount, float currentCash)
        {
            if (currentCash >= towerSO.Cost)
            {
                button.interactable = true;
            }
            else
            {
                button.interactable = false;
            }
        }
    }
}