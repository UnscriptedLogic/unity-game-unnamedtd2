using GameManagement;
using UnityEngine;

namespace Game.StatusEffects
{
    public class StatusBurning : StatusEffect
    {
        protected override void OnEffectStarted()
        {
            //if (AssetManager.instance.FlameParticle != null)
            //{
            //    effect = Instantiate(AssetManager.instance.FlameParticle, transform);
            //}
        }

        protected override void OnEffectTick()
        {
            unit.TakeDamage(amount);
        }
    }
}