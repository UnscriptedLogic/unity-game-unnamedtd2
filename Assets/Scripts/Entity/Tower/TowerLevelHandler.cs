using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnscriptedLogic.Currency;
using UnscriptedLogic.MathUtils;

public class TowerLevelHandler : MonoBehaviour
{
    [Header("Level Settings")]
    [SerializeField] private int level;
    [SerializeField] private int upgradePoints;

    private TowerBase tower;
    private CurrencyHandler experienceHandler;
    private CurrencyHandler upgradePointsHandler;
    private ExperienceLevel currentExperienceLevel;

    public int Level => level;
    public int UpgradePoints => upgradePoints;
    public CurrencyHandler ExperienceHandler => experienceHandler;
    public CurrencyHandler PointsHandler => upgradePointsHandler;
    public ExperienceLevel ExperienceLevel => currentExperienceLevel;

    private void Start()
    {
        tower = GetComponent<TowerBase>();

        experienceHandler = new CurrencyHandler(0f);
        upgradePointsHandler = new CurrencyHandler(upgradePoints);

        currentExperienceLevel = TowerDefenseManager.instance.ExperienceLevelsSO.ExpList[level];

        tower.OnProjectileHitEvent += Tower_OnTowerProjectileHit;
    }

    private void Tower_OnTowerProjectileHit(object sender, OnProjectileHitEventArgs e)
    {
        experienceHandler.Modify(ModifyType.Add, tower.Damage);
        if (experienceHandler.Current >= currentExperienceLevel.amount)
            LevelUp();
    }

    private void LevelUp()
    {
        if (level + 1 < TowerDefenseManager.instance.ExperienceLevelsSO.ExpList.Count)
        {
            level++;
            upgradePointsHandler.Modify(ModifyType.Add, 1);
            experienceHandler.Modify(ModifyType.Set, 0);
            currentExperienceLevel = TowerDefenseManager.instance.ExperienceLevelsSO.ExpList[level];
            return;
        }
    }
}
