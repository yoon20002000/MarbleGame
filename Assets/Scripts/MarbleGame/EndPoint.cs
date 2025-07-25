using System;
using UnityEngine;
using UnityEngine.Assertions;

public class EndPoint : MonoBehaviour
{
    // [SerializeField]
    // private GameManager gameManager;
    [SerializeField]
    private BoxCollider2D boxCollider;

    private void Awake()
    {
        if (boxCollider == null)
        {
            boxCollider = GetComponent<BoxCollider2D>();
            if (boxCollider == null)
            {
                Assert.IsNull(boxCollider, "BoxCollider not set");
                boxCollider = gameObject.AddComponent<BoxCollider2D>();
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!MarbleGameManager.Instance.IsRacing)
        {
            return;
        }
        
        MarbleGameManager.Instance.EnterEndPoint(other.gameObject.GetComponent<Marble>());
    }
}
