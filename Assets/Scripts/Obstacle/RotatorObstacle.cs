using System;
using UnityEngine;
using UnityEngine.Assertions;

public class RotatorObstacle : MonoBehaviour
{
    [SerializeField]
    private float rotateAngle = -15f;
    [SerializeField]
    private float rotationSpeed = 15f;

    [SerializeField]
    private Transform rotateTargetTransform;

    private void Awake()
    {
        Assert.IsNotNull(rotateTargetTransform);
    }

    private void FixedUpdate()
    {
        rotateTargetTransform.Rotate(Vector3.forward, rotateAngle * Time.deltaTime * rotationSpeed);
    }
}
