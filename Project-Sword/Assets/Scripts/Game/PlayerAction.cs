using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class PlayerAction : MonoBehaviourPunCallbacks, IOnEventCallback
{
    [SerializeField] PhotonView view;
    [SerializeField] GameObject hand;
    [SerializeField] GameObject throwRockPrefab;

    void Start()
    {
        
    }

    void Update()
    {
        if (!view.IsMine)
            return;

        if (Input.GetKeyDown(KeyCode.Mouse0) && hand.transform.childCount != 0)
            throwRock();
    }

    void throwRock()
    {
        Destroy(hand.transform.GetChild(0).gameObject);    //destroy hand rock

        GameObject thrownRock = Instantiate(throwRockPrefab, this.transform.position + transform.forward * 2, this.gameObject.transform.rotation);
        PhotonView thrownRockView = thrownRock.GetComponent<PhotonView>();

        if (PhotonNetwork.AllocateViewID(thrownRockView))
        {
            object[] data = new object[]
            {
                thrownRock.transform.position, thrownRock.transform.rotation , thrownRockView.ViewID, view.ViewID
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

            PhotonNetwork.RaiseEvent(EventsList.THROW_ROCK, data, raiseEventOptions, sendOptions);
        }
        else
        {
            Debug.LogError("Failed to allocate a ViewId");
            Destroy(thrownRock);
        }
    }

    public void OnEvent(ExitGames.Client.Photon.EventData eventData)
    {
        if (eventData.Code == EventsList.THROW_ROCK)
        {
            object[] data = (object[])eventData.CustomData;

            if (PhotonView.Find((int)data[3]) != null && (int) data[3] == view.ViewID)       //we need to check because the object could get destroyed during propagation, or player leave room
            {
                Destroy(hand.transform.GetChild(0).gameObject);
                GameObject spawnedRock = Instantiate(throwRockPrefab, (Vector3)data[0], (Quaternion)data[1]);
                PhotonView spawnedRockView = spawnedRock.GetComponent<PhotonView>();
                spawnedRockView.ViewID = (int)data[2];
            }
        }
    }
}
