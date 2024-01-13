using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PeristentObjectSpawner : MonoBehaviour
{
    [SerializeField] GameObject persistentObjectPrefab;

    static bool hasSpawned = false;

    private void Awake()
    {
        if (hasSpawned) return;
        SpawnPeristentObject();
        hasSpawned = true;
    }

    private void SpawnPeristentObject()
    {
        // ������Ʈ ����
        GameObject persistentObject = Instantiate(persistentObjectPrefab);
        DontDestroyOnLoad(persistentObject);
    }
}
