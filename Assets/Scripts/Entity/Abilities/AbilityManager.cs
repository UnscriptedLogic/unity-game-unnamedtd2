using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;

[DefaultExecutionOrder(1)]
public class AbilityManager : MonoBehaviour
{
    [SerializeField] private AbilitiesSO abilities;
    private Dictionary<AbilityInfo, Ability> allUsableAbilities;

    public static AbilityManager instance { get; private set; }

    private void Awake()
    {
        instance = this;

        allUsableAbilities = new Dictionary<AbilityInfo, Ability>()
        {
            { abilities.AbilityInfos[0], new CriticalHits() },
            { abilities.AbilityInfos[1], new RelentlessStacks() },
            { abilities.AbilityInfos[2], new DoubleTap() },
            { abilities.AbilityInfos[3], new PowerShot() },
            { abilities.AbilityInfos[4], new ProjectileMaintenance() },
            { abilities.AbilityInfos[5], new SplitShot() },
            { abilities.AbilityInfos[6], new TippedArrows() },
        };
    }

    public Ability GetAbilityByName(string abilityName)
    {
        for (int i = 0; i < allUsableAbilities.Count; i++)
        {
            if (allUsableAbilities.ElementAt(i).Key.Name == abilityName)
            {
                return allUsableAbilities.ElementAt(i).Value;
            }
        }

        return null;
    }

    public AbilityInfo GetAbilityInfoByName(string abilityName)
    {
        for (int i = 0; i < allUsableAbilities.Count; i++)
        {
            if (allUsableAbilities.ElementAt(i).Key.Name == abilityName)
            {
                return allUsableAbilities.ElementAt(i).Key;
            }
        }

        return null;
    }

    public AbilityInfo GetAbilityInfoByAbility(Ability abilitycompare)
    {
        for (int i = 0; i < allUsableAbilities.Count; i++)
        {
            if (allUsableAbilities.ElementAt(i).Value.GetType() == abilitycompare.GetType())
            {
                return allUsableAbilities.ElementAt(i).Key;
            }
        }

        return null;
    }
}