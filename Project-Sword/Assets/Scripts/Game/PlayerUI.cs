using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Photon.Pun;

public class PlayerUI : MonoBehaviour       //this is attached to player prefab -> canvas
{
    PlayerData playerData;

    [SerializeField] TextMeshProUGUI playerNameText;
    [SerializeField] Image playerHealthImage;


    void Start()
    {
        playerData = this.gameObject.transform.parent.transform.GetComponent<PlayerData>();

        playerNameText.text = playerData.getPlayerName();
        playerHealthImage.fillAmount = playerData.getPlayerHealth() / 100;
    }
}
