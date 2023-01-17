using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class SpawnItems : MonoBehaviour
{
    [SerializeField] GameObject rockPrefab;
    float timeSinceSpawned;

    private void Update()
    {
        if (!PhotonNetwork.IsMasterClient)
            return;

        timeSinceSpawned += Time.deltaTime;

        if(timeSinceSpawned >= 3)
        {
            float randVal = Random.Range(-40, 40);
            PhotonNetwork.InstantiateRoomObject(rockPrefab.name, new Vector3(randVal, 4, randVal), Quaternion.identity);
            timeSinceSpawned = 0;
        }
    }

}
