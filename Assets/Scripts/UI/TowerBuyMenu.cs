using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnscriptedLogic.MathUtils;

public class TowerBuyMenu : MonoBehaviour
{
    [SerializeField] private Transform parent;
    [SerializeField] private GameObject towerBuyPrefab;

    [SerializeField] private List<TowerSO> towerListDebug;

    private TowerDefenseManager tdManager;

    private void Start()
    {
        tdManager = TowerDefenseManager.instance;

        for (int i = 0; i < towerListDebug.Count; i++)
        {
            TowerSO towerSO = towerListDebug[i];
            GameObject buttonObject = Instantiate(towerBuyPrefab, parent);
            TowerBuyButton towerBuyButton = buttonObject.GetComponent<TowerBuyButton>();

            if (!towerBuyButton) return;

            tdManager.CashSystem.OnModified += (sender, eventArgs) =>
            {
                towerBuyButton.TowerBtn.interactable = eventArgs.currentValue >= towerSO.TowerCost;
            };

            int index = i;
            towerBuyButton.Initialize(towerSO.TowerCost, towerSO.IconSpr, () => 
            {
                if (tdManager.CashSystem.HasEnough(towerSO.TowerCost))
                {
                    BuyTower(index);
                }
            });
        }
    }

    private void BuyTower(int index)
    {
        TowerDefenseManager.instance.BuildTower(TowerDefenseManager.instance.AllTowerList.TowerList[index]);
    }
}
