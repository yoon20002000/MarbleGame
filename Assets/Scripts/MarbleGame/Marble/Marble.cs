using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.Serialization;

public class Marble : MonoBehaviour
{
    private MarbleManager marbleManager;
    [HideInInspector]
    public MarbleData MarbleData { get; private set; }
    public Rigidbody2D MarbleRigidbody { get; private set; }
    [SerializeField]
    private SpriteRenderer spriteRenderer;
    
    private void Awake()
    {
        if (spriteRenderer == null)
        {
            spriteRenderer = GetComponent<SpriteRenderer>();
            
            if (spriteRenderer == null)
            {
                spriteRenderer = this.AddComponent<SpriteRenderer>();
            }
        }

        if (MarbleRigidbody == null)
        {
            MarbleRigidbody = GetComponent<Rigidbody2D>();
            Assert.IsNotNull(MarbleRigidbody, "MarbleRigidbody is not added.");
            if (MarbleRigidbody == null)
            {
                MarbleRigidbody = this.AddComponent<Rigidbody2D>();
            }
        }

        SetMarbleSimulated(false);
    }

    private void OnDestroy()
    {
        if (marbleManager != null)
        {
            marbleManager.RemoveMarble(this);    
        }
    }

    public void Initialize(MarbleManager inMarbleManager, MarbleData inMarbleData)
    {
        SetMarbleManager(inMarbleManager);
        SetMarbleData(inMarbleData);
        SetMarbleColor(inMarbleData.MarbleColor);
        SetMarbleSimulated(false);
    }

    public void SetMarbleManager(MarbleManager inMarbleManager)
    {
        marbleManager = inMarbleManager;
    }
    public void SetMarbleData(MarbleData inMarbleData)
    {
        MarbleData = inMarbleData;
    }
    
    public void SetMarbleColor(Color marbleColor)
    {
        spriteRenderer.color = marbleColor;
    }

    public void SetMarbleSimulated(bool isSimulate)
    {
        MarbleRigidbody.simulated = isSimulate;
    }
}
