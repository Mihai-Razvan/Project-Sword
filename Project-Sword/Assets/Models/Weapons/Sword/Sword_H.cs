using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sword_H : MonoBehaviour
{
    [SerializeField] int itemCode;
    [SerializeField] GameObject swordPrefab;
    [SerializeField] Transform hand;
    [SerializeField] PlayerInventory playerInventory;

    void Start()
    {
        playerInventory.onItemSelected += setPrefab;
    }


    void setPrefab()
    {
        if (playerInventory.getUsedItemCode() == itemCode)
        {
            GameObject sword = Instantiate(swordPrefab, hand.position, Quaternion.identity);
            sword.transform.SetParent(hand);
            playerInventory.setUsedItem(sword);
        }
    }
}
