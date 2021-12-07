using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : SingletonMonobehaviour<InventoryManager>
{
    private Dictionary<int, ItemDetails> ItemDetailsDictionary;
    [SerializeField]
    private SO_ItemList itemList;

    public List<InventoryItem>[] inventoryLists;
    private int[] inventoryListCapacityIntArray;

    protected override void Awake()
    {
        base.Awake();
        ItemDetailsDictionary = new Dictionary<int, ItemDetails>();
        CreateItemDetailsDictionary();
        CreateInventoryLists();
    }
    
    //给各个容器和容量初始化
    private void CreateInventoryLists()
    {
        inventoryLists = new List<InventoryItem>[(int) InventoryLocation.count];
        inventoryListCapacityIntArray = new int[(int) InventoryLocation.count];
        for (int i = 0; i < (int)InventoryLocation.count; i++)
        {
            inventoryLists[i] = new List<InventoryItem>();
        }
        inventoryListCapacityIntArray[(int)InventoryLocation.player] = Settings.playerInitialInventoryCapacity;
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

    public void AddItem(InventoryLocation location, int itemCode, GameObject go)
    {
        AddItem(location, itemCode);
        Destroy(go);
    }

    private void AddItem(InventoryLocation location, int itemCode)
    {
        int position=FindItemInInventory(location,itemCode);
        if (position == -1)
        {
            AddItemPosition(location,itemCode);
        }
        else
        {
            AddItemPosition(location,itemCode,position);
        }
    }

    /// <summary>
    /// 在对应的列表里找加进来的物体，没有的返回-1，有的返回位置
    /// </summary>
    /// <param name="location"></param>
    /// <param name="itemCode"></param>
    /// <returns></returns>
    private int FindItemInInventory(InventoryLocation location, int itemCode)
    {
        List<InventoryItem> items = inventoryLists[(int) location];
        for (int i = 0; i < items.Count; i++)
        {
            if (items[i].itemCode == itemCode)
            {
                return i;
            }
        }
        return -1;
    }

    private void AddItemPosition(InventoryLocation location, int itemCode)
    {
        List<InventoryItem> items = inventoryLists[(int) location];
        InventoryItem inventoryItem = new InventoryItem();
        inventoryItem.itemCode = itemCode;
        inventoryItem.itemQuantity = 1;
        items.Add(inventoryItem);
        //打印
        DebugPrintInventoryList(items);
        //触发事件
        //EventHandler.InventoryUpdatedEvent(location, items);
    }
    private void AddItemPosition(InventoryLocation location, int itemCode,int position)
    {
        List<InventoryItem> items = inventoryLists[(int) location];
        InventoryItem inventoryItem = new InventoryItem();
        inventoryItem.itemCode = itemCode;
        inventoryItem.itemQuantity = items[position].itemQuantity + 1;
        items[position] = inventoryItem;//重新赋值
        //打印
        DebugPrintInventoryList(items);
        //触发事件:比如ui修改
        //EventHandler.InventoryUpdatedEvent(location, items);
    }

    private void DebugPrintInventoryList(List<InventoryItem> items)
    {
        foreach (var item in items)
        {
            Debug.Log("物品描述："+InventoryManager.Instance.GetItemDetails(item.itemCode).itemDescription+
                      "总共有："+item.itemQuantity+"个");
        }
        Debug.Log("********************************************************");
    }
}
