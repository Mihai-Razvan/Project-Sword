using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class SpawnPlayer : MonoBehaviour
{
    [SerializeField] GameObject playerPrefab;

    void Start()
    {
        float randVal = Random.Range(-30, 30);
        PhotonNetwork.Instantiate(playerPrefab.name, new Vector3(randVal, 4, randVal), Quaternion.identity);
    }
}
