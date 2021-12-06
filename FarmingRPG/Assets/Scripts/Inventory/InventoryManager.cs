using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : SingletonMonobehaviour<InventoryManager>
{
    private Dictionary<int, ItemDetails> ItemDetailsDictionary;
    [SerializeField]
    private SO_ItemList itemList;

    protected override void Awake()
    {
        base.Awake();
        ItemDetailsDictionary = new Dictionary<int, ItemDetails>();
        CreateItemDetailsDictionary();
    }

    private void CreateItemDetailsDictionary()
    {
        foreach (var itemDetail in itemList.itemDetails)
        {
            ItemDetailsDictionary.Add(itemDetail.itemCode,itemDetail);
        }
    }

    public ItemDetails GetItemDetails(int itemCode)
    {
        ItemDetails details;
        if (ItemDetailsDictionary.TryGetValue(itemCode, out details))
        {
            return details;
        }
        else
        {
            return null;
        }
    }
}
