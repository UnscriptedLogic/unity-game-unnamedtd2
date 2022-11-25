using Core;
using System;
using UnitManagement;
using UnityEngine;

namespace ProjectileManagement
{
    public struct ProjectileSettings
    {
        public float speed;
        public float lifetime;

        public ProjectileSettings(float speed, float lifetime)
        {
            this.speed = speed;
            this.lifetime = lifetime;
        }
    }

    public class ProjectileBase : MonoBehaviour
    {
        [SerializeField] protected float speed = 1f;
        [SerializeField] protected float lifeTime = 1f;

        public event Action<UnitBase> OnEnemyHit;
        public event Action<ProjectileBase> OnProjectileDestroyed;

        protected float _lifetime;
        protected bool initialized;

        private void Update()
        {
            if (!initialized) return;

            if (_lifetime <= 0f)
            {
                PoolManager.poolManagerInstance.PushToPool(gameObject);
            }
            else
            {
                _lifetime -= Time.deltaTime;
            }

            Move();
        }

        public void InitializeAndSetActive(ProjectileSettings projectileSettings)
        {
            speed = projectileSettings.speed;
            lifeTime = projectileSettings.lifetime;
            _lifetime = lifeTime;
            initialized = true;

            gameObject.SetActive(true);
        }

        protected void Move()
        {
            transform.position += transform.forward * speed * Time.deltaTime;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Enemy"))
            {
                OnEnemyHit?.Invoke(other.GetComponent<UnitBase>());
                PoolManager.poolManagerInstance.PushToPool(gameObject);
            }
        }

        private void OnDisable()
        {
            OnProjectileDestroyed?.Invoke(this);
        }
    }
}