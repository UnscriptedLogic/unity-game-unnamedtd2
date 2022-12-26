using GameManagement;
using ProjectileManagement;
using System;
using System.Collections;
using System.Collections.Generic;
using UnitManagement;
using UnityEngine;

namespace TowerManagement
{
    public class RapidTower : TowerBase
    {
        [Header("Rapid Tower Extension")]
        public bool useTriple = false;
        public bool useExplosive = false;
        [SerializeField] private float explosionRange = 3f;
        [SerializeField] private GameObject explosionPrefab;

        protected override void FireProjectile()
        {
            if (useTriple)
            {
                CreateBullet(out ProjectileBase projectileBase0, projectilePrefabs[0], shootAnchors[0]);
                CreateBullet(out ProjectileBase projectileBase1, projectilePrefabs[0], shootAnchors[1]);
                CreateBullet(out ProjectileBase projectileBase2, projectilePrefabs[0], shootAnchors[2]);

                SubscribeProjectileEvents(projectileBase0);
                SubscribeProjectileEvents(projectileBase1);
                SubscribeProjectileEvents(projectileBase2);
                return;
            }

            base.FireProjectile();
        }

        protected override void OnProjectileHit(UnitBase unit, ProjectileBase projectileBase)
        {
            if (useExplosive)
            {
                Collider[] colliders = Physics.OverlapSphere(unit.transform.position, explosionRange, unitLayer);
                for (int i = 0; i < colliders.Length; i++)
                {
                    if (colliders[i].CompareTag("Enemy"))
                    {
                        float damagePercent = explosionRange * 0.01f * Vector3.Distance(unit.transform.position, colliders[i].transform.position);
                        colliders[i].GetComponent<UnitBase>().TakeDamage(Mathf.RoundToInt(damage * 0.01f * damagePercent));
                    }
                }

                GameObject explosion = LevelManagement.PullObject(explosionPrefab, unit.transform.position, Quaternion.Euler(-90f, 0f, 0f), true);
                explosion.transform.localScale = Vector3.one * 0.5f;
                StartCoroutine(LevelManagement.PushObjectAfter(explosion, 3f));
            }

            base.OnProjectileHit(unit, projectileBase);
        }
    }
}