using System;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public struct ProjectileTags
    {
        public const string SHRAPNEL = "Shrapnel";
    }

    [System.Serializable]
    public struct Settings
    {
        [SerializeField] private float speed;
        [SerializeField] private float lifetime;
        [SerializeField] private int pierce;
        [SerializeField] private LayerMask unitLayer;
        [SerializeField] private ProjectileBehaviour projectileBehaviour;

        public float Speed => speed;
        public float Lifetime => lifetime;
        public int Pierce => pierce;
        public LayerMask UnitLayer => unitLayer;
        public ProjectileBehaviour ProjectileBehaviour => projectileBehaviour;

        public Settings(float speed, float lifetime, int pierce, LayerMask unitLayer, ProjectileBehaviour projectileBehaviour = null)
        {
            this.pierce = pierce;
            this.speed = speed;
            this.lifetime = lifetime;
            this.unitLayer = unitLayer;
            this.projectileBehaviour = projectileBehaviour;
        }
    }

    [SerializeField] protected TrailRenderer[] trailRenderers;

    protected bool initialized;
    protected bool collided;

    protected ProjectileBehaviour projectileBehaviour;
    protected Dictionary<string, int> tags;

    protected Settings settings;
    protected int pierce;
    protected float lifeTime;

    protected Collider other;

    public Settings ProjectileSettings => settings;
    public int CurrentPierceCount => pierce;

    public event EventHandler OnProjectileDestroyed;
    public event EventHandler<UnitBase> OnEnemyHit;

    private void Update()
    {
        if (!initialized) return;

        if (lifeTime <= 0f)
        {
            Destroy(gameObject);
        }
        else
        {
            lifeTime -= Time.deltaTime;
        }

        if (collided)
        {
            projectileBehaviour.OnHit(other, this, OnEnemyHit);
            collided = false;
        }

        projectileBehaviour.Move(this);
    }

    public void InitializeAndSetActive(Settings projectileSettings, ProjectileBehaviour projectileBehaviour = null)
    {
        tags = new Dictionary<string, int>();

        settings = projectileSettings;

        pierce = 0;
        lifeTime = settings.Lifetime;

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

    public void IncreasePierce() => pierce++;

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
        OnProjectileDestroyed?.Invoke(this, EventArgs.Empty);
        tags = new Dictionary<string, int>();
    }
}