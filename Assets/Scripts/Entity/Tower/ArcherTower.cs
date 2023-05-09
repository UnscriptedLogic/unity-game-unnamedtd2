using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArcherTower : TowerBase
{
    protected override void FireProjectile()
    {
        animator.SetTrigger("Attack");
        soundManager.PlaySound(new AudioSettings(audioFields[0].clip, audioFields[0].volume, AudioType.TOWER), shootAnchors[0].position);
        base.FireProjectile();
        OnProjectileFired();
    }
}
