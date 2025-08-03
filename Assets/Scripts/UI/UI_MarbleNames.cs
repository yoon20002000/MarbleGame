using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class UI_MarbleNames : MonoBehaviour
{
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
                marbleName.gameObject?.SetActive(true);
            },
            actionOnRelease: marbleName =>
            {
                activedMarbleNames.Remove(marbleName);
                marbleName?.gameObject?.SetActive(false);
            },
            actionOnDestroy: marbleName =>
            {
                activedMarbleNames?.Remove(marbleName);
                Destroy(marbleName?.gameObject);
            },
            collectionCheck: false,
            defaultCapacity: 500);

        MarbleGameManager.Instance.OnMarbleAdd.AddListener(AddMarbleNameUI);
        MarbleGameManager.Instance.OnMarblePreRemove.AddListener(RemoveMarbleNameUI);
    }

    private void OnDestroy()
    {
        MarbleGameManager.Instance.OnMarbleAdd.RemoveListener(AddMarbleNameUI);
        MarbleGameManager.Instance.OnMarblePreRemove.RemoveListener(RemoveMarbleNameUI);
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