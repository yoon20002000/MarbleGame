using System;
using UnityEngine;
using UnityEngine.Assertions;

public class EndPoint : MonoBehaviour
{
    [SerializeField]
    private BoxCollider2D boxCollider;

    [SerializeField]
    private LayerMask triggerLayerMask;
    private void Awake()
    {
        if (boxCollider == null)
        {
            boxCollider = GetComponent<BoxCollider2D>();
            Assert.IsNull(boxCollider, "BoxCollider not set");
            if (boxCollider == null)
            {
                boxCollider = gameObject.AddComponent<BoxCollider2D>();
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer != triggerLayerMask)
        {
            return;
        }
    }
}
