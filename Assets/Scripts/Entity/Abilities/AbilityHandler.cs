using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnscriptedLogic.Currency;
using UnscriptedLogic.MathUtils;

public class Ability
{
    protected AbilityHandler abilityHandler;
    protected Tower tower;
    protected CurrencyHandler levelHandler;

    protected int maxLevel;
    protected int[] levelRequirements;

    public CurrencyHandler LevelHandler => levelHandler;
    public int CurrentLevel => (int)levelHandler.Current;
    public int MaxLevel => maxLevel;
    public int[] LevelRequirements => levelRequirements;
    public int NextLevel => levelRequirements[CurrentLevel - 1];

    public void Initialize(AbilityHandler abilityHandler, Tower tower)
    {
        this.abilityHandler = abilityHandler;
        this.tower = tower;

        tower.OnTowerProjectileCreated += OnProjectileCreated;
        tower.OnTowerProjectileFired += OnProjectileFired;
        tower.OnTowerProjectileHit += OnProjectileHit;
        tower.OnTowerProjectileDestroyed += OnProjectileDestroyed;
        tower.OnTowerTargetFound += OnTargetFound;
        tower.WhileTowerTargetFound += WhileTargetFound;
        tower.OnTowerTargetLost += OnTargetLost;

        OnAdded();
    }

    public void LevelUp()
    {
        levelHandler.Modify(ModifyType.Add, 1);
        OnLevelUp();
    }

    public virtual void OnAdded() { }
    public virtual void Update() { }
    public virtual void FixedUpdate() { }
    protected virtual void OnTargetFound(Transform target) { }
    protected virtual void WhileTargetFound(Transform target) { }
    protected virtual void OnTargetLost() { }
    protected virtual void OnProjectileCreated(GameObject bulletObject, ProjectileBase projectileScript) { }
    protected virtual void OnProjectileFired() { }
    protected virtual void OnProjectileHit(UnitBase unitScript, ProjectileBase projectileScript, Action<UnitBase, float> damageMethod) { }
    protected virtual void OnProjectileDestroyed(ProjectileBase projectileScript) { }
    protected virtual void OnLevelUp() { }
}

public class AbilityHandler : MonoBehaviour
{
    [SerializeField] private List<Ability> abilities;

    public List<Ability> Abilities => abilities;

    private void Start()
    {
        abilities = new List<Ability>();
    }

    public void AddAbility(Ability ability, Tower tower)
    {
        ability.Initialize(this, tower);
        abilities.Add(ability);
    }

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
}
