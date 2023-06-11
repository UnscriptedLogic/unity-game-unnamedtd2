using System;
using UnityEngine;

public class ProjectileBehaviour
{
    public virtual void Initialize(ProjectileBase parent)
    {

    }

    public virtual void Move(ProjectileBase projBase)
    {
        projBase.transform.position += projBase.transform.forward * projBase.Speed * Time.deltaTime;
    }

    public virtual void OnHit(Collider other, ProjectileBase projBase, Action<UnitBase, ProjectileBase> OnEnemyHit)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            projBase.IncreasePierce();
            OnEnemyHit?.Invoke(other.GetComponent<UnitBase>(), projBase);

            if (projBase.CurrentPierce >= projBase.Pierce)
            {
                //PoolManager.poolManagerInstance.PushToPool(projBase.gameObject);
                UnityEngine.Object.Destroy(projBase.gameObject);
            }
        }
    }
}