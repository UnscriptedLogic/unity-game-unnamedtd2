using Core;
using System;
using System.Collections;
using System.Collections.Generic;
using UnitManagement;
using UnityEngine;

namespace ProjectileManagement
{
    public class ProjectileBehaviour
    {
        public virtual void Move(ProjectileBase projBase)
        {
            projBase.transform.position += projBase.transform.forward * projBase.Speed * Time.deltaTime;
        }

        public virtual void OnHit(Collider other, ProjectileBase projBase, Action<UnitBase> OnEnemyHit)
        {
            if (other.CompareTag("Enemy"))
            {
                projBase.IncreasePierce();
                OnEnemyHit?.Invoke(other.GetComponent<UnitBase>());

                if (projBase.CurrentPierce >= projBase.Pierce)
                {
                    PoolManager.poolManagerInstance.PushToPool(projBase.gameObject);
                }
            }
        }
    }
}