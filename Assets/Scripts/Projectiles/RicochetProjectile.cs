using System;
using System.Collections.Generic;
using UnityEngine;

public class RicochetProjectile : ProjectileBehaviour
{
    private float checkRadius;
    private List<GameObject> taggedEnemies;

    private ProjectileBase projectileBase;

    public RicochetProjectile(float checkRadius) : base()
    {
        this.checkRadius = checkRadius;
        taggedEnemies = new List<GameObject>();
    }

    public override void OnHit(Collider other, ProjectileBase projBase, Action<UnitBase, ProjectileBase> OnEnemyHit)
    {
        if (projectileBase == null)
        {
            projectileBase = projBase;
            projectileBase.OnProjectileDestroyed += ProjectileBase_OnProjectileDestroyed;
        }

        taggedEnemies.Add(other.gameObject);

        Collider[] colliders = Physics.OverlapSphere(projBase.transform.position, checkRadius, LayerMask.GetMask("Unit"));
        for (int i = 0; i < colliders.Length; i++)
        {
            if (!colliders[i].CompareTag("Enemy")) continue;

            if (!taggedEnemies.Contains(colliders[i].gameObject))
            {
                projBase.transform.LookAt(colliders[i].transform);
                break;
            }
        }

        if (other.CompareTag("Enemy"))
        {
            projBase.IncreasePierce();
            OnEnemyHit?.Invoke(other.GetComponent<UnitBase>(), projBase);

            if (projBase.CurrentPierce >= projBase.Pierce)
            {
                PoolManager.poolManagerInstance.PushToPool(projBase.gameObject);
                taggedEnemies = new List<GameObject>();
            }
        }
    }

    private void ProjectileBase_OnProjectileDestroyed(ProjectileBase obj)
    {
        taggedEnemies = new List<GameObject>();
    }
}