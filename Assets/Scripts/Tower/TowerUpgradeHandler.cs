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

            public UpgradeProperty(float damage = 0, float range = 0, float reloadTime = 0, float projSpeed = 0, float projLifetime = 0)
            {
                this.damage = damage;
                this.range = range;
                this.reloadTime = reloadTime;
                this.projSpeed = projSpeed;
                this.projLifetime = projLifetime;
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

        public List<int> UpgradesChosen => upgradesChosen;
        public TowerSO TowerSO => towerSO;

        protected virtual void Start()
        {
            towerBase = GetComponentInChildren<TowerBase>();
            InitUpgrades();
        }

        protected virtual void InitUpgrades()
        {

        }

        public void UpgradeTower(int upgradePropertyIndex)
        {
            if (upgradesChosen.Count < upgradeGroups.Count)
            {
                if (upgradePropertyIndex < upgradeGroups[upgradesChosen.Count].upgradeProperties.Count)
                {
                    UpgradeProperty upgradeProperty = upgradeGroups[upgradesChosen.Count].upgradeProperties[upgradePropertyIndex];

                    //Core settings
                    towerBase.damage += upgradeProperty.damage;
                    towerBase.range += upgradeProperty.range;
                    towerBase.reloadTime += upgradeProperty.reloadTime;

                    //Projectile settings
                    towerBase.projectileSpeed += upgradeProperty.projSpeed;
                    towerBase.projectileLifetime += upgradeProperty.projLifetime;

                    upgradesChosen.Add(upgradePropertyIndex);
                }
            }
        }
    }
}