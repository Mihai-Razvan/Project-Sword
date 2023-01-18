using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class PickItem : MonoBehaviourPunCallbacks, IOnEventCallback
{
    [SerializeField] PhotonView view;

    [SerializeField] LayerMask itemMask;
    [SerializeField] Camera playerCamera;
    [SerializeField] float detectionCapsuleLength;
    [SerializeField] float detectionCapsuleRadius;
    [SerializeField] Transform handTransform;
    [SerializeField] GameObject rockPrefab;

    void Start()
    {

    }

    void Update()
    {
        if (!view.IsMine)
            return;

        checkPickableItem();
    }

    void checkPickableItem()
    {
        Collider[] colliders = Physics.OverlapCapsule(playerCamera.transform.position, playerCamera.transform.position + playerCamera.transform.forward * detectionCapsuleLength, detectionCapsuleRadius, itemMask);

        if (Input.GetKeyDown(KeyCode.E) && colliders.Length != 0)
        {
            pickCollectedItem(colliders[0].gameObject.GetComponent<PhotonView>().ViewID);
        }
    }

    void pickCollectedItem(int itemId)
    {
        object[] data = new object[]
            {
                view.ViewID, itemId           //view.ViewId is the playerId
            };

        RaiseEventOptions raiseEventOptions = new RaiseEventOptions
        {
            Receivers = ReceiverGroup.All,
            CachingOption = EventCaching.AddToRoomCache
        };

        ExitGames.Client.Photon.SendOptions sendOptions = new ExitGames.Client.Photon.SendOptions
        {
            Reliability = true
        };

        PhotonNetwork.RaiseEvent(EventsList.COLLECT_ITEM, data, raiseEventOptions, sendOptions);
    }

    public void OnEvent(ExitGames.Client.Photon.EventData eventData)
    {
        if (eventData.Code == EventsList.COLLECT_ITEM)
        {
            object[] data = (object[])eventData.CustomData;

            if(PhotonView.Find((int) data[1]) != null)       //we need to check because the object could get destroyed during propagation
                Destroy(PhotonView.Find((int) data[1]).gameObject);

            if(PhotonView.Find((int)data[0]) != null && (int) data[0] == view.ViewID)      //we need to check for null because the player could leave room during propagation
            {
                GameObject spawnedObject = Instantiate(rockPrefab, handTransform.position, Quaternion.identity);
                spawnedObject.transform.SetParent(handTransform);
            }
        }
    }
}
