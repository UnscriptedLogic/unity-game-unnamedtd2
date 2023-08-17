using System;
using UnityEngine;

public class ProjectileBehaviour
{
    protected Projectile parent;

    public virtual void Initialize(Projectile parent)
    {
        this.parent = parent;
    }

    public virtual void Move(Projectile projBase)
    {
        projBase.transform.position += projBase.transform.forward * projBase.ProjectileSettings.Speed * Time.deltaTime;
    }

    public virtual void OnHit(Collider other, Projectile projectile, EventHandler<UnitBase> OnEnemyHit)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            projectile.IncreasePierce();
            OnEnemyHit?.Invoke(this, other.GetComponent<UnitBase>());

            if (projectile.CurrentPierceCount >= projectile.ProjectileSettings.Pierce)
            {
                UnityEngine.Object.Destroy(projectile.gameObject);
            }
        }
    }
}