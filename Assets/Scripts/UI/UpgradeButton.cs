using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UserInterfaceManagement
{
    public class UpgradeButton : MonoBehaviour
    {
        [SerializeField] private Image icon;
        [SerializeField] private TextMeshProUGUI desc;
        [SerializeField] private TextMeshProUGUI cost;

        public void InitButton(Sprite sprite, string description, float cost)
        {
            icon.sprite = sprite;
            desc.text = description;
            this.cost.text = $"${cost}";
        }
    }
}