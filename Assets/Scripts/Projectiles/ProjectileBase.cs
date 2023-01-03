using Core;
using System;
using System.Collections.Generic;
using UnitManagement;
using UnityEngine;

namespace ProjectileManagement
{
    public struct ProjectileSettings
    {
        public int pierce;
        public float speed;
        public float lifetime;
        public ProjectileBehaviour projectileBehaviour;

        public ProjectileSettings(float speed, float lifetime, int pierce, ProjectileBehaviour projectileBehaviour = null)
        {
            this.pierce = pierce;
            this.speed = speed;
            this.lifetime = lifetime;
            this.projectileBehaviour = projectileBehaviour;
        }
    }
    
    public class ProjectileBase : MonoBehaviour
    {
        public const string SHRAPNEL_TAG = "Shrapnel";

        protected int pierce;
        protected float speed = 1f;
        protected float lifeTime = 1f;
        [SerializeField] protected TrailRenderer[] trailRenderers;
        
        public event Action<UnitBase, ProjectileBase> OnEnemyHit;
        public event Action<ProjectileBase> OnProjectileDestroyed;

        public ProjectileBehaviour projectileBehaviour;
        public Dictionary<string, int> tags;

        protected float _lifetime;
        protected bool initialized;
        protected int _pierce;

        public float Speed => speed;
        public int Pierce => pierce;
        public int CurrentPierce => _pierce;

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

            projectileBehaviour.Move(this);
        }

        public void InitializeAndSetActive(ProjectileSettings projectileSettings, ProjectileBehaviour projectileBehaviour = null)
        {
            tags = new Dictionary<string, int>();

            _pierce = 0;
            pierce = projectileSettings.pierce;
            speed = projectileSettings.speed;
            lifeTime = projectileSettings.lifetime;
            _lifetime = lifeTime;
            initialized = true;

            if (projectileBehaviour == null)
            {
                this.projectileBehaviour = new ProjectileBehaviour();
            } else
            {
                this.projectileBehaviour = projectileBehaviour;
            }

            gameObject.SetActive(true);
        }

        public void IncreasePierce() => _pierce++;
        
        private void OnTriggerEnter(Collider other)
        {
            projectileBehaviour.OnHit(other, this, OnEnemyHit);
        }

        private void OnEnable()
        {
            for (int i = 0; i < trailRenderers.Length; i++)
            {
                trailRenderers[i].Clear();
            }
        }

        private void OnDisable()
        {
            OnProjectileDestroyed?.Invoke(this);
            tags = new Dictionary<string, int>();
        }
    }
}