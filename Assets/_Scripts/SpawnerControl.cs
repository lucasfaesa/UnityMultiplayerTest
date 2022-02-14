using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using Random = UnityEngine.Random;

public class SpawnerControl : NetworkBehaviour
{
    [SerializeField] private GameObject objectPrefab;
    [SerializeField] private int maxObjectInstanceCount = 3;

    private void SpawnObjects(float x, float z)
    {
        //if (!IsServer) return;
        
        Debug.Log("Spawning");
        GameObject go = Instantiate(objectPrefab, new Vector3(x, 10f, z), Quaternion.identity);
    }

    [ServerRpc]
    public void SpawnObjectsServerRpc()
    {
        for (int i = 0; i < maxObjectInstanceCount; i++)
        {
            float x = Random.Range(-10, 10);
            float y = Random.Range(-10, 10);
            
            SpawnObjectsClientRpc(x, y);
            
            Debug.Log("Random Range X: " + x + "Random Range Z: " + y);
        }
    }

    [ClientRpc]
    private void SpawnObjectsClientRpc(float x, float z)
    {
        SpawnObjects(x,z);
    }

}
