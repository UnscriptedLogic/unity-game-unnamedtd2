using System;
using System.Collections.Generic;
using UnityEngine;

public class TowerUpgradeHandler : MonoBehaviour
{
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

        public Action<Tower> method;

        public UpgradeProperty(float damage = 0, float range = 0, float reloadTime = 0, float projSpeed = 0, float projLifetime = 0, int projPierce = 0, Action<Tower> method = null)
        {
            this.damage = damage;
            this.range = range;
            this.reloadTime = reloadTime;
            this.projSpeed = projSpeed;
            this.projLifetime = projLifetime;
            this.projPierce = projPierce;

            this.method = method;
        }

        public UpgradeProperty(Tower towerBase)
        {
            damage = towerBase.Damage;
            range = towerBase.Range;
            reloadTime = towerBase.ReloadTime;
            projSpeed = towerBase.ProjectileSpeed;
            projLifetime = towerBase.ProjectileLifetime;
            projPierce = towerBase.ProjectilePierce;
        }

        public void StackProperty(UpgradeProperty upgradeProperty)
        {
            damage += upgradeProperty.damage;
            range += upgradeProperty.range;
            reloadTime += upgradeProperty.reloadTime;
            projSpeed += upgradeProperty.projSpeed;
            projLifetime += upgradeProperty.projLifetime;
            projPierce += upgradeProperty.projPierce;
            method += upgradeProperty.method;
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
    protected Tower towerBase;
    protected List<int> upgradesChosen = new List<int>();
    protected UpgradeProperty persistantProperty;

    public List<int> UpgradesChosen => upgradesChosen;
    public TowerSO TowerSO => towerSO;

    public Action<Tower> OnTowerBaseReplaced;

    protected virtual void Start()
    {
        towerBase = GetComponent<Tower>();
        InitUpgrades(towerBase);
        persistantProperty = new UpgradeProperty(towerBase);
    }

    protected virtual void InitUpgrades(Tower towerBase)
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

                    towerBase = tower.GetComponent<Tower>();
                    OnTowerBaseReplaced?.Invoke(towerBase);
                }

                UpgradeProperty upgradeProperty = upgradeGroups[upgradesChosen.Count].upgradeProperties[upgradePropertyIndex];
                persistantProperty.StackProperty(upgradeProperty);

                //Core settings
                towerBase.Damage = persistantProperty.damage;
                towerBase.Range = persistantProperty.range;
                towerBase.ReloadTime = persistantProperty.reloadTime;

                //Projectile settings
                towerBase.ProjectilePierce = persistantProperty.projPierce;
                towerBase.ProjectileSpeed = persistantProperty.projSpeed;
                towerBase.ProjectileLifetime = persistantProperty.projLifetime;

                upgradeGroups[upgradesChosen.Count].upgradeProperties[upgradePropertyIndex].method?.Invoke(towerBase);

                //persistantProperty.method?.Invoke(towerBase);

                upgradesChosen.Add(upgradePropertyIndex);
            }
        }
    }
}