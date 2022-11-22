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
            CommonTowerLogic();
        }

        protected override void OnTargetFound()
        {
            
        }

        protected override void OnTargetLost()
        {
            
        }
    }
}