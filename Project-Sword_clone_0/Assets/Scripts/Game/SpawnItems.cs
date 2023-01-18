using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class SpawnItems : MonoBehaviourPunCallbacks, IOnEventCallback
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
            instantiateRock();
            timeSinceSpawned = 0;
        }
    }

    void instantiateRock()
    {
        float randX = Random.Range(-40, 40);
        float randz = Random.Range(-40, 40);
        GameObject spawnedObject = Instantiate(rockPrefab, new Vector3(randX, 4, randz), Quaternion.identity);

        PhotonView spawnedObjectView = spawnedObject.GetComponent<PhotonView>();

        if(PhotonNetwork.AllocateViewID(spawnedObjectView))
        {
            object[] data = new object[]
            {
                spawnedObject.transform.position, spawnedObject.transform.rotation, spawnedObjectView.ViewID
            };

            RaiseEventOptions raiseEventOptions = new RaiseEventOptions
            {
                Receivers = ReceiverGroup.Others,
                CachingOption = EventCaching.AddToRoomCache
            };

            ExitGames.Client.Photon.SendOptions sendOptions = new ExitGames.Client.Photon.SendOptions
            {
                Reliability = true
            };

            PhotonNetwork.RaiseEvent(EventsList.SPAWN_ITEM_EVENT, data, raiseEventOptions, sendOptions);
        }
        else
        {
            Debug.LogError("Failed to allocate a ViewId");
            Destroy(spawnedObject);
        }
    }

    public void OnEvent(ExitGames.Client.Photon.EventData eventData)
    {
        if (eventData.Code == EventsList.SPAWN_ITEM_EVENT)
        {
            object[] data = (object[])eventData.CustomData;

            GameObject spawnedObject = Instantiate(rockPrefab, (Vector3)data[0], (Quaternion)data[1]);
            PhotonView spawnedObjectView = spawnedObject.GetComponent<PhotonView>();
            spawnedObjectView.ViewID = (int)data[2];
        }
    }
}
