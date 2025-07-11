using System;
using UnityEngine;
using UnityEngine.Assertions;

public class EndPoint : MonoBehaviour
{
    [SerializeField]
    private BoxCollider2D boxCollider;

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

    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log(gameObject.name + " entered");
    }
}
