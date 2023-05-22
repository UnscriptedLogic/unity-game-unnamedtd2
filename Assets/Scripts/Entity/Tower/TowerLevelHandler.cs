using System;
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
    public ExperienceLevel CurrentExperienceLevelNeeded => currentExperienceLevel;

    public event EventHandler OnLevelUp;

    private void OnEnable()
    {
        tower = GetComponent<TowerBase>();

        experienceHandler = new CurrencyHandler(0f);
        upgradePointsHandler = new CurrencyHandler(upgradePoints, max: 1f);

        currentExperienceLevel = TowerDefenseManager.instance.ExperienceLevelsSO.ExpList[level];

        tower.OnProjectileHitEvent += Tower_OnTowerProjectileHit;
    }

    private void Tower_OnTowerProjectileHit(object sender, OnProjectileHitEventArgs e)
    {
        experienceHandler.Modify(ModifyType.Add, tower.Damage);
        CheckLevelUp();
    }

    private void CheckLevelUp()
    {
        if (experienceHandler.Current >= currentExperienceLevel.amount)
        {
            if (level + 1 < TowerDefenseManager.instance.ExperienceLevelsSO.ExpList.Count)
            {
                level++;
                upgradePointsHandler.Modify(ModifyType.Add, 1);
                experienceHandler.Modify(ModifyType.Set, experienceHandler.Current - currentExperienceLevel.amount);
                currentExperienceLevel = TowerDefenseManager.instance.ExperienceLevelsSO.ExpList[level];

                OnLevelUp?.Invoke(this, EventArgs.Empty);
            }
        }
    }
}
