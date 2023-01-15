using Core;
using ProjectileManagement;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnitManagement;
using UnityEngine;

namespace TowerManagement
{
    public class TowerBase : MonoBehaviour
    {
        public enum TargetSortMode
        {
            First,
            Last,
            Closest,
            Furthest,
            Strongest,
            Weakest
        }

        [Serializable]
        public class RotationHeads
        {
            public Transform rotationHead;
            public bool levelled;
        }

        [Header("Base Tower Settings")]
        public float damage = 1f;
        public float range = 1f;
        public float reloadTime = 1f;

        [Header("Base Projectile Settings")]
        public float projectileSpeed = 10f;
        public float projectileLifetime = 3f;
        public int pierce = 1;

        [Header("Base Components")]
        [SerializeField] protected Animator animator;
        [SerializeField] protected GameObject[] projectilePrefabs;
        [SerializeField] protected RotationHeads[] rotationHeads;
        [SerializeField] protected Transform[] shootAnchors;

        [Space(10)]
        [SerializeField] protected AudioClip[] audioClips;

        [Header("Base Settings")]
        [SerializeField] protected bool drawGizmos;
        [SerializeField] protected float losBaseOffset = 0.5f;
        [SerializeField] protected LayerMask losObstructionLayer;
        [SerializeField] protected LayerMask unitLayer;
        [SerializeField] protected TargetSortMode targetSortMode = TargetSortMode.First;

        protected float _reloadTime;

        protected Transform currentTarget;
        protected List<Transform> targetsInRange = new List<Transform>();

        protected virtual void Start()
        {
            //rangeCollider.radius = range;
            _reloadTime = reloadTime;
        }

        protected virtual void Update()
        {
            if (currentTarget != null)
            {
                WhileTargetFound();
            }
        }

        protected virtual void FixedUpdate()
        {
            if (currentTarget != null)
            {
                if (LostTarget())
                {
                    currentTarget = null;
                    OnTargetLost();
                    return;
                }
            }

            if (targetsInRange.Count > 0)
            {
                targetsInRange.Clear();
            }

            Collider[] colliders = Physics.OverlapSphere(transform.position, range, unitLayer);
            for (int i = 0; i < colliders.Length; i++)
            {
                if (HasLOSToTarget(colliders[i].transform))
                {
                    targetsInRange.Add(colliders[i].transform);
                }
            }

            if (targetsInRange.Count > 0)
            {
                SortTargets();
            }
        }

        protected void SortTargets()
        {
            Transform target = null;

            switch (targetSortMode)
            {
                case TargetSortMode.First:
                    //Node closest to the end point
                    break;
                case TargetSortMode.Last:
                    //Node furthest from the end point
                    break;
                case TargetSortMode.Closest:
                    //Closest distance to tower
                    target = targetsInRange.OrderBy(t => Vector3.Distance(t.position, transform.position)).FirstOrDefault();
                    break;
                case TargetSortMode.Furthest:
                    //Furthest distance to tower
                    target = targetsInRange.OrderByDescending(t => Vector3.Distance(t.position, transform.position)).FirstOrDefault();
                    break;
                case TargetSortMode.Strongest:
                    //Highest health point in range
                    break;
                case TargetSortMode.Weakest:
                    //Lowest health point in range
                    break;
                default:
                    break;
            }

            if (currentTarget == null || target != currentTarget)
            {
                currentTarget = target;
                OnTargetFound();
            }
        }

        protected bool HasLOSToTarget(Transform target)
        {
            return !Physics.Raycast(transform.position + Vector3.up * losBaseOffset, target.position - transform.position, out RaycastHit hit, range, losObstructionLayer);
        }

        protected void RotateToTarget(Transform rotationHead, Transform target = null, bool levelled = false)
        {
            if (target == null)
            {
                target = currentTarget;
            }

            if (currentTarget == null)
            {
                return;
            }

            Vector3 targetPos = target.position;
            if (levelled)
            {
                targetPos.y = rotationHead.position.y;
            }

            rotationHead.LookAt(targetPos);
        }
        
        protected bool LostTarget()
        {
            if (!currentTarget.gameObject.activeInHierarchy)
            {
                return true;
            }

            if (!HasLOSToTarget(currentTarget))
            {
                return true;
            }

            if (Vector3.Distance(currentTarget.position, transform.position) >= range + 1f)
            {
                return true;
            }

            return false;
        }

        public void RotateTargettingForward()
        {

        }

        public void RotateTargettingBackward()
        {

        }

        protected GameObject CreateBullet(out ProjectileBase projectileBase, GameObject prefab, Transform anchor, ProjectileBehaviour projectileBehaviour = null)
        {
            GameObject bullet = PoolManager.poolManagerInstance.PullFromPool(prefab, anchor.position, anchor.rotation, false);
            ProjectileSettings projectileSettings = new ProjectileSettings(projectileSpeed, projectileLifetime, pierce);
            projectileBase = bullet.GetComponent<ProjectileBase>();

            projectileBase.InitializeAndSetActive(projectileSettings, projectileBehaviour);
            return bullet;
        }

        protected GameObject CreateBullet(out ProjectileBase projectileBase, GameObject prefab, Vector3 position, Quaternion rotation, ProjectileBehaviour projectileBehaviour = null)
        {
            GameObject bullet = PoolManager.poolManagerInstance.PullFromPool(prefab, position, rotation, false);
            ProjectileSettings projectileSettings = new ProjectileSettings(projectileSpeed, projectileLifetime, pierce);
            projectileBase = bullet.GetComponent<ProjectileBase>();

            projectileBase.InitializeAndSetActive(projectileSettings, projectileBehaviour);
            return bullet;
        }

        protected GameObject CreateBullet(out ProjectileBase projectileBase, GameObject prefab, Vector3 position, Quaternion rotation, ProjectileSettings projectileSettings, ProjectileBehaviour projectileBehaviour = null)
        {
            GameObject bullet = PoolManager.poolManagerInstance.PullFromPool(prefab, position, rotation, false);
            projectileBase = bullet.GetComponent<ProjectileBase>();

            projectileBase.InitializeAndSetActive(projectileSettings, projectileBehaviour);
            return bullet;
        }

        protected virtual void SubscribeProjectileEvents(ProjectileBase projectileBase)
        {
            projectileBase.OnEnemyHit += OnProjectileHit;
            projectileBase.OnProjectileDestroyed += OnProjectileDestroyed;
        }

        protected virtual void UnsubscribeProjectileEvents(ProjectileBase projectileBase)
        {
            projectileBase.OnEnemyHit -= OnProjectileHit;
            projectileBase.OnProjectileDestroyed -= OnProjectileDestroyed;
        }

        protected virtual void CommonTowerLogic()
        {
            if (_reloadTime <= 0f)
            {
                if (currentTarget != null)
                {
                    for (int i = 0; i < rotationHeads.Length; i++)
                    {
                        RotateToTarget(rotationHeads[i].rotationHead, levelled: rotationHeads[i].levelled);
                    }

                    FireProjectile();
                    _reloadTime = reloadTime;
                }
            }
            else
            {
                _reloadTime -= Time.deltaTime;
            }
        }

        protected virtual void OnValidate()
        {
            //rangeCollider.radius = range;
        }

        protected virtual void OnDrawGizmos()
        {
            if (!drawGizmos) return;

            Gizmos.DrawSphere(transform.position + Vector3.up * losBaseOffset, 0.1f);

            for (int i = 0; i < targetsInRange.Count; i++)
            {
                Gizmos.color = HasLOSToTarget(targetsInRange[i].transform) ? Color.green : Color.red;
                Gizmos.DrawLine(transform.position + Vector3.up * losBaseOffset, targetsInRange[i].position);
            }
        }

        protected virtual void OnTargetFound()
        {
            //Debug.Log("TargetFound()");
            animator.SetBool("Attacking", true);
        }

        protected virtual void WhileTargetFound()
        {
            //Debug.Log("WhileTargetFound()");
            CommonTowerLogic();
        }

        protected virtual void OnTargetLost()
        {
            //Debug.Log("TargetLost()");
            animator.SetBool("Attacking", false);
        }

        protected virtual void FireProjectile()
        {
            //Debug.Log("FireProjectile()");

            CreateBullet(out ProjectileBase projectileBase, projectilePrefabs[0], shootAnchors[0]);
            SubscribeProjectileEvents(projectileBase);
        }

        protected virtual void OnProjectileFired()
        {
            //Debug.Log("OnProjectileFired()");
        }

        protected virtual void OnProjectileHit(UnitBase unit, ProjectileBase projectileBase)
        {
            //Debug.Log("OnProjectileHit()");
            unit.TakeDamage(damage);
        }

        protected virtual void OnProjectileDestroyed(ProjectileBase projectile)
        {
            UnsubscribeProjectileEvents(projectile);
        }
    }
}