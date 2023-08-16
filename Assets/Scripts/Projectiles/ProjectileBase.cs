using ProjectileManagement;
using System;
using System.Collections.Generic;
using UnityEngine;

public struct ProjectileSettings
{
    public int pierce;
    public float speed;
    public float lifetime;
    public LayerMask unitLayer;
    public ProjectileBehaviour projectileBehaviour;

    public ProjectileSettings(float speed, float lifetime, int pierce, LayerMask unitLayer, ProjectileBehaviour projectileBehaviour = null)
    {
        this.pierce = pierce;
        this.speed = speed;
        this.lifetime = lifetime;
        this.unitLayer = unitLayer;
        this.projectileBehaviour = projectileBehaviour;
    }
}

public class ProjectileBase : MonoBehaviour
{
    public const string SHRAPNEL_TAG = "Shrapnel";

    protected int pierce;
    protected float speed = 1f;
    protected float lifeTime = 1f;
    protected LayerMask unitLayer;
    [SerializeField] protected TrailRenderer[] trailRenderers;

    public event Action<UnitBase, ProjectileBase> OnEnemyHit;
    public event Action<ProjectileBase> OnProjectileDestroyed;

    public ProjectileBehaviour projectileBehaviour;
    public Dictionary<string, int> tags;

    protected Collider other;
    protected bool collided;
    protected float _lifetime;
    protected bool initialized;
    protected int _pierce;

    public float Speed => speed;
    public int Pierce => pierce;
    public int CurrentPierce => _pierce;
    public LayerMask UnitLayer => unitLayer;

    private void Update()
    {
        if (!initialized) return;

        if (_lifetime <= 0f)
        {
            //PoolManager.poolManagerInstance.PushToPool(gameObject);
            Destroy(gameObject);
        }
        else
        {
            _lifetime -= Time.deltaTime;
        }

        if (collided)
        {
            projectileBehaviour.OnHit(other, this, OnEnemyHit);
            collided = false;
        }

        projectileBehaviour.Move(this);
    }

    public void InitializeAndSetActive(ProjectileSettings projectileSettings, ProjectileBehaviour projectileBehaviour = null)
    {
        tags = new Dictionary<string, int>();

        _pierce = 0;
        pierce = projectileSettings.pierce;
        speed = projectileSettings.speed;
        lifeTime = projectileSettings.lifetime;
        unitLayer = projectileSettings.unitLayer;
        _lifetime = lifeTime;
        initialized = true;

        if (projectileBehaviour == null)
        {
            this.projectileBehaviour = new ProjectileBehaviour();
        }
        else
        {
            this.projectileBehaviour = projectileBehaviour;
        }

        Debug.Log(this.projectileBehaviour);
        this.projectileBehaviour.Initialize(this);
        gameObject.SetActive(true);
    }

    public void IncreasePierce() => _pierce++;

    private void OnTriggerEnter(Collider other)
    {
        collided = true;
        this.other = other;
    }

    private void OnEnable()
    {
        for (int i = 0; i < trailRenderers.Length; i++)
        {
            trailRenderers[i].Clear();
        }
    }

    private void OnDisable()
    {
        OnProjectileDestroyed?.Invoke(this);
        tags = new Dictionary<string, int>();
    }
}