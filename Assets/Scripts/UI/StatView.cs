using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class StatView : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI nameTMP;
    [SerializeField] private TextMeshProUGUI valueTMP;

    public TextMeshProUGUI NameTMP => nameTMP;
    public TextMeshProUGUI ValueTMP => valueTMP;

    public void Initialized(string statName, string value)
    {
        nameTMP.text = statName;
        valueTMP.text = value;
    }
}
