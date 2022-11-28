using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace UserInterfaceManagement
{
    public class TowerButton : MonoBehaviour
    {
        [SerializeField] private Sprite towerIcon;
        [SerializeField] private Sprite bgSprite;
        [SerializeField] private Sprite borderSprite;
        [SerializeField] private TextMeshProUGUI costText;
        [SerializeField] private Button button;

        public Sprite TowerIcon => towerIcon;
        public Sprite BGsprite => bgSprite;
        public Sprite BorderSprite => borderSprite;
        public string CostText { get => costText.text; set { costText.text = value; } }
        public Button BuyButton => button;
    }
}