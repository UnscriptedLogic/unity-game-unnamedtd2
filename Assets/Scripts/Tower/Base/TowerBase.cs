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

        [Header("Base Settings")]
        [SerializeField] protected float damage = 1f;
        [SerializeField] protected float range = 1f;
        [SerializeField] protected float reloadTime = 1f;

        [Header("Base Components")]
        [SerializeField] protected SphereCollider rangeCollider;
        [SerializeField] protected Transform[] rotationHeads;
        [SerializeField] protected Transform[] shootAnchors;

        [Header("Base Settings")]
        [SerializeField] protected bool drawGizmos;
        [SerializeField] protected float losBaseOffset = 0.5f;
        [SerializeField] protected LayerMask losObstructionLayer;
        [SerializeField] protected LayerMask unitLayer;
        [SerializeField] protected TargetSortMode targetSortMode = TargetSortMode.First;

        protected Transform currentTarget;
        protected List<Transform> targetsInRange = new List<Transform>();

        protected float _reloadTime;

        protected virtual void Start()
        {
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
                }

                if (Vector3.Distance(currentTarget.position, transform.position) >= range + 1f)
                {
                    currentTarget = null;
                    OnTargetLost();
                }
            }

            if (targetsInRange.Count > 0)
            {
                targetsInRange.Clear();
            }

            Collider[] colliders = Physics.OverlapSphere(transform.position, range, unitLayer);
            for (int i = 0; i < colliders.Length; i++)
            {
                targetsInRange.Add(colliders[i].transform);

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
                    break;
                case TargetSortMode.Furthest:
                    //Furthest distance to tower
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

            target = targetsInRange[0];

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

        protected void CommonTowerFunctionality()
        {

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
            Debug.Log("OnTargetFound()");
        }

        protected virtual void WhileTargetFound()
        {
            Debug.Log("WhileTargetFound()");
        }

        protected virtual void OnTargetLost()
        {
            Debug.Log("OnTargetLost()");
        }

        protected virtual void FireProjectile()
        {
            Debug.Log("FireProjectile()");
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