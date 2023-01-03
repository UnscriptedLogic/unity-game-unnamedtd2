using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.StatusEffects
{
    public class StatusKnockback : StatusEffect
    {
        float originalSpeed;

        protected override void OnEffectStarted()
        {
            originalSpeed = unit.Speed;
            unit.SetSpeed(unit.Speed / 100 * amount);
        }

        protected override void OnEffectTick()
        {
            unit.SetSpeed(Mathf.MoveTowards(unit.Speed, originalSpeed, duration * Time.deltaTime));
        }
        
        protected override void OnEffectEnded()
        {
            unit.SetSpeed(originalSpeed);
        }
    }
}