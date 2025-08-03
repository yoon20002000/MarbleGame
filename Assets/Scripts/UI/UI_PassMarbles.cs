using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class UI_PassMarbles : MonoBehaviour
{
    [SerializeField] private RectTransform marblePassContent;

    [SerializeField] private Transform marblePassPrefab;
    
    private ObjectPool<UI_PassMarble> passMarblePool;
    private List<UI_PassMarble> activedPassMarbles = new List<UI_PassMarble>(100);

    private void Awake()
    {
        passMarblePool = new ObjectPool<UI_PassMarble>(
            createFunc: () =>
            {
                var go = Instantiate(marblePassPrefab, marblePassContent);
                return go.GetComponent<UI_PassMarble>();
            },
            actionOnGet: marbleName =>
            {
                activedPassMarbles.Add(marbleName);
            },
            actionOnRelease: marbleName =>
            {
                activedPassMarbles.Remove(marbleName);
                marbleName?.gameObject?.SetActive(false);
            },
            actionOnDestroy: marbleName =>
            {
                activedPassMarbles?.Remove(marbleName);
                Destroy(marbleName?.gameObject);
            },
            collectionCheck: false,
            defaultCapacity: 500);

        // 마블 통과 delegate 추가 필요
        MarbleGameManager.Instance.OnGameStart.AddListener(OnGameStart);
        MarbleGameManager.Instance.OnMarblePassEndPoint.AddListener(OnMarblePassEndPoint);
        
    }


    private void OnGameStart(List<Marble> marbles)
    {
        for (int i = activedPassMarbles.Count - 1; i >= 0; --i)
        {
            passMarblePool.Release(activedPassMarbles[i]);
        }
        activedPassMarbles.Clear();
    }
    
    private void OnMarblePassEndPoint(int marbleRank, Marble passMarble)
    {
        UI_PassMarble passMarbleUI = passMarblePool.Get();
        passMarbleUI.gameObject.SetActive(false);
        passMarbleUI.UpdateUI(marbleRank, passMarble.MarbleData.MarbleName);
        passMarbleUI.gameObject.SetActive(true);
    }
}
