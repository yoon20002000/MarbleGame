using System;
using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.Pool;
using Assert = UnityEngine.Assertions.Assert;
using Random = UnityEngine.Random;

public class MarbleManager : MonoBehaviour
{
    private List<Marble> marbles = new List<Marble>();
    public IReadOnlyList<Marble> Marbles => marbles;
    private List<MarbleData> marblesData = new List<MarbleData>();
    public IReadOnlyList<MarbleData> MarblesData => marblesData;
    public int MarbleCount => marbles.Count;

    [SerializeField]
    private Transform marbleSpawnPoint;
    [SerializeField]
    private Transform marblePrefab;
    private ObjectPool<Marble> marblePool;
    
    [SerializeField]
    private SpawnPointManager spawnPointManager;

    [SerializeField] 
    private float donationPerAmount = 1000;
    private void Awake()
    {
        Assert.IsNotNull(spawnPointManager);
        
        marblePool = new ObjectPool<Marble>(
            createFunc: () =>
            {
                var go = Instantiate(marblePrefab, marbleSpawnPoint);
                return go.GetComponent<Marble>();
            },
            actionOnGet: marble =>
            {
                marble.gameObject.SetActive(true);
                marbles.Add(marble);
            },
            actionOnRelease: marble =>
            {
                marble.gameObject.SetActive(false);
                marbles.Remove(marble);
            },
            actionOnDestroy: marble =>
            {
                marbles.Remove(marble);
                Destroy(marble.gameObject);
            },
            collectionCheck: false,
            defaultCapacity: 10);
    }

    private void OnDestroy()
    {
        RemoveAllMarblesData();
    }

    public void AddMarbleData(string marbleName, string donor, bool isAnonymous, int  donationAmount, string msg)
    {
        for (int i = 0; i < donationAmount / donationPerAmount; ++i)
        {
            MarbleData marbleData = new MarbleData(marbles.Count, marbleName, GetRandomColor(),
                new DonationData(isAnonymous, donor, donationAmount, msg));
            marblesData.Add(marbleData);
            AddMarble(marbleData);    
        }
    }
    public Marble AddMarble(MarbleData marbleData)
    {
        Marble marble = marblePool.Get();
        marble.transform.SetLocalPositionAndRotation(spawnPointManager.GetSpawnLocalPosition(marbleData.MarbleID), Quaternion.identity);
        marble.Initialize(this, marbleData);
        return marble;
    }

    public void ForceRemoveMarble(Marble marble)
    {
        marbles.Remove(marble);
        marblePool.Release(marble);
    }
    public void RemoveMarble(Marble marble, bool bIsRacing = true)
    {
        marbles.Remove(marble);
        StartCoroutine(ReleaseAfterSeconds(marble, 3.0f));
        if (!bIsRacing)
        {
            UpdateAllMarblesPosition();    
        }
    }

    public void RemoveMarble(string marbleName, int marbleCount, bool bIsRacing = true)
    {
        int index = Mathf.Min(marbleCount - 1, marbles.Count - 1);
        for ( ; index >= 0; --index)
        {
            Marble removeMarble =  marbles[index];
            if (removeMarble.MarbleData.MarbleName.Equals(marbleName))
            {
                marbles.RemoveAt(index);
                marblePool.Release(removeMarble);
            }
        }
        if (!bIsRacing)
        {
            UpdateAllMarblesPosition();    
        }
    }

    public void RemoveAllMarbles()
    {
        StopAllMarbleManagerCoroutines();
        marbles.Clear();
        for (int i = marbles.Count - 1; i >= 0; --i)
        {
            marblePool.Release(marbles[i]);
        }
    }

    public void RemoveAllMarblesData()
    {
        RemoveAllMarbles();
        marblesData.Clear();
    }

    public void ResetMarblesPosition()
    {
        for (int i = 0; i < marbles.Count; ++i)
        {
            marbles[i].transform.position = spawnPointManager.GetSpawnLocalPosition(i);
        }
    }
    public void SetAllMarbleSimulate(bool isSimulated)
    {
        for (int i = 0; i < marbles.Count; ++i)
        {
            marbles[i].MarbleRigidbody.simulated = isSimulated;
        }
    }

    private Color GetRandomColor()
    {
        return new Color(Random.Range(0f, 1f), Random.Range(0f, 1f), Random.Range(0f, 1f));
    }

    public void UpdateAllMarblesPosition()
    {
        for (int i = 0; i < marbles.Count; ++i)
        {
            marbles[i].transform.localPosition = spawnPointManager.GetSpawnLocalPosition(i);
        }
    }

    private IEnumerator ReleaseAfterSeconds(Marble marble, float sec)
    {
        yield return Awaitable.WaitForSecondsAsync(sec);
        
        marblePool.Release(marble);
    }

    public void ResetMarbleManagerState()
    {
        StopAllCoroutines();
        RemoveAllMarbles();
        RemoveAllMarblesData();
    }
    
    public void StopAllMarbleManagerCoroutines()
    {
        StopAllCoroutines();
    }
}
