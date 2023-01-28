using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class PlayerInventory : MonoBehaviour
{
    [SerializeField] PhotonView view;
    [SerializeField] PlayerStateManager player;
    int[] itemCodes;
    int selectedSlot;
    public delegate void OnItemSelected(PlayerStateManager v);       //when a new inventory slot from the inventory bar is selected
    public OnItemSelected onItemSelected;

    public enum Items
    {
        None,
        Sword
    }
    public Items usedItem;


    void Start()
    {
        itemCodes = new int[2] { 1, 0 };
        selectedSlot = 0;
        usedItem = Items.None;
    }


    void Update()
    {
        if (!view.IsMine)
            return;

        selectItem();
    }

    void selectItem()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
            selectedSlot = 0;
        else if (Input.GetKeyDown(KeyCode.Alpha2))
            selectedSlot = 1;
        else
            return;

        switch(itemCodes[selectedSlot])
        {
            case 0:
                usedItem = Items.None;
                break;
            case 1:
                usedItem = Items.Sword;
                break;
        }

        onItemSelected(player);
    }
}
