using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnscriptedLogic.MathUtils;

public class ConeheadGFX : MonoBehaviour
{
    [SerializeField] private GameObject coneObject;
    [SerializeField] private TrailRenderer trailRenderer;
    private UnitBase unitbase;
    private bool called;

    private void Start()
    {
        unitbase = GetComponent<UnitBase>();
        unitbase.HealthHandler.OnModified += HealthHandler_OnModified;
        called = false;
    }

    private void HealthHandler_OnModified(object sender, UnscriptedLogic.Currency.CurrencyEventArgs e)
    {
        if (!called)
        {
            if (e.currentValue <= unitbase.MaxHealth * 0.5f)
            {
                called = true;

                coneObject.transform.SetParent(null);

                coneObject.GetComponent<MeshCollider>().enabled = true;
                Rigidbody rb = coneObject.GetComponent<Rigidbody>();
                rb.isKinematic = false;
                trailRenderer.enabled = true;

                float x = RandomLogic.BetFloats(-0.2f, 0.2f);
                float z = RandomLogic.BetFloats(-0.2f, 0.2f);
                Vector3 end = new Vector3(transform.position.x + x, transform.position.y + 1f, transform.position.z + z);
                Vector3 dir = (end - transform.position).normalized;

                rb.AddForce(dir * 10, ForceMode.Impulse);
                rb.AddTorque(dir * 10, ForceMode.Impulse);

                Destroy(coneObject, 10f);
            }
        }
    }
}
