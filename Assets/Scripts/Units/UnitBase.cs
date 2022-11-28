using Core;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UnitManagement
{
    public class UnitBase : MonoBehaviour
    {
        [SerializeField] protected float health = 5f;
        [SerializeField] protected float speed = 5f;

        protected float currentHealth;

        protected virtual void Start()
        {
            currentHealth = health;
        }

        public void TakeDamage(float damage)
        {
            currentHealth -= damage;
            OnUnitDamaged(damage);
            if (currentHealth <= 0f)
            {
                currentHealth = 0f;
                OnUnitDeath();
                return;
            }
        }

        protected virtual void OnUnitDamaged(float damage)
        {
            //Damage flashing or something
        }

        protected virtual void OnUnitDeath()
        {
            PoolManager.poolManagerInstance.PushToPool(gameObject);
        }
    }
}