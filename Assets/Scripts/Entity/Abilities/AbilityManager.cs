using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;

[DefaultExecutionOrder(1)]
public class AbilityManager : MonoBehaviour
{
    [SerializeField] private AbilitiesSO towerAbilities;
    [SerializeField] private AbilitiesSO unitAbilities;

    private Dictionary<AbilityInfo, Ability> allUsableAbilities = new Dictionary<AbilityInfo, Ability>();
    private Dictionary<AbilityInfo, UnitAbility> allUnitAbilities = new Dictionary<AbilityInfo, UnitAbility>();

    public static AbilityManager instance { get; private set; }

    private void Start()
    {
        instance = this;

        allUsableAbilities = new Dictionary<AbilityInfo, Ability>()
        {
            { towerAbilities.AbilityInfos[0], new CriticalHits() },
            { towerAbilities.AbilityInfos[1], new RelentlessStacks() },
            { towerAbilities.AbilityInfos[2], new DoubleTap() },
            { towerAbilities.AbilityInfos[3], new PowerShot() },
            { towerAbilities.AbilityInfos[4], new ProjectileMaintenance() },
            { towerAbilities.AbilityInfos[5], new SplitShot() },
            { towerAbilities.AbilityInfos[6], new TippedArrows() },
        };

        allUnitAbilities = new Dictionary<AbilityInfo, UnitAbility>()
        {
            { unitAbilities.AbilityInfos[0], new ComfortCone() }
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

    public AbilityInfo GetUnitAbilityInfoByAbility(UnitAbility abilitycompare)
    {
        for (int i = 0; i < allUnitAbilities.Count; i++)
        {
            if (allUnitAbilities.ElementAt(i).Value.GetType() == abilitycompare.GetType())
            {
                return allUnitAbilities.ElementAt(i).Key;
            }
        }

        return null;
    }
}