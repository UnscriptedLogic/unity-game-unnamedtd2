using UnityEngine;

public class RiflerTower : Tower
{
    [HideInInspector] public bool useDoubleShells = false;
    [HideInInspector] public bool useQuadShells = false;
    [HideInInspector] public bool useKnockback = false;
    [HideInInspector] public bool useShrapnel = false;

    //[SerializeField] private int shrapnelCount = 6;

    public ProjectileBehaviour projectileBehaviour;

    protected override void FireProjectile()
    {
        animator.SetTrigger("Attack");

        if (useQuadShells)
        {
            for (int i = 0; i < 5; i++)
            {
                CreateBullet(out ProjectileBase projectileBase, projectilePrefabs[0], shootAnchors[i], projectileBehaviour);
                SubscribeProjectileEvents(projectileBase);
            }
            return;
        }

        if (useDoubleShells)
        {
            for (int i = 0; i < 3; i++)
            {
                CreateBullet(out ProjectileBase projectileBase, projectilePrefabs[0], shootAnchors[i], projectileBehaviour);
                SubscribeProjectileEvents(projectileBase);
            }
            return;
        }

        CreateBullet(out ProjectileBase baseScript, projectilePrefabs[0], shootAnchors[0], projectileBehaviour);
        SubscribeProjectileEvents(baseScript);
    }

    protected override void OnProjectileHit(UnitBase unit, ProjectileBase projectileBase)
    {
        //if (useKnockback)
        //{
        //    unit.GetComponent<AffectorManager>().ApplyEffect(AffectorManager.EffectType.KnockBack, 0.2f, 20f, 0.05f);
        //}

        //if (useShrapnel)
        //{
        //    if (!projectileBase.tags.ContainsKey(ProjectileBase.SHRAPNEL_TAG))
        //    {
        //        for (int i = 0; i < shrapnelCount; i++)
        //        {
        //            Vector3 rotation = new Vector3(0, 360f / shrapnelCount * i, 0f);
        //            CreateBullet(out ProjectileBase projectileBase0, projectilePrefabs[0], unit.transform.position, Quaternion.Euler(rotation), new ProjectileSettings(projectileSpeed, 0.1f, pierce), new ProjectileBehaviour());
        //            SubscribeProjectileEvents(projectileBase0);
        //            projectileBase0.tags.Add(ProjectileBase.SHRAPNEL_TAG, 0);
        //        }
        //    }
        //}

        //if (projectileBase.tags.ContainsKey(ProjectileBase.SHRAPNEL_TAG))
        //{
        //    unit.TakeDamage(Mathf.RoundToInt(damage / shrapnelCount));
        //    return;
        //}

        base.OnProjectileHit(unit, projectileBase);
    }

    protected override void OnTargetFound()
    {

    }

    protected override void OnTargetLost()
    {

    }
}