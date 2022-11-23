using ProjectileManagement;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
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

        [Header("Base Tower Settings")]
        [SerializeField] protected float damage = 1f;
        [SerializeField] protected float range = 1f;
        [SerializeField] protected float reloadTime = 1f;

        [Header("Base Projectile Settings")]
        [SerializeField] protected float projectileSpeed = 10f;
        [SerializeField] protected float projectileLifetime = 3f;

        [Header("Base Components")]
        [SerializeField] protected Animator animator;
        [SerializeField] protected SphereCollider rangeCollider;
        [SerializeField] protected GameObject[] projectilePrefabs;
        [SerializeField] protected Transform[] rotationHeads;
        [SerializeField] protected Transform[] shootAnchors;

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
            rangeCollider.radius = range;
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
                if (!HasLOSToTarget(currentTarget))
                {
                    currentTarget = null;
                    OnTargetLost();
                    return;
                }

                if (Vector3.Distance(currentTarget.position, transform.position) >= range + 1f)
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

            //Vector3 direction = target.position - rotationHead.position;
            //Quaternion lookRotation = Quaternion.LookRotation(direction);
            //Vector3 rotation = Quaternion.Lerp(rotationHead.rotation, lookRotation, Time.deltaTime * 10f).eulerAngles;
            //rotationHead.rotation = Quaternion.Euler(0f, rotation.y, 0f);

            rotationHead.LookAt(targetPos);
        }

        protected GameObject CreateBullet()
        {
            GameObject bullet = Instantiate(projectilePrefabs[0], shootAnchors[0].position, shootAnchors[0].rotation);
            ProjectileSettings projectileSettings = new ProjectileSettings(projectileSpeed, damage, projectileLifetime);
            ProjectileBase projectileBase = bullet.GetComponent<ProjectileBase>();
            projectileBase.Initialize(projectileSettings);
            return bullet;
        }

        protected virtual void CommonTowerLogic()
        {
            if (_reloadTime <= 0f)
            {
                if (currentTarget != null)
                {
                    RotateToTarget(rotationHeads[0], levelled: true);
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
            rangeCollider.radius = range;
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
            Debug.Log("TargetFound()");
            animator.SetBool("Attacking", true);
        }

        protected virtual void WhileTargetFound()
        {
            Debug.Log("WhileTargetFound()");
        }

        protected virtual void OnTargetLost()
        {
            Debug.Log("TargetLost()");
            animator.SetBool("Attacking", false);
        }

        protected virtual void FireProjectile()
        {
            Debug.Log("FireProjectile()");

            CreateBullet();
        }

        protected virtual void OnProjectileFired()
        {
            Debug.Log("OnProjectileFired()");
        }

        protected virtual void OnProjectileHit()
        {
            Debug.Log("OnProjectileHit()");
        }
    }
}