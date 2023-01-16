using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Photon.Pun;

public class UI : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI serverNameText;
    [SerializeField] TextMeshProUGUI playersNumber;

    void Update()
    {
        serverNameText.text = PhotonNetwork.CurrentRoom.Name.ToString();
        playersNumber.text = "Players: " + PhotonNetwork.CurrentRoom.PlayerCount.ToString();
    }

    public void onQuitButtonClick()
    {
        Application.Quit();
    }
}
