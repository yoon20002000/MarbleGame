using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class UI_MarbleNames : MonoBehaviour
{
    [SerializeField]
    private GameManager gameManager;
    [SerializeField] private RectTransform marbleNameContent;

    [SerializeField] private Transform marbleNamePrefab;
    private ObjectPool<UI_MarbleName> marbleNamePool;
    private List<UI_MarbleName> activedMarbleNames = new List<UI_MarbleName>(100);

    private void Awake()
    {
        marbleNamePool = new ObjectPool<UI_MarbleName>(
            createFunc: () =>
            {
                var go = Instantiate(marbleNamePrefab, marbleNameContent);
                return go.GetComponent<UI_MarbleName>();
            },
            actionOnGet: marbleName =>
            {
                activedMarbleNames.Add(marbleName);
                marbleName.gameObject.SetActive(true);
            },
            actionOnRelease: marbleName =>
            {
                activedMarbleNames.Remove(marbleName);
                marbleName.gameObject.SetActive(false);
            },
            actionOnDestroy: marbleName =>
            {
                activedMarbleNames.Remove(marbleName);
                Destroy(marbleName.gameObject);
            },
            collectionCheck: false,
            defaultCapacity: 500);
        
        gameManager.MarbleManager.OnMarbleAdded.AddListener(AddMarbleNameUI);
        gameManager.MarbleManager.OnMarblePreRemove.AddListener(RemoveMarbleNameUI);
    }
    
    private void AddMarbleNameUI(Marble marble)
    {
        UI_MarbleName marbleNameUI = marbleNamePool.Get();
        marbleNameUI.SetMarble(marble);
    }
    
    private void RemoveMarbleNameUI(Marble marble)
    {
        UI_MarbleName targetUI = activedMarbleNames.Find(x => x.TargetMarble == marble);
        marbleNamePool.Release(targetUI);
    }
}