using Core;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UnitManagement
{
    public class UnitBase : MonoBehaviour, IRequiresPath
    {
        [Header("Base Stats")]
        [SerializeField] protected float health = 5f;
        [SerializeField] protected float speed = 5f;

        [Header("Base Settings")]
        [SerializeField] protected float baseOffset = 1f;
        [SerializeField] protected bool faceDir;

        protected float currentHealth;
        protected Vector3[] nodes;

        protected int waypointCounter;
        protected bool isAlive;

        public float MaxHealth => health;
        public float Speed => speed;
        public float CurrentHealth => currentHealth;

        protected virtual void Start()
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
            Debug.Log("Unit Initialized");
        }

        protected void ApplyMovement()
        {
            if (waypointCounter >= nodes.Length)
                return;

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
        }

        protected virtual void OnUnitDeath()
        {
            PoolManager.poolManagerInstance.PushToPool(gameObject);
        }

        public void DestroyUnit()
        {

        }
    }
}