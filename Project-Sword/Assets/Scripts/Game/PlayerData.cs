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

    public void changePlayerHealth(int val)
    {
        Debug.Log("change health");
        dataHashTable["PlayerHealth"] = (int) dataHashTable["PlayerHealth"] + val;
        this.gameObject.transform.Find("Canvas").gameObject.GetComponent<PlayerUI>().updateUI();
    }
}
