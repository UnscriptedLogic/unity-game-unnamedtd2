using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TowerBuyMenu : MonoBehaviour
{
    [SerializeField] private Transform parent;
    [SerializeField] private GameObject towerBuyPrefab;

    [SerializeField] private List<TowerSO> towerListDebug;

    private void Start()
    {
        int index = 0;
        for (int i = 0; i < towerListDebug.Count; i++)
        {
            TowerSO towerSO = towerListDebug[i];
            GameObject buttonObject = Instantiate(towerBuyPrefab, parent);
            TowerBuyButton towerBuyButton = buttonObject.GetComponent<TowerBuyButton>();

            if (!towerBuyButton) return;

            index = i;
            towerBuyButton.Initialize(towerSO.TowerCost, towerSO.IconSpr, () => { BuyTower(index); });
        }
    }

    private void BuyTower(int index)
    {
        TowerDefenseManager.instance.BuildTower(TowerDefenseManager.instance.AllTowerList.TowerList[index]);
    }
}
