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
}
