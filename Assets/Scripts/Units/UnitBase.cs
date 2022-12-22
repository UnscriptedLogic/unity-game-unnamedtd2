using Core;
using GameManagement;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UnitManagement
{
    public class UnitBase : MonoBehaviour, IRequiresPath, IUsesUnitEvent
    {
        [Header("Base Stats")]
        [SerializeField] protected string id;
        [SerializeField] protected float health = 5f;
        [SerializeField] protected float speed = 5f;

        [Header("Base Settings")]
        [SerializeField] protected float baseOffset = 1f;
        [SerializeField] protected bool faceDir;

        protected float currentHealth;
        protected Vector3[] nodes;

        protected int waypointCounter;
        protected bool isAlive;

        private UnitEventHandler unitEventHandler;

        public Action<float> OnUnitTookDamage;
        public Action<UnitBase> OnUnitCompletedPath;

        public string ID => id;
        public float MaxHealth => health;
        public float Speed => speed;
        public float CurrentHealth => currentHealth;

        protected virtual void OnEnable()
        {
            currentHealth = health;
        }

        protected virtual void Update()
        {
            if (!isAlive) return;

            ApplyMovement();
        }

        public void InitWithPath(Vector3[] nodes)
        {
            this.nodes = nodes;
            isAlive = true;
            transform.position = nodes[0] + Vector3.up * baseOffset;
            waypointCounter = 0;
        }

        protected void ApplyMovement()
        {
            if (!isAlive) return;

            if (waypointCounter >= nodes.Length)
            {
                OnUnitCompletedPath?.Invoke(this);
                LevelManagement.PushObject(gameObject);
                return;
            }

            Vector3 direction = GetWaypoint(waypointCounter) - transform.position;
            transform.Translate(direction.normalized * speed * Time.deltaTime, Space.World);
            if (faceDir)
            {
                transform.forward = direction;
            }

            if (Vector3.Distance(transform.position, GetWaypoint(waypointCounter)) <= GetWaypointVerifyDistance())
                waypointCounter++;
        }

        protected float GetWaypointVerifyDistance()
        {
            Vector2 checkRange = new Vector2(0.1f, 0.5f);
            Vector2 speedRange = new Vector2(1f, 9f);

            if (speed <= speedRange.x)
            {
                return checkRange.x;
            }
            else if (speed >= speedRange.y)
            {
                return checkRange.y;
            } else
            {
                return checkRange.y / speedRange.y * speed;
            }
        }

        protected Vector3 GetWaypoint(int index)
        {
            Vector3 position = nodes[index];
            position.y = transform.position.y;
            return position;
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
            OnUnitTookDamage?.Invoke(damage);
            unitEventHandler.UnitHealthModified(ModificationType.Subtract, id, damage, currentHealth);
        }

        protected virtual void OnUnitDeath()
        {
            isAlive = false;
            LevelManagement.PushObject(gameObject);
        }

        public void DestroyUnit()
        {

        }

        public void InitWithUnitEventHandler(UnitEventHandler unitEventHandler)
        {
            this.unitEventHandler = unitEventHandler;
        }
    }
}