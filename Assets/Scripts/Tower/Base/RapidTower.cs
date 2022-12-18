using ProjectileManagement;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TowerManagement
{
    public class RapidTower : TowerBase
    {
        public bool useTriple = false;

        protected override void Start()
        {
            base.Start();
        }

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
    }
}