using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConeheadAbilities : MonoBehaviour
{
    private UnitBase unit;
    private UnitAbilityHandler abilityHandler;

    private void Start()
    {
        unit = GetComponent<UnitBase>();
        abilityHandler = GetComponent<UnitAbilityHandler>();

        abilityHandler.AddAbility(new ComfortCone(), unit);
    }
}
