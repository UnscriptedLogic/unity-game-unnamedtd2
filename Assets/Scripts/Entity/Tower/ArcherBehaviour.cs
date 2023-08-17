using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArcherBehaviour : TowerBehaviour
{
    protected override void FireProjectile()
    {
        animator.SetTrigger("Attack");
        FXManager.instance.PlaySound(new AudioSettings(audioFields[0].clip, audioFields[0].volume, AudioType.TOWER), shootAnchors[0].position);

        Projectile projectile = CreateProjectile(projectilePrefabs[0], shootAnchors[0], new HomingProjectile());
        SubscribeProjectileEvents(projectile);
    }
}
