using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnscriptedLogic.MathUtils;
using UnscriptedLogic.Raycast;

public class HomingProjectile : ProjectileBehaviour
{
    private Transform target;

    public override void OnHit(Collider other, ProjectileBase projBase, Action<UnitBase, ProjectileBase> OnEnemyHit)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            OnEnemyHit?.Invoke(other.GetComponent<UnitBase>(), projBase);
            UnityEngine.Object.Destroy(projBase.gameObject);
        }
    }

    public override void Initialize(ProjectileBase parent)
    {
        Ray ray = new Ray(parent.transform.position, parent.transform.forward);
        if (Physics.SphereCast(ray, 0.25f, out RaycastHit hitinfo, 1000f, parent.UnitLayer))
        {
            target = hitinfo.transform;
        }
    }

    public override void Move(ProjectileBase projBase)
    {
        Vector3 dir = projBase.transform.forward;

        if (target != null)
        {
            dir = (target.position - projBase.transform.position).normalized;
        }

        projBase.transform.position += dir * projBase.Speed * Time.deltaTime;
    }
}
