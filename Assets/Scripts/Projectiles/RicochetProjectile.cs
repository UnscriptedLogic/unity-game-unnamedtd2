using Core;
using System;
using System.Collections;
using System.Collections.Generic;
using UnitManagement;
using UnityEngine;

namespace ProjectileManagement
{
    public class RicochetProjectile : ProjectileBehaviour
    {
        private float checkRadius;
        private float minimumDist = 0.05f;
        private List<GameObject> taggedEnemies;

        public RicochetProjectile(float checkRadius) : base()
        {
            this.checkRadius = checkRadius;
            taggedEnemies = new List<GameObject>();
        }

        public override void OnHit(Collider other, ProjectileBase projBase, Action<UnitBase> OnEnemyHit)
        {
            Debug.Log("Ricochet");

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
                OnEnemyHit?.Invoke(other.GetComponent<UnitBase>());

                if (projBase.CurrentPierce >= projBase.Pierce)
                {
                    PoolManager.poolManagerInstance.PushToPool(projBase.gameObject);
                    taggedEnemies = new List<GameObject>();
                }
            }
        }
    }
}