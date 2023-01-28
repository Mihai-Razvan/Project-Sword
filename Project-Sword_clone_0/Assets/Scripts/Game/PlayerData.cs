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
        dataHashTable["PlayerState"] = "IDLE";

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
        dataHashTable["PlayerHealth"] = (int)dataHashTable["PlayerHealth"] + val;
        this.gameObject.transform.Find("Canvas").gameObject.GetComponent<PlayerUI>().updateUI();
    }

    public string GetData(string key)
    {
        if (dataHashTable.ContainsKey(key))
            return dataHashTable[key].ToString();
        else
            return null;
    }

    public void UpdateData(string key, string value)
    {
        dataHashTable[key] = value;
        PhotonNetwork.LocalPlayer.SetCustomProperties(dataHashTable);
    }
}
