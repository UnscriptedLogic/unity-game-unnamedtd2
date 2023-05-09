using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TowerBuyButton : MonoBehaviour
{
    [SerializeField] private Image iconImg;
    [SerializeField] private TextMeshProUGUI costTMP;
    [SerializeField] private Button towerBtn;

    public Button TowerBtn => towerBtn;

    public void Initialize(float cost, Sprite icon, Action onClick)
    {
        costTMP.text = $"${cost}";
        iconImg.sprite = icon;

        towerBtn.onClick.AddListener(() => { onClick(); });
    }
}
