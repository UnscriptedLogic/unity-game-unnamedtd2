using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using TowerManagement;
using System;

namespace UserInterfaceManagement
{
    public class TowerButton : MonoBehaviour
    {
        [SerializeField] private Image towerIcon;
        [SerializeField] private Image bgSprite;
        [SerializeField] private Image borderSprite;
        [SerializeField] private TextMeshProUGUI costText;
        [SerializeField] private Button button;

        public Button BuyButton => button;

        public void Initialize(TowerSO towerSO, Action OnButtonPressed)
        {
            towerIcon.sprite = towerSO.Icon;
            bgSprite.color = towerSO.BGColor;
            borderSprite.color = towerSO.BRColor;
            costText.text = towerSO.Cost.ToString();

            button.onClick.AddListener(() => OnButtonPressed());
        }
    }
}