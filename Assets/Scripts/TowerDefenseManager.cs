using System;
using System.Linq;
using UnityEngine;
using UnscriptedLogic.Currency;

public class TowerDefenseManager : MonoBehaviour
{
    [Header("Game Settings")]
    [SerializeField] private int startHealth;
    [SerializeField] private float startCash;

    [Header("Global Game Variables")]
    [SerializeField] private TowerListSO allUsableTowers;

    [Header("Components")]
    [SerializeField] private BuildManager buildManager;

    private CurrencyHandler healthSystem;
    private CurrencyHandler cashSystem;

    public CurrencyHandler HealthSystem => healthSystem;
    public CurrencyHandler CashSystem => cashSystem;

    public TowerListSO AllTowerList => allUsableTowers;

    public static TowerDefenseManager instance { get; private set; }

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        healthSystem = new CurrencyHandler(startHealth);
        cashSystem = new CurrencyHandler(startCash);

        UnitBase.OnAnyUnitCompletedPath += OnUnitCompletedPath;
    }

    public void BuildTower(TowerSO tower)
    {
        int towerIndex = allUsableTowers.TowerList.IndexOf(tower);
        buildManager.AttemptBuild(towerIndex);
    }

    private void OnUnitCompletedPath(object sender, EventArgs eventArgs)
    {
        UnitBase unitScript = sender as UnitBase;

        Destroy(unitScript.gameObject);
    }
}