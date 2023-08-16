using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArcherTower : TowerBase
{
    protected override void FireProjectile()
    {
        animator.SetTrigger("Attack");
        soundManager.PlaySound(new AudioSettings(audioFields[0].clip, audioFields[0].volume, AudioType.TOWER), shootAnchors[0].position);

        GameObject bullet = CreateBullet(out ProjectileBase projectileBase, projectilePrefabs[0], shootAnchors[0], new HomingProjectile());
        SubscribeProjectileEvents(projectileBase);

        OnTowerProjectileCreated?.Invoke(bullet, projectileBase);

        OnProjectileFired();
    }
}
