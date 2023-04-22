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
            { abilities.AbilityInfos[0], GetCriticalHits() },
            { abilities.AbilityInfos[1], GetRelentlessStacks() },
        };
    }

    public Ability GetCriticalHits() => new CriticalHits();
    public Ability GetRelentlessStacks() => new RelentlessStacks();

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