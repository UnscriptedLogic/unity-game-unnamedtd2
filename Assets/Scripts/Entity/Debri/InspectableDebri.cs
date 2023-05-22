using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InspectableDebri : MonoBehaviour, IInspectable
{
    [SerializeField] private string displayName;
    [SerializeField] private Sprite icon;

    public string DisplayName => displayName;
    public Sprite Icon => icon;
}
