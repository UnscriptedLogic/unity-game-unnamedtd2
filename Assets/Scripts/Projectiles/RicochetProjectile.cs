using System;
using System.Collections.Generic;
using UnityEngine;

public class RicochetProjectile : ProjectileBehaviour
{
    private float checkRadius;
    private List<GameObject> taggedEnemies;

    private Projectile projectileBase;

    public RicochetProjectile(float checkRadius) : base()
    {
        this.checkRadius = checkRadius;
        taggedEnemies = new List<GameObject>();
    }

    public override void OnHit(Collider other, Projectile projectile, EventHandler<UnitBase> OnEnemyHit)
    {
        //if (projectileBase == null)
        //{
        //    projectileBase = projectile;
        //    projectileBase.OnProjectileDestroyed += ProjectileBase_OnProjectileDestroyed;
        //}

        //taggedEnemies.Add(other.gameObject);

        //Collider[] colliders = Physics.OverlapSphere(projectile.transform.position, checkRadius, LayerMask.GetMask("Unit"));
        //for (int i = 0; i < colliders.Length; i++)
        //{
        //    if (!colliders[i].CompareTag("Enemy")) continue;

        //    if (!taggedEnemies.Contains(colliders[i].gameObject))
        //    {
        //        projectile.transform.LookAt(colliders[i].transform);
        //        break;
        //    }
        //}

        //if (other.CompareTag("Enemy"))
        //{
        //    projectile.IncreasePierce();
        //    OnEnemyHit?.Invoke(other.GetComponent<UnitBase>(), projBase);

        //    if (projectile.CurrentPierce >= projectile.Pierce)
        //    {
        //        PoolManager.poolManagerInstance.PushToPool(projectile.gameObject);
        //        taggedEnemies = new List<GameObject>();
        //    }
        //}
    }

    private void ProjectileBase_OnProjectileDestroyed(Projectile obj)
    {
        taggedEnemies = new List<GameObject>();
    }
}