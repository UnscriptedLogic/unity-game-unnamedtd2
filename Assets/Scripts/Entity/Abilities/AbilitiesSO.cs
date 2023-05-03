using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public enum AbilityType
{
    PASSIVE,
    PASSIVECD,
    ACTIVE,
}

[System.Serializable]
public class AbilityInfo
{
    [SerializeField] private string name;
    [SerializeField] private Sprite iconSpr;
    [TextArea(3, 9)] [SerializeField] private string description;
    [TextArea(3, 9)] [SerializeField] private string loreDescription;
    [Space(10)] [SerializeField] private AbilityType abilityType = AbilityType.PASSIVE;

    public string Name => name;
    public Sprite IconSpr => iconSpr;
    public string Description => description;
    public string LoreDescription => loreDescription;
    public AbilityType AbilityType => abilityType;
}

[CreateAssetMenu(fileName = "New Ability List", menuName = "ScriptableObjects/Create New Ability List")]
public class AbilitiesSO : ScriptableObject
{
    [SerializeField] private List<AbilityInfo> abilityInfos;

    public List<AbilityInfo> AbilityInfos => abilityInfos;
}