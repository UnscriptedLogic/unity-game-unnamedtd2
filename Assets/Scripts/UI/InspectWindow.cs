using System.Collections;
using System.Collections.Generic;
using TowerManagement;
using UnityEngine;

public class InspectWindow : MonoBehaviour
{
    [Header("Upgrade Section")]
    [SerializeField] private GameObject upgradeSection;
    [SerializeField] private GameObject upgradeButtonPrefab;

    private void Start()
    {
        
    }

    public void ShowModal()
    {

    }

    public void InitUpgradeButtons(TowerSO towerSO, int levelIndex)
    {
        // Clear all the upgrade buttons
        foreach (Transform child in upgradeSection.transform)
        {
            Destroy(child.gameObject);
        }

        // Create the upgrade buttons
        for (int i = 0; i < towerSO.TowerLevels[levelIndex].upgradeOptions.Length; i++)
        {
            GameObject upgradeButton = Instantiate(upgradeButtonPrefab, upgradeSection.transform);
        }
    }
}
