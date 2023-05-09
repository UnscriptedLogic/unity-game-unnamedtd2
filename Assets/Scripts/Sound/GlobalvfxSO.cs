using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct FXPair
{
    public AudioSettings audioSettings;
    public EffectSettings effectSettings;

    public AudioSettings AudioSettings => audioSettings;
    public EffectSettings EffectSettings => effectSettings;
}

[CreateAssetMenu(fileName = "New Global Sound List", menuName = "ScriptableObjects/Create New Global Sound List")]
public class GlobalvfxSO : ScriptableObject
{
    [SerializeField] private FXPair levelUp;
    [SerializeField] private FXPair crit;

    public FXPair LevelUp => levelUp;
    public FXPair Crit => crit;
}
