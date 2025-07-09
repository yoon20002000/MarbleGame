using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class MarbleManager : MonoBehaviour
{
    private List<Marble> marbles = new List<Marble>();
    public IReadOnlyList<Marble> Marbles => marbles;

    [SerializeField]
    private Transform marbleSpawnPoint;
    [SerializeField]
    private Transform marblePrefab;
    private ObjectPool<Marble> marblePool;

    private void Awake()
    {
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

    public Marble AddMarbleData(string marbleName, string donor, bool isAnonymous, int  donationAmount, string msg)
    {
        Marble marble = AddMarble(new MarbleData(marbles.Count, marbleName, GetRandomColor(),
            new DonationData(isAnonymous, donor, donationAmount, msg)));
        return marble;
    }
    public Marble AddMarble(MarbleData marbleData)
    {
        Marble marble = marblePool.Get();

        marble.Initialize(this, marbleData);
        return marble;
    }

    public void RemoveMarble(Marble marble)
    {
        marbles.Remove(marble);
        marblePool.Release(marble);
    }

    private Color GetRandomColor()
    {
        return new Color(Random.Range(0f, 1f), Random.Range(0f, 1f), Random.Range(0f, 1f));
    }
}
