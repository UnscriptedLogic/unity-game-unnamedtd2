using ProjectileManagement;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TowerManagement
{
    public class ScoutTower : TowerBase
    {
        public bool useDoubleShells = false;
        public bool useQuadShells = false;

        public ProjectileBehaviour projectileBehaviour;

        protected override void FireProjectile()
        {
            animator.SetTrigger("Attacking");

            if (useQuadShells)
            {
                for (int i = 0; i < 5; i++)
                {
                    CreateBullet(out ProjectileBase projectileBase, projectilePrefabs[0], shootAnchors[i], projectileBehaviour);
                    SubscribeProjectileEvents(projectileBase);
                }
                return;
            }
            
            if (useDoubleShells)
            {
                for (int i = 0; i < 3; i++)
                {
                    CreateBullet(out ProjectileBase projectileBase, projectilePrefabs[0], shootAnchors[i], projectileBehaviour);
                    SubscribeProjectileEvents(projectileBase);
                }
                return;
            }

            CreateBullet(out ProjectileBase baseScript, projectilePrefabs[0], shootAnchors[0], projectileBehaviour);
            SubscribeProjectileEvents(baseScript);
        }

        protected override void OnTargetFound()
        {
            
        }

        protected override void OnTargetLost()
        {
            
        }
    }
}