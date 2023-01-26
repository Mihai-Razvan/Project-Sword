using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class PlayerInventory : MonoBehaviour
{
    [SerializeField] PhotonView view;
    int[] itemCodes;
    int selectedSlot;
    GameObject usedItem;
    public delegate void OnItemSelected();       //when a new inventory slot from the inventory bar is selected
    public OnItemSelected onItemSelected;

    void Start()
    {
        itemCodes = new int[2] { 1, 0 };
        selectedSlot = 0;
    }


    void Update()
    {
        if (!view.IsMine)
            return;

        changeItem();
    }

    void changeItem()
    {
        if (Input.GetKeyDown(KeyCode.Alpha2))
            selectedSlot = 1;
        else if (Input.GetKeyDown(KeyCode.Alpha1))
            selectedSlot = 0;
        else
            return;

        destroyUsedItem();
        onItemSelected();
    }

    void destroyUsedItem()
    {
        if (usedItem != null)
            Destroy(usedItem);
    }

    public int getUsedItemCode()
    {
        return itemCodes[selectedSlot];
    }

    public void setUsedItem(GameObject item)
    {
        usedItem = item;
    }
}
