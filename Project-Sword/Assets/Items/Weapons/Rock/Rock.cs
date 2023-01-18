using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class Rock : MonoBehaviourPunCallbacks, IOnEventCallback
{
    [SerializeField] PhotonView view;

    [SerializeField] float speed;
    [SerializeField] LayerMask playerMask;
    [SerializeField] float collisionRadius;

    void Start()
    {
        
    }

    void Update()
    {
        transform.position = transform.position + transform.forward * speed * Time.deltaTime;

        if (!PhotonNetwork.IsMasterClient)
            return;

        checkCollision();
    }

    void checkCollision()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, collisionRadius, playerMask);

        if(colliders.Length != 0)
        {
            int hitPlayerViewId = colliders[0].gameObject.GetComponent<PhotonView>().ViewID;
            hitPlayer(hitPlayerViewId);
        }
    }

    void hitPlayer(int playerViewId)
    {
        object[] data = new object[]
        {
                playerViewId, view.ViewID         //view.ViewId is the rock viewId
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

        PhotonNetwork.RaiseEvent(EventsList.ROCK_HIT_PLAYER, data, raiseEventOptions, sendOptions);

        Destroy(this.gameObject);
        GameObject player = PhotonView.Find(playerViewId).gameObject;
        player.GetComponent<PlayerData>().changePlayerHealth(-30);
    }

    public void OnEvent(ExitGames.Client.Photon.EventData eventData)
    {
        if (eventData.Code == EventsList.ROCK_HIT_PLAYER)
        {
            object[] data = (object[])eventData.CustomData;
          

            if (PhotonView.Find((int)data[1]) != null && (int)data[1] == view.ViewID)       //this rock
            {
                if (PhotonView.Find((int)data[0]) != null)    //the player that was hit
                {
                    GameObject player = PhotonView.Find((int)data[0]).gameObject;
                    player.GetComponent<PlayerData>().changePlayerHealth(-30);
                }

                Destroy(this.gameObject);
            }
        }
    }
}
