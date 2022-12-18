using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TowerManagement
{
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
                damage = towerBase.damage;
                range = towerBase.range;
                reloadTime = towerBase.reloadTime;
                projSpeed = towerBase.projectileSpeed;
                projLifetime = towerBase.projectileLifetime;
                projPierce = towerBase.pierce;
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
        protected TowerBase towerBase;
        protected List<int> upgradesChosen = new List<int>();
        protected UpgradeProperty persistantProperty;

        public List<int> UpgradesChosen => upgradesChosen;
        public TowerSO TowerSO => towerSO;

        public Action<TowerBase> OnTowerBaseReplaced;

        protected virtual void Start()
        {
            towerBase = GetComponentInChildren<TowerBase>();
            InitUpgrades(towerBase);
            persistantProperty = new UpgradeProperty(towerBase);
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
                    GameObject overrideObject = towerSO.TowerLevels[upgradesChosen.Count].upgradeOptions[upgradePropertyIndex].overrideObject;
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

                    UpgradeProperty upgradeProperty = upgradeGroups[upgradesChosen.Count].upgradeProperties[upgradePropertyIndex];
                    persistantProperty.StackProperty(upgradeProperty);

                    //Core settings
                    towerBase.damage = persistantProperty.damage;
                    towerBase.range = persistantProperty.range;
                    towerBase.reloadTime = persistantProperty.reloadTime;

                    //Projectile settings
                    towerBase.projectileSpeed = persistantProperty.projSpeed;
                    towerBase.projectileLifetime = persistantProperty.projLifetime;

                    persistantProperty.method?.Invoke(towerBase);

                    upgradesChosen.Add(upgradePropertyIndex);
                }
            }
        }
    }
}