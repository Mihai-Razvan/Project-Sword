using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Photon.Pun;
using Photon.Realtime;

public class PlayerData : MonoBehaviourPunCallbacks
{
    PhotonView view;
    ExitGames.Client.Photon.Hashtable dataHashTable;

    [SerializeField] TextMeshProUGUI playerNameText;

    void Start()
    {
        view = this.gameObject.GetComponent<PhotonView>();

        if(view.IsMine)
        {
            string playerName = "Player " + PhotonNetwork.CurrentRoom.PlayerCount.ToString();
            playerNameText.text = playerName;
            PhotonNetwork.LocalPlayer.CustomProperties.Add("PlayerName", playerName);
        }
        else
        {
            if (PhotonNetwork.LocalPlayer.CustomProperties.ContainsKey("PlayerName"))
                playerNameText.text = PhotonNetwork.LocalPlayer.CustomProperties["PlayerName"].ToString();
            else
            {
                string playerName = "Player " + PhotonNetwork.CurrentRoom.PlayerCount.ToString();
                playerNameText.text = playerName;
                PhotonNetwork.LocalPlayer.CustomProperties.Add("PlayerName", playerName);
            }
        }
    }

    void Update()
    {
 
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        base.OnPlayerEnteredRoom(newPlayer);

        string playerName = "Player " + PhotonNetwork.CurrentRoom.PlayerCount.ToString();
        newPlayer.CustomProperties.Add("PlayerName", playerName);
    }

    public override void OnPlayerPropertiesUpdate(Player targetPlayer, ExitGames.Client.Photon.Hashtable changedProps)
    {
       
    } 
}
