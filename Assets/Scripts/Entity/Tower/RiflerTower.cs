using UnityEngine;

public class RiflerTower : Tower
{
    public ProjectileBehaviour projectileBehaviour;

    protected override void FireProjectile()
    {
        base.FireProjectile();

        animator.SetTrigger("Attack");
    }

    //protected override void OnProjectileHit(UnitBase unit, ProjectileBase projectileBase)
    //{
    //    base.OnProjectileHit(unit, projectileBase);

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
    //}
}