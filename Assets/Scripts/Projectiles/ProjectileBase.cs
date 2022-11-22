using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ProjectileManagement
{
    public struct ProjectileSettings
    {
        public float speed;
        public float damage;
        public float lifetime;

        public ProjectileSettings(float speed, float damage, float lifetime)
        {
            this.speed = speed;
            this.damage = damage;
            this.lifetime = lifetime;
        }
    }

    public class ProjectileBase : MonoBehaviour
    {
        [SerializeField] protected float speed = 1f;
        [SerializeField] protected float damage = 1f;
        [SerializeField] protected float lifeTime = 1f;

        protected bool initialized;

        private void OnEnable()
        {
            initialized = false;
        }

        public void Initialize(ProjectileSettings projectileSettings)
        {
            speed = projectileSettings.speed;
            damage = projectileSettings.damage;
            lifeTime = projectileSettings.lifetime;

            Destroy(gameObject, lifeTime);
            initialized = true;
        }
    }
}