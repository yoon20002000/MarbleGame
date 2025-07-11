using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.Serialization;

public class SpawnPointManager : MonoBehaviour
{
    [SerializeField]
    private Vector2 spawnPrefabSize = default;

    [FormerlySerializedAs("spawnVerticalOffset")] [SerializeField]
    private Vector2 spawnVerticalSpace;
    
    [SerializeField]
    private List<Transform> spawnPoints;
    public int HorizontalCount => spawnPoints.Count;

    
    private void Awake()
    {
        Assert.IsTrue(spawnPoints != null && spawnPoints.Count >= 0);
    }

    public Vector3 GetSpawnLocalPosition(int index)
    {
        int spawnIndex = index % HorizontalCount;
        
        Vector3 spawnPosition = spawnPoints[spawnIndex].localPosition;
        
        int rowIndex = index / HorizontalCount;

        spawnPosition.y += (rowIndex * spawnPrefabSize.y) + (spawnVerticalSpace.y * rowIndex);
        
        return spawnPosition;
    }
}
