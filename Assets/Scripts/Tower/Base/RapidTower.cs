using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TowerManagement
{
    public class RapidTower : TowerBase
    {
        protected override void Update()
        {
            base.Update();
        }

        protected override void FireProjectile()
        {
            
        }

        protected override void OnTargetFound()
        {
            FireProjectile();
        }

        protected override void OnTargetLost()
        {
            
        }
    }
}