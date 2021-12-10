using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq.Expressions;
using UnityEngine;
using Debug = UnityEngine.Debug;

public class InventoryManager : SingletonMonobehaviour<InventoryManager>
{
    private Dictionary<int, ItemDetails> ItemDetailsDictionary;
    [SerializeField]
    private SO_ItemList itemList;

    public List<InventoryItem>[] inventoryLists;
    private int[] inventoryListCapacityIntArray;
    public int[] selectItemInInventoryLists;

    protected override void Awake()
    {
        base.Awake();
        ItemDetailsDictionary = new Dictionary<int, ItemDetails>();
        CreateItemDetailsDictionary();
        CreateInventoryLists();
        InitSelectItems();
    }

    private void InitSelectItems()
    {
        selectItemInInventoryLists = new int[(int) InventoryLocation.count];
        for (int i = 0; i < selectItemInInventoryLists.Length; i++)
        {
            selectItemInInventoryLists[i] = -1;
        }
    }
    
    //保存在当前容器中选中的物品
    public void SaveSelectItem(InventoryLocation location, int itemCode)
    {
        selectItemInInventoryLists[(int) location] = itemCode;
        //Debug.Log($"当前选中的是{GetItemDetails(itemCode).itemDescription}");
    }
    
    //清除在当前容器选中的物品
    public void ClearSelectItem(InventoryLocation location)
    {
        selectItemInInventoryLists[(int) location] = -1;
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

    public void SwapInventorySlot(InventoryLocation location, int from, int to)
    {
        List<InventoryItem> items = inventoryLists[(int) location];
        //to和from都不能超过背包里有的数据的长度，to>0
        if (items.Count > 0 && to < items.Count && to >= 0&&from!=to)
        {
            InventoryItem item = items[from];
            items[from] = items[to];
            items[to] = item;
        }
        EventHandler.InventoryUpdatedEvent(InventoryLocation.player, items);
    }

    /// <summary>
    /// 从背包里移除物品
    /// </summary>
    /// <param name="location"></param>
    /// <param name="itemCode"></param>
    public void MoveItem(InventoryLocation location, int itemCode)
    {
        List<InventoryItem> items = inventoryLists[(int) location];
        int position = FindItemInInventory(location, itemCode);
        if (position != -1)
        {
            if (items[position].itemQuantity > 1)
            {
                InventoryItem tmp = new InventoryItem();
                tmp.itemCode = itemCode;
                tmp.itemQuantity = items[position].itemQuantity - 1;
                items[position] = tmp;
            }
            else
            {
                items.RemoveAt(position);
            }
        }
        EventHandler.InventoryUpdatedEvent(InventoryLocation.player, items);
    }

    /// <summary>
    /// 在对应的列表里找加进来的物体，没有的返回-1，有的返回位置
    /// </summary>
    /// <param name="location"></param>
    /// <param name="itemCode"></param>
    /// <returns></returns>
    public int FindItemInInventory(InventoryLocation location, int itemCode)
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
        //DebugPrintInventoryList(items);
        //触发事件
        EventHandler.InventoryUpdatedEvent(location, items);
    }
    private void AddItemPosition(InventoryLocation location, int itemCode,int position)
    {
        List<InventoryItem> items = inventoryLists[(int) location];
        InventoryItem inventoryItem = new InventoryItem();
        inventoryItem.itemCode = itemCode;
        inventoryItem.itemQuantity = items[position].itemQuantity + 1;
        items[position] = inventoryItem;//重新赋值
        //打印
        //DebugPrintInventoryList(items);
        //触发事件:比如ui修改
        EventHandler.InventoryUpdatedEvent(location, items);
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
    
    //把对应的工具转成对应的字符串
    public string CovertItemTypeToString(ItemType type)
    {
        string itemDescription;
        switch (type)
        {
            case ItemType.Breaking_tool:
                itemDescription = Settings.BreakingTool;
                break;
            case ItemType.Chopping_tool:
                itemDescription = Settings.ChoppingTool;
                break;
            case ItemType.Collecting_tool:
                itemDescription = Settings.CollectingTool;
                break;
            case ItemType.Hoeing_tool:
                itemDescription = Settings.HoeingTool;
                break;
            case ItemType.Reaping_tool:
                itemDescription = Settings.ReapingTool;
                break;
            case ItemType.Watering_tool:
                itemDescription = Settings.WateringTool;
                break;
            default:
                itemDescription = type.ToString();
                break;
        }
        return itemDescription;
    }
}
