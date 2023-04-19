using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AbilityButton : MonoBehaviour
{
    [Header("Levelling Up")]
    [SerializeField] private Button levelUpButton;

    [Header("Base")]
    [SerializeField] private Image iconImg;
    [SerializeField] private Image borderImg;

    [Header("Level Knobs")]
    [SerializeField] private GameObject levelKnobPrefab;
    [SerializeField] private Transform levelKnobParent;
}
