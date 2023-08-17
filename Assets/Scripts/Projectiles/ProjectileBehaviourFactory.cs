using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ProjectileManagement
{
    public class ProjectileBehaviourFactory
    {
        private Projectile context;

        public ProjectileBehaviourFactory(Projectile context)
        {
            this.context = context;
        }

        public ProjectileBehaviour RicochetBullets(float checkRadius)
        {
            return new RicochetProjectile(checkRadius);
        }
    }
}