using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnscriptedLogic.MathUtils;
using UnscriptedLogic.Raycast;

public class HomingProjectile : ProjectileBehaviour
{
    private Transform target;

    public override void OnHit(Collider other, Projectile projectile, EventHandler<UnitBase> OnEnemyHit)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            OnEnemyHit?.Invoke(this, other.GetComponent<UnitBase>());
            UnityEngine.Object.Destroy(projectile.gameObject);
        }
    }

    public override void Initialize(Projectile parent)
    {
        Ray ray = new Ray(parent.transform.position, parent.transform.forward);
        if (Physics.SphereCast(ray, 0.5f, out RaycastHit hitinfo, 1000f, parent.ProjectileSettings.UnitLayer))
        {
            target = hitinfo.transform;
        }
    }

    public override void Move(Projectile projBase)
    {
        Vector3 dir = projBase.transform.forward;

        if (target != null)
        {
            dir = (target.position - projBase.transform.position).normalized;
        }

        projBase.transform.position += dir * parent.ProjectileSettings.Speed * Time.deltaTime;
    }
}
