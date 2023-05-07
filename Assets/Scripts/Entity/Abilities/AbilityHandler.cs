using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Playables;
using UnityEngine;
using UnscriptedLogic.Currency;
using UnscriptedLogic.MathUtils;

public class OnAnyAbilityHasUpgradeEventArgs : EventArgs
{
    public TowerBase tower;
}

public struct AbilityInitSettings
{
    public AbilityHandler abilityHandler;
    public TowerBase towerBase;
    public TowerUpgradeHandler upgradeHandler;
    public TowerLevelHandler towerLevelHandler;
}

public class Ability
{
    protected TowerLevelHandler towerLevelHandler;
    protected TowerUpgradeHandler upgradeHandler;
    protected AbilityHandler abilityHandler;
    protected CurrencyHandler levelHandler;
    protected TowerBase tower;

    protected int maxLevel;
    protected int[] levelRequirements;

    public CurrencyHandler LevelHandler => levelHandler;
    public int CurrentLevel => (int)levelHandler.Current;
    public int MaxLevel => maxLevel;
    public int[] LevelRequirements => levelRequirements;
    public int NextLevel => levelRequirements[CurrentLevel - 1];

    public static event EventHandler<OnAnyAbilityHasUpgradeEventArgs> OnAnyAbilityHasUpgrade;

    public void Initialize(AbilityInitSettings initSettings)
    {
        abilityHandler = initSettings.abilityHandler;
        tower = initSettings.towerBase;
        upgradeHandler = initSettings.upgradeHandler;
        towerLevelHandler= initSettings.towerLevelHandler;

        tower.OnTowerProjectileCreated += OnProjectileCreated;
        tower.OnTowerProjectileFired += OnProjectileFired;
        tower.OnProjectileHitEvent += OnProjectileHit;
        tower.OnTowerProjectileDestroyed += OnProjectileDestroyed;
        tower.OnTowerTargetFound += OnTargetFound;
        tower.WhileTowerTargetFound += WhileTargetFound;
        tower.OnTowerTargetLost += OnTargetLost;

        towerLevelHandler.ExperienceHandler.OnModified += ExperienceHandler_OnModified;

        OnAdded();
    }

    private void ExperienceHandler_OnModified(object sender, CurrencyEventArgs e)
    {
        //Checks if the current ability can be upgraded
        if (towerLevelHandler.Level + 1 >= NextLevel && !(towerLevelHandler.PointsHandler.Current <= 0f))
        {
            OnAnyAbilityHasUpgrade?.Invoke(this, new OnAnyAbilityHasUpgradeEventArgs()
            {
                tower = tower
            });
        }
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
    protected virtual void OnProjectileHit(object sender, OnProjectileHitEventArgs eventArgs) { }
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

        Ability.OnAnyAbilityHasUpgrade += Ability_OnAnyAbilityHasUpgrade;
    }

    private void Ability_OnAnyAbilityHasUpgrade(object sender, OnAnyAbilityHasUpgradeEventArgs e)
    {
        FXManager.instance.PlayGlobalEffect();
    }

    public void AddAbility(Ability ability, TowerBase tower)
    {
        AbilityInitSettings settings = new AbilityInitSettings()
        {
            abilityHandler = this,
            towerBase = tower,
            upgradeHandler = tower.GetComponent<TowerUpgradeHandler>(),
            towerLevelHandler = tower.GetComponent<TowerLevelHandler>()
        };

        ability.Initialize(settings);
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
