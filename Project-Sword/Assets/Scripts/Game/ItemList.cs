using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemList : MonoBehaviour
{
    [SerializeField] string[] itemNamesHelper;
    static string[] itemNames;

    void Start()
    {
        itemNames = itemNamesHelper;
    }

    static string getItemName(int itemCode)
    {
        return itemNames[itemCode];
    }
}
