using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ProjectileManagement
{
    public class ProjectileBehaviourFactory
    {
        private ProjectileBase context;

        public ProjectileBehaviourFactory(ProjectileBase context)
        {
            this.context = context;
        }

        public ProjectileBehaviour RicochetBullets(float checkRadius)
        {
            return new RicochetProjectile(checkRadius);
        }
    }
}