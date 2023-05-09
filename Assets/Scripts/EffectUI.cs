using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnscriptedLogic.MathUtils;

public class EffectUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI amountTMP;
    [SerializeField] private Rigidbody rb;
    [SerializeField] private float force;
    [SerializeField] private AnimationCurve scaleOverTime;
    [SerializeField] private float animationTime;

    private float currentAnimationTime;
    private Transform target;

    public TextMeshProUGUI AmountTMP => amountTMP;

    private void Start()
    {
        target = Camera.main.transform;

        float x = RandomLogic.BetFloats(-0.2f, 0.2f);
        float z = RandomLogic.BetFloats(-0.2f, 0.2f);
        Vector3 end = new Vector3(transform.position.x + x, transform.position.y + 1f, transform.position.z + z);
        Vector3 dir = (end - transform.position).normalized;

        rb.AddForce(dir * force, ForceMode.Impulse);
    }

    private void Update()
    {
        transform.LookAt(target);

        currentAnimationTime += Time.deltaTime * animationTime;
        float scale = scaleOverTime.Evaluate(currentAnimationTime);
        transform.localScale = Vector3.one * scale;
    }
}
