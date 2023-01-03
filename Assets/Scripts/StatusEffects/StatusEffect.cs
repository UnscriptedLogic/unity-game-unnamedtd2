using System;
using System.Collections;
using System.Collections.Generic;
using UnitManagement;
using UnityEngine;

namespace Game.StatusEffects
{
    public class StatusEffect : MonoBehaviour
    {
        protected float duration;
        protected float amount;
        protected float tickInterval;

        protected float _duration;
        protected float _tick;
        protected bool isInifinite;
        protected AffectorManager affectorManager;
        protected GameObject effect;
        protected UnitBase unit;

        public float TickInterval => tickInterval;
        public float Amount => amount;
        public float Duration => duration;

        public void Initialize(AffectorManager affectorManager, float duration, float amount, float tickInterval)
        {
            this.affectorManager = affectorManager;
            this.duration = duration;
            this.amount = amount;
            this.tickInterval = tickInterval;
            unit = GetComponent<UnitBase>();

            _duration = duration;
            if (duration == 0)
            {
                isInifinite = true;
            }

            OnEffectStarted();
        }

        protected virtual void Update()
        {
            ReduceDuration();

            if (_tick <= 0f)
            {
                OnEffectTick();
                _tick = tickInterval;
            }
            else
            {
                _tick -= Time.deltaTime;
            }
        }

        protected virtual void OnEffectStarted()
        {

        }

        protected virtual void OnEffectTick()
        {

        }

        protected virtual void OnEffectEnded()
        {

        }

        public void ResetDuration(float newDuration)
        {
            _duration = newDuration;
        }

        protected void ReduceDuration()
        {
            if (isInifinite)
                return;

            if (_duration <= 0f)
            {
                DestroyStatus();
            }
            else
            {
                _duration -= Time.deltaTime;
            }
        }

        protected void OnDestroy()
        {
            DestroyStatus();
        }

        protected void OnDisable()
        {
            DestroyStatus();
        }

        public virtual void DestroyStatus()
        {
            OnEffectEnded();

            if (effect != null)
                Destroy(effect.gameObject);
            Destroy(this);
        }
    }
}