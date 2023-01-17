using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using Photon.Pun;

public class MainMenuButtons : MonoBehaviourPunCallbacks
{
    [SerializeField] Button joinButton;

    private void Start()
    {
        joinButton.interactable = false;
    }


    public void onConnectButtonClick()
    {
        PhotonNetwork.ConnectUsingSettings();
    }

    public void onJoinButtonClick()
    {
        int numberOfRooms = PhotonNetwork.CountOfRooms;

        PhotonNetwork.JoinOrCreateRoom("Room 1", null, null, null);
    }

    public override void OnConnectedToMaster()
    {
       // Debug.Log("Connected to master");
        PhotonNetwork.JoinLobby();
    }

    public override void OnJoinedLobby()
    {
     //   Debug.Log("Joined lobby");
        joinButton.interactable = true;
    }

    public override void OnJoinedRoom()
    {
        PhotonNetwork.LocalPlayer.NickName = "Player " + PhotonNetwork.CurrentRoom.PlayerCount;
        PhotonNetwork.LoadLevel("Game");
    }
}
