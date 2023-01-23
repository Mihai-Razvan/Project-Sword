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

    void Awake()
    {
        view = this.gameObject.GetComponent<PhotonView>();
        dataHashTable = new ExitGames.Client.Photon.Hashtable();

        initValues();
    }

    void initValues()
    {
        dataHashTable["PlayerName"] = view.Owner.NickName;
        dataHashTable["PlayerHealth"] = 100;
        dataHashTable["PlayerMovementState"] = "IDLE";

        PhotonNetwork.LocalPlayer.SetCustomProperties(dataHashTable);
    }

    public string getPlayerName()
    {
        return dataHashTable["PlayerName"].ToString();
    }

    public int getPlayerHealth()
    {
        return int.Parse(dataHashTable["PlayerHealth"].ToString());
    }

    public string getPlayerMovementState()
    {
        if (view.Owner.CustomProperties.ContainsKey("PlayerMovementState"))
            return view.Owner.CustomProperties["PlayerMovementState"].ToString();
        else
            return null;
    }

    public void changePlayerHealth(int val)
    {
        dataHashTable["PlayerHealth"] = (int)dataHashTable["PlayerHealth"] + val;
        this.gameObject.transform.Find("Canvas").gameObject.GetComponent<PlayerUI>().updateUI();
    }


    public void setPlayerMovementState(string val)
    {
        dataHashTable["PlayerMovementState"] = val;
        PhotonNetwork.LocalPlayer.SetCustomProperties(dataHashTable);
    }
}
