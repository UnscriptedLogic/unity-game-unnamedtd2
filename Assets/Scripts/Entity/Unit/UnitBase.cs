using System;
using System.Collections.Generic;
using UnityEngine;
using UnscriptedLogic.Currency;
using UnscriptedLogic.MathUtils;

public class UnitTookDamageEventArgs : EventArgs
{
    public float damage;
    public float currentHealth;
}

public class UnitBase : MonoBehaviour, IInspectable
{
    [Header("Health Settings")]
    [SerializeField] private float health;
    [SerializeField] private float armor;

    [Header("Movement Settings")]
    [SerializeField] private float speed = 5f;
    [SerializeField] [Range(0, 1)] private float turnSpeed = 0.5f;
    [SerializeField] private float offsetHeight;
    [SerializeField] private float stoppingDistance = 0.1f;

    [Header("Misc Settings")]
    [SerializeField] private float deathDelay = 5f;

    [Space(10)]
    [Tooltip("The reference matching float for the animation's walking speed")]
    [SerializeField] private bool autoMatchRefWalkSpeed;
    [SerializeField] private float refWalkSpeedAnim;
    [Tooltip("The multiplier adjustment for the animation speed")]
    [SerializeField] private float walkSpeedAdjust = 2f;
    [Space(10)]
    [SerializeField] private BoxCollider boxCollider;
    [SerializeField] private Animator animator;
    [SerializeField] private DamageFlashSkinned damageFlash;

    private Vector3[] points;
    private int currentPoint = 0;

    private CurrencyHandler healthHandler;
    private CurrencyHandler speedHandler;
    private CurrencyHandler armorHandler;

    public float MaxHealth => health;
    public float CurrentHealth => healthHandler.Current;
    public float MaxSpeed => speed;
    public float CurrentSpeed => speedHandler.Current;
    public float StartArmor => armor;
    public float CurrentArmor => armorHandler.Current;

    public int CurrentWaypoint => currentPoint;
    public static event EventHandler OnAnyUnitSpawned;
    public static event EventHandler OnAnyUnitDespawned;
    public static event EventHandler OnAnyUnitCompletedPath;
    public static event EventHandler<UnitTookDamageEventArgs> OnAnyUnitTookDamage;

    private void OnEnable()
    {
        points = MapManager.instance.Pathway.ToArray();

        if (points.Length != 0)
            transform.position = new Vector3(points[currentPoint].x, offsetHeight, points[currentPoint].z);

        armorHandler = new CurrencyHandler(armor);
        healthHandler = new CurrencyHandler(health, max: health);
        speedHandler = new CurrencyHandler(speed);

        healthHandler.OnEmpty += OnDeath;
        speedHandler.OnModified += SpeedHandler_OnModified;
        animator.speed = (animator.speed / refWalkSpeedAnim * speedHandler.Current) * walkSpeedAdjust;

        OnAnyUnitSpawned?.Invoke(this, EventArgs.Empty);
    }

    private void OnDisable()
    {
        OnAnyUnitDespawned?.Invoke(this, EventArgs.Empty);
    }

    private void SpeedHandler_OnModified(object sender, CurrencyEventArgs e)
    {
        animator.speed = (animator.speed / refWalkSpeedAnim * speedHandler.Current) * walkSpeedAdjust;
    }

    private void Update()
    {
        if (points.Length == 0) return;

        Vector3 targetPosition = new Vector3(points[currentPoint].x, offsetHeight, points[currentPoint].z);
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime);
        transform.rotation = MathLogic.RotateSmooth(transform, targetPosition, turnSpeed);

        if (Vector3.Distance(transform.position, targetPosition) < stoppingDistance)
        {
            currentPoint++;
            if (currentPoint >= points.Length)
            {
                OnAnyUnitCompletedPath?.Invoke(this, EventArgs.Empty);
                Destroy(gameObject);
            }
        }
    }

    public void TakeDamage(float damage)
    {
        healthHandler.Modify(ModifyType.Subtract, damage);

        damageFlash.Flash();
        OnAnyUnitTookDamage?.Invoke(this, new UnitTookDamageEventArgs()
        {
            damage = damage,
            currentHealth = healthHandler.Current,
        });
    }

    public virtual void OnDeath(object sender, EventArgs e)
    {
        points = new Vector3[0];
        boxCollider.enabled = false;
        animator.SetTrigger("Die");
        Destroy(gameObject, deathDelay);
    }

    public void KillUnit()
    {
        Destroy(gameObject);
    }

    private void OnValidate()
    {
        if (autoMatchRefWalkSpeed)
        {
            refWalkSpeedAnim = speed;
        }
    }
}