using UnityEngine;
using UnscriptedLogic.Currency;
using UnscriptedLogic.MathUtils;

public class SplitShot : Ability
{
    private int[] chances;
    private int[] damageReduction;

    public override void OnAdded()
    {
        maxLevel = 4;

        levelRequirements = new int[3] { 7, 10, 13 };

        levelHandler = new CurrencyHandler(1, max: maxLevel);

        chances = new int[4] { 10, 20, 30, 40 };
        damageReduction = new int[4] { 75, 50, 25, 0 };
    }

    protected override void OnProjectileCreated(GameObject bulletObject, ProjectileBase projectileScript)
    {
        if (RandomLogic.IntZeroTo(100) > chances[(int)levelHandler.Current - 1]) return;

        tower.CreateBullet(out ProjectileBase leftProjectile, tower.ProjectilePrefabs[0], tower.ShootAnchors[1]);
        leftProjectile.tags.Add("SplitShot", 1);
        tower.SubscribeProjectileEvents(leftProjectile);

        tower.CreateBullet(out ProjectileBase rightProjectile, tower.ProjectilePrefabs[0], tower.ShootAnchors[2]);
        rightProjectile.tags.Add("SplitShot", 1);
        tower.SubscribeProjectileEvents(rightProjectile);
    }

    protected override void OnProjectileHit(object sender, OnProjectileHitEventArgs eventArgs)
    {
        if (eventArgs.projectile.tags.ContainsKey("SplitShot"))
        {
            eventArgs.unit.TakeDamage(tower.Damage / 100 * (100 - damageReduction[CurrentLevel - 1]));
        }
    }
}
