using System;
using UnityEngine;

public class OnProjectileHitEventArgs : EventArgs
{
    public UnitBase unit;
    public Projectile projectile;
    public Action<UnitBase, float> ApplyDamageMethod;
}

public class TowerBase : MonoBehaviour
{
    
}
