using System;
using System.Collections.Generic;
using UnityEngine;
using static TowerUpgradeHandler;

[DefaultExecutionOrder(2)]
public class TowerUpgradeHandler : MonoBehaviour
{
    [System.Serializable]
    public class UpgradeProperty
    {
        //Core settings
        public float damage { get; private set; }
        public float range { get; private set; }
        public float reloadTime { get; private set; }

        //Projectile settings
        public float projSpeed { get; private set; }
        public float projLifetime { get; private set; }
        public int projPierce { get; private set; }

        public Action<TowerBase> method;

        public UpgradeProperty(float damage = 0, float range = 0, float reloadTime = 0, float projSpeed = 0, float projLifetime = 0, int projPierce = 0, Action<TowerBase> method = null)
        {
            this.damage = damage;
            this.range = range;
            this.reloadTime = reloadTime;
            this.projSpeed = projSpeed;
            this.projLifetime = projLifetime;
            this.projPierce = projPierce;

            this.method = method;
        }

        public UpgradeProperty(TowerBase towerBase)
        {
            //damage = towerBase.Damage;
            //range = towerBase.Range;
            //reloadTime = towerBase.ReloadTime;
            //projSpeed = towerBase.ProjectileSpeed;
            //projLifetime = towerBase.ProjectileLifetime;
            //projPierce = towerBase.ProjectilePierce;
        }

        public static UpgradeProperty operator +(UpgradeProperty a, UpgradeProperty b)
        {
            UpgradeProperty sum = new UpgradeProperty();

            sum.damage = a.damage + b.damage;
            sum.range = a.range + b.range;
            sum.reloadTime = a.reloadTime + b.reloadTime;
            sum.projSpeed = a.projSpeed + b.projSpeed;
            sum.projLifetime = a.projLifetime + b.projLifetime;
            sum.projPierce = a.projPierce + b.projPierce;
            sum.method = a.method + b.method;

            return sum;
        }

        public static UpgradeProperty operator -(UpgradeProperty a, UpgradeProperty b)
        {
            UpgradeProperty sum = a;

            sum.damage -= b.damage;
            sum.range -= b.range;
            sum.reloadTime -= b.reloadTime;
            sum.projSpeed -= b.projSpeed;
            sum.projLifetime -= b.projLifetime;
            sum.projPierce -= b.projPierce;
            sum.method -= b.method;

            return sum;
        }
    }

    public class UpgradeGroup
    {
        public List<UpgradeProperty> upgradeProperties;

        public UpgradeGroup()
        {
            upgradeProperties = new List<UpgradeProperty>();
        }
    }

    [SerializeField] protected TowerSO towerSO;
    protected List<UpgradeGroup> upgradeGroups = new List<UpgradeGroup>();
    protected TowerBase towerBase;
    protected List<int> upgradesChosen = new List<int>();

    //Sum of instant upgrades
    protected UpgradeProperty upgradePersistantProperty;

    //Sum of all ability affected upgrades
    protected UpgradeProperty abilityPersistentProperty;

    public List<int> UpgradesChosen => upgradesChosen;
    public TowerSO TowerSO => towerSO;

    public Action<TowerBase> OnTowerBaseReplaced;

    protected virtual void Start()
    {
        towerBase = GetComponent<TowerBase>();
        InitUpgrades(towerBase);
        upgradePersistantProperty = new UpgradeProperty(towerBase);
        abilityPersistentProperty = new UpgradeProperty();
    }

    protected virtual void InitUpgrades(TowerBase towerBase)
    {

    }

    public void UpgradeTower(int upgradePropertyIndex)
    {
        if (upgradesChosen.Count < upgradeGroups.Count)
        {
            if (upgradePropertyIndex < upgradeGroups[upgradesChosen.Count].upgradeProperties.Count)
            {
                GameObject overrideObject = towerSO.TowerLevels[upgradesChosen.Count].upgradeOptions[upgradePropertyIndex].OverrideObject;
                if (overrideObject != null)
                {
                    Destroy(towerBase.gameObject);
                    GameObject instantiatedOverride = Instantiate(overrideObject, transform);
                    Transform tower = instantiatedOverride.transform.GetChild(0);
                    tower.SetParent(transform);
                    Destroy(instantiatedOverride);

                    towerBase = tower.GetComponent<TowerBase>();
                    OnTowerBaseReplaced?.Invoke(towerBase);
                }

                upgradePersistantProperty += upgradeGroups[upgradesChosen.Count].upgradeProperties[upgradePropertyIndex];
                ApplyStats();

                upgradeGroups[upgradesChosen.Count].upgradeProperties[upgradePropertyIndex].method?.Invoke(towerBase);
                upgradesChosen.Add(upgradePropertyIndex);
            }
        }
    }

    private void ApplyStats()
    {
        UpgradeProperty propertySum = new UpgradeProperty();
        propertySum = upgradePersistantProperty + abilityPersistentProperty;

        //Core settings
        //towerBase.Damage = propertySum.damage;
        //towerBase.Range = propertySum.range;
        //towerBase.ReloadTime = propertySum.reloadTime;

        //Projectile settings
        //towerBase.ProjectilePierce = propertySum.projPierce;
        //towerBase.ProjectileSpeed = propertySum.projSpeed;
        //towerBase.ProjectileLifetime = propertySum.projLifetime;
    }

    /// <summary>
    /// Updates a seperate stat group property meant for towerAbilities.
    /// It stores the cumulation of all the upgrades affecting attributes of the tower ignoring any status effects.
    /// Abilities should not directly affect the main persistent property.
    /// Enemies who want to disable passives also have easy access 
    /// </summary>
    public void UpdatePersistentStats(UpgradeProperty upgradeProperty)
    {
        abilityPersistentProperty += upgradeProperty;
        ApplyStats();
    }
}