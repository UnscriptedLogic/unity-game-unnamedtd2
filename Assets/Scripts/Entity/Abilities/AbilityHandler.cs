using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Playables;
using UnityEngine;
using UnscriptedLogic.Currency;
using UnscriptedLogic.MathUtils;

public class OnAnyAbilityEventArgs : EventArgs
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
    protected bool called;

    public CurrencyHandler LevelHandler => levelHandler;
    public int CurrentLevel => (int)levelHandler.Current;
    public int MaxLevel => maxLevel;
    public int[] LevelRequirements => levelRequirements;
    public int NextLevel => levelRequirements[CurrentLevel - 1];

    public static event EventHandler<OnAnyAbilityEventArgs> OnAnyAbilityHasUpgrade;
    public static event EventHandler<OnAnyAbilityEventArgs> OnAnyAbilityUpgraded;

    public void Initialize(AbilityInitSettings initSettings)
    {
        abilityHandler = initSettings.abilityHandler;
        tower = initSettings.towerBase;
        upgradeHandler = initSettings.upgradeHandler;
        towerLevelHandler = initSettings.towerLevelHandler;

        tower.OnTowerProjectileCreated += OnProjectileCreated;
        tower.OnTowerProjectileFired += OnProjectileFired;
        tower.OnProjectileHitEvent += OnProjectileHit;
        tower.OnTowerProjectileDestroyed += OnProjectileDestroyed;
        tower.OnTowerTargetFound += OnTargetFound;
        tower.WhileTowerTargetFound += WhileTargetFound;
        tower.OnTowerTargetLost += OnTargetLost;

        towerLevelHandler.OnLevelUp += TowerLevelHandler_OnLevelUp;
        OnAnyAbilityUpgraded += Ability_OnAnyAbilityUpgraded;

        OnAdded();
    }

    private void Ability_OnAnyAbilityUpgraded(object sender, OnAnyAbilityEventArgs e)
    {
        if (e.tower != tower) return;

        called = false;
    }

    private void TowerLevelHandler_OnLevelUp(object sender, EventArgs e)
    {
        //Checks if the current ability can be upgraded
        if (called) return;

        if (levelHandler.Current >= maxLevel) return;

        if (towerLevelHandler.Level + 1 >= NextLevel && towerLevelHandler.PointsHandler.Current > 0)
        {
            called = true;
            OnAnyAbilityHasUpgrade?.Invoke(this, new OnAnyAbilityEventArgs()
            {
                tower = tower
            });
        }
    }

    public void LevelUp()
    {
        levelHandler.Modify(ModifyType.Add, 1);
        OnLevelUp();
        OnAnyAbilityUpgraded?.Invoke(this, new OnAnyAbilityEventArgs() 
        { 
            tower = tower 
        });
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

    private FXManager fXManager;
    private TowerBase tower;
    private TowerLevelHandler towerLevelHandler;
    private GameObject particleGO;

    public List<Ability> Abilities => abilities;

    private void Start()
    {
        abilities = new List<Ability>();
        fXManager = FXManager.instance;
        tower = transform.GetComponent<TowerBase>();
        towerLevelHandler = transform.GetComponent<TowerLevelHandler>();
        Ability.OnAnyAbilityHasUpgrade += Ability_OnAnyAbilityHasUpgrade;
        Ability.OnAnyAbilityUpgraded += Ability_OnAnyAbilityUpgraded;

    }

    private void Ability_OnAnyAbilityUpgraded(object sender, OnAnyAbilityEventArgs e)
    {
        if (e.tower != tower) return;

        Destroy(particleGO);
        particleGO = null;

        //bool noMoreUpgrades = true;
        //if (towerLevelHandler.PointsHandler.Current > 0)
        //{
        //    for (int i = 0; i < abilities.Count; i++)
        //    {
        //        if (abilities[i].CurrentLevel < abilities[i].MaxLevel)
        //        {
        //            if (towerLevelHandler.Level + 1 >= abilities[i].NextLevel)
        //            {
        //                noMoreUpgrades = false;
        //                break;
        //            }
        //        }
        //    }
        //}

        //if (noMoreUpgrades)
        //{
        //    Destroy(particleGO);
        //    particleGO = null;
        //}
    }

    private void Ability_OnAnyAbilityHasUpgrade(object sender, OnAnyAbilityEventArgs e)
    {
        if (e.tower != tower) return;

        if (particleGO == null)
        {
            (GameObject soundGO, GameObject particleGO) = FXManager.instance.PlayGlobalEffect(fXManager.GlobalEffects.LevelUp, tower.transform.position, Quaternion.identity, Vector3.one);
            this.particleGO = particleGO;
        }
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
