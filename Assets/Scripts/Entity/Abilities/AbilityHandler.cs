using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum AbilityType
{
    Passive,
    Active
}

public class Ability
{
    public AbilityHandler abilityHandler;
    public Tower tower;
    
    protected string name;
    protected string description;
    protected string loreDescription;
    protected AbilityType type = AbilityType.Passive;
    
    public string Name => name;
    public string Description => description;
    public string LoreDescription => loreDescription;
    public AbilityType Type => type;

    public virtual void OnAdded()
    {
        //Code triggers when the ability is first added to the entity
    }

    public virtual void Update()
    {
        //Code triggers on Update() from the handler
    }

    public virtual void FixedUpdate()
    {
        //Code triggers on FixedUpdate from the handler
    }
}

public class AbilityHandler : MonoBehaviour
{
    [SerializeField] private List<Ability> abilities;

    public List<Ability> Abilities => abilities;

    private void Update()
    {
        for (int i = 0; i < abilities.Count; i++)
        {
            abilities[i].Update();
        }
    }

    private void FixedUpdate()
    {
        for (int i = 0; i < abilities.Count; i++)
        {
            abilities[i].FixedUpdate();
        }
    }

    public void AddAbility(Ability ability, Tower tower)
    {
        abilities.Add(ability);
        ability.abilityHandler = this;
        ability.OnAdded();
    }
}
