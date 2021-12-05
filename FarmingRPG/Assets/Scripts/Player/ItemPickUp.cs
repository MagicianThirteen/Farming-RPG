using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemPickUp : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        Item item = other.gameObject.GetComponent<Item>();
        if (item != null)
        {
            ItemDetails itemDetails = InventoryManager.Instance.GetItemDetails(item.ItemCode);
            Debug.Log("当前碰得到的物体是："+itemDetails.itemDescription);
        }
    }
}
