using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Assertions;

public class Marble : MonoBehaviour
{
    private MarbleManager marbleManager;
    private MarbleData marbleData;
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
    }

    public void SetMarbleManager(MarbleManager inMarbleManager)
    {
        marbleManager = inMarbleManager;
    }
    public void SetMarbleData(MarbleData inMarbleData)
    {
        marbleData = inMarbleData;
    }
    
    public void SetMarbleColor(Color marbleColor)
    {
        spriteRenderer.color = marbleColor;
    }
}
