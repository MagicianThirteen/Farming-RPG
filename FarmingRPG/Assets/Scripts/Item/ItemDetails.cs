
using UnityEngine;

[System.Serializable]
public class ItemDetails
{
    public int itemCode;
    public ItemType itemType;
    public string itemDescription;
    public Sprite itemSprite;
    public string itemLongDescription;
    public short itemUseGridRadius;//它被玩家扔出去的时候占几个格子
    public float itemUseRadius;
    public bool isStartingItem;
    public bool canBePickedUp;
    public bool canBeDropped;
    public bool canBeEaten;
    public bool canBeCarried;

    
}
