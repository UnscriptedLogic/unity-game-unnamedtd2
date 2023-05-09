using UnityEngine;

public class RiflerTower : TowerBase
{
    public ProjectileBehaviour projectileBehaviour;
    public bool useHitscan;

    protected override void FireProjectile()
    {
        animator.SetTrigger("Attack");
        soundManager.PlaySound(new AudioSettings(audioFields[0].clip, audioFields[0].volume, AudioType.TOWER), shootAnchors[0].position);

        if (useHitscan)
        {
            OnProjectileFired();
            OnProjectileHit(currentTarget.GetComponent<UnitBase>(), null);
        } else
        {
            base.FireProjectile();
            OnProjectileFired();
        }
    }

    //protected override void OnProjectileHitEvent(UnitBase unit, ProjectileBase projectileBase)
    //{
    //    base.OnProjectileHitEvent(unit, projectileBase);

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