using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnscriptedLogic.MathUtils;

[System.Serializable]
public struct FXPair
{
    public AudioSettings audioSettings;
    public EffectSettings effectSettings;

    public AudioSettings AudioSettings => audioSettings;
    public EffectSettings EffectSettings => effectSettings;
}

[System.Serializable]
public struct ThemeFX
{
    public FXPair mainscreenTheme;
    public FXPair proceedClick;
    public FXPair gameAtmosphere;
}

[CreateAssetMenu(fileName = "New Global Sound List", menuName = "ScriptableObjects/Create New Global Sound List")]
public class GlobalvfxSO : ScriptableObject
{
    [Header("Theme Related")]
    [SerializeField] private ThemeFX ruins;

    [Header("Tower Related")]
    [SerializeField] private FXPair levelUp;
    [SerializeField] private FXPair crit;
    [SerializeField] private FXPair towerPlace;
    [SerializeField] private FXPair[] upgradeBoughts;

    public ThemeFX RuinsTheme => ruins;

    public FXPair LevelUp => levelUp;
    public FXPair Crit => crit;
    public FXPair TowerPlaced => towerPlace;
    
    public FXPair[] UpgradeBoughts => upgradeBoughts;
    public FXPair RandomUpgradeBought => RandomLogic.FromArray(upgradeBoughts);
}
