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

        int index = 0;
        for (int i = 0; i < towerListDebug.Count; i++)
        {
            TowerSO towerSO = towerListDebug[i];
            GameObject buttonObject = Instantiate(towerBuyPrefab, parent);
            TowerBuyButton towerBuyButton = buttonObject.GetComponent<TowerBuyButton>();

            if (!towerBuyButton) return;

            tdManager.CashSystem.OnModified += (type, amount, curr) =>
            {
                towerBuyButton.TowerBtn.gameObject.SetActive(curr >= towerSO.TowerCost);
            };

            index = i;
            towerBuyButton.Initialize(towerSO.TowerCost, towerSO.IconSpr, () => 
            {
                if (tdManager.CashSystem.HasEnough(towerSO.TowerCost))
                {

                    BuyTower(index);
                    tdManager.CashSystem.Modify(ModifyType.Subtract, towerSO.TowerCost);
                }
            });
        }
    }

    private void BuyTower(int index)
    {
        TowerDefenseManager.instance.BuildTower(TowerDefenseManager.instance.AllTowerList.TowerList[index]);
    }
}
