using System;
using UnityEngine;

public class TowerBehaviour : TowerComponent
{
    public enum TargetSortMode
    {
        First,
        Last,
        Closest,
        Furthest,
        Strongest,
        Weakest
    }

    [Serializable]
    public class RotationHeads
    {
        public Transform rotationHead;
        public bool levelled;
    }

    [Serializable]
    public class AudioFields
    {
        public AudioClip clip;
        public float volume;
    }

    [Header("Tower Components")]
    [SerializeField] protected RotationHeads[] rotationHeads;
    [SerializeField] protected GameObject[] projectilePrefabs;
    [SerializeField] protected Transform[] shootAnchors;

    [Header("Base Targetting Settings")]
    [SerializeField] protected float losBaseOffset = 0.5f;
    [SerializeField] protected LayerMask losObstructionLayer;
    [SerializeField] protected LayerMask unitLayer;

    [Header("Others")]
    [SerializeField] protected Animator animator;
    [SerializeField] protected AudioFields[] audioFields;

    protected Transform currentTarget;
    protected float currentReloadTime;

    protected TowerStats Stats => tower.Stats;

    protected override void OnControllerInitialized(object sender, EventArgs e)
    {
        //Short delay before shooting the first bullet
        currentReloadTime = 0.5f;
    }

    protected virtual void Update()
    {
        if (currentTarget != null)
        {
            WhileTargetFound();
        }
    }

    protected virtual void WhileTargetFound()
    {
        CommonTowerLogic();
    }

    protected virtual void CommonTowerLogic()
    {
        if (currentReloadTime <= 0f)
        {
            if (currentTarget != null)
            {
                for (int i = 0; i < rotationHeads.Length; i++)
                {
                    RotateToTarget(rotationHeads[i].rotationHead, levelled: rotationHeads[i].levelled);
                }

                currentReloadTime = Stats.AttackSpeed;
                FireProjectile();
            }
        }
        else
        {
            currentReloadTime -= Time.deltaTime;
        }
    }

    protected bool LostTarget()
    {
        if (!currentTarget.gameObject.activeInHierarchy)
        {
            return true;
        }

        if (!HasLOSToTarget(currentTarget, out RaycastHit losObstruction))
        {
            return true;
        }

        if (Vector3.Distance(currentTarget.position, transform.position) >= Stats.RangeDistance + 1f)
        {
            return true;
        }

        return false;
    }

    protected bool HasLOSToTarget(Transform target, out RaycastHit hit)
    {
        Vector3 origin = transform.position + Vector3.up * losBaseOffset;
        return !Physics.Raycast(origin, (target.position - origin).normalized, out hit, Vector3.Distance(target.position, origin), losObstructionLayer);
    }

    protected void RotateToTarget(Transform rotationHead, Transform target = null, bool levelled = false)
    {
        if (target == null)
        {
            target = currentTarget;
        }

        if (currentTarget == null)
        {
            return;
        }

        Vector3 targetPos = target.position;
        if (levelled)
        {
            targetPos.y = rotationHead.position.y;
        }

        rotationHead.LookAt(targetPos);
    }

    public Projectile CreateProjectile(GameObject prefab, Transform anchor, ProjectileBehaviour projectileBehaviour = null)
    {
        return CreateProjectile(
            prefab,
            anchor.position,
            anchor.rotation,
            new Projectile.Settings(
                Stats.ProjectileSpeed,
                Stats.ProjectileSpeed,
                Stats.ProjectilePierce,
                unitLayer
                ),
            projectileBehaviour
            );
    }

    public Projectile CreateProjectile(GameObject prefab, Vector3 position, Quaternion rotation, Projectile.Settings projectileSettings, ProjectileBehaviour projectileBehaviour = null)
    {
        GameObject projectileGO = Instantiate(prefab, position, rotation);
        Projectile projectile = projectileGO.GetComponent<Projectile>();
        projectile.InitializeAndSetActive(projectileSettings, projectileBehaviour);
        return projectile;
    }

    public virtual void SubscribeProjectileEvents(Projectile projectileBase)
    {
        projectileBase.OnEnemyHit += OnProjectileHit;
        projectileBase.OnProjectileDestroyed += OnProjectileDestroyed;
    }

    public virtual void UnsubscribeProjectileEvents(Projectile projectileBase)
    {
        projectileBase.OnEnemyHit -= OnProjectileHit;
        projectileBase.OnProjectileDestroyed -= OnProjectileDestroyed;
    }

    protected virtual void FireProjectile()
    {
        Projectile projectile = CreateProjectile(projectilePrefabs[0], shootAnchors[0]);
        SubscribeProjectileEvents(projectile);
    }

    protected virtual void OnProjectileHit(object sender, UnitBase unit) { }

    protected virtual void OnProjectileDestroyed(object sender, EventArgs args)
    {
        Projectile projectile = sender as Projectile;
        UnsubscribeProjectileEvents(projectile);
    }
}