using System.Collections;
using System.Collections.Generic;
using UnitManagement;
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
    }
}