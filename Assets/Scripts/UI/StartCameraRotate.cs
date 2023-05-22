using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartCameraRotate : MonoBehaviour
{
    [SerializeField] private float rotateSpeed;

    private void Update()
    {
        transform.RotateAround(Vector3.zero, Vector3.up, rotateSpeed * Time.deltaTime);
    }
}
