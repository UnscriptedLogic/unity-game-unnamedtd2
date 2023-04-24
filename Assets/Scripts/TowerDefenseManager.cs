using System;
using System.Linq;
using TMPro;
using UnityEngine;
using UnscriptedLogic.Currency;

public class TowerDefenseManager : MonoBehaviour
{
    [Header("Game Settings")]
    [SerializeField] private int startHealth;
    [SerializeField] private float startCash;
    [SerializeField] private ExperienceLevelsSO experienceLevels;

    [Header("Global Game Variables")]
    [SerializeField] private TowerListSO allUsableTowers;

    [Header("Components")]
    [SerializeField] private BuildManager buildManager;

    [Header("UI")]
    [SerializeField] private TextMeshProUGUI healthTMP;
    [SerializeField] private TextMeshProUGUI currencyTMP;

    private CurrencyHandler healthSystem;
    private CurrencyHandler cashSystem;

    public CurrencyHandler HealthSystem => healthSystem;
    public CurrencyHandler CashSystem => cashSystem;
    public TowerListSO AllTowerList => allUsableTowers;
    public ExperienceLevelsSO ExperienceLevelsSO => experienceLevels;
    
    public static TowerDefenseManager instance { get; private set; }

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        healthSystem = new CurrencyHandler(startHealth);
        cashSystem = new CurrencyHandler(startCash);

        healthTMP.text = healthSystem.Current.ToString();
        currencyTMP.text = $"${cashSystem.Current}";

        healthSystem.OnModified += (type, amount, curr) => healthTMP.text = curr.ToString();
        cashSystem.OnModified += (type, amount, curr) => currencyTMP.text = $"${curr}";

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
