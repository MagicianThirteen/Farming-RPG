using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIInventoryBar : MonoBehaviour
{
    private bool _isInventoryBarPositionBottom=false;

    public bool IsInventoryBarPositionBottom
    {
        get => _isInventoryBarPositionBottom;
        set => _isInventoryBarPositionBottom = value;
    }

    public Sprite blank16x16sprite;
    public UIInventorySlot[] inventorySlots;
    public GameObject itemGo;
    public GameObject dragItemGo;

    private RectTransform _rect;
    // Update is called once per frame
    private void Start()
    {
        _rect = GetComponent<RectTransform>();
    }

    private void OnEnable()
    {
        EventHandler.InventoryUpdatedEvent += InventoryUpdated;
    }

    private void OnDisable()
    {
        EventHandler.InventoryUpdatedEvent -= InventoryUpdated;
    }

    void Update()
    {
        SwitchInventoryBarPosition();
    }

    private void InventoryUpdated(InventoryLocation location, List<InventoryItem> items)
    {
        if (location == InventoryLocation.player)
        {
            if (items.Count >= 0&&inventorySlots.Length>0)
            {
                //清空之前的列表
                ClearInventoryBar();
                for (int i = 0; i < inventorySlots.Length; i++)
                {
                    if (i < items.Count)
                    {
                        ItemDetails itemDetails = InventoryManager.Instance.GetItemDetails(items[i].itemCode);
                        SetSlotInformation(inventorySlots[i],itemDetails.itemSprite, items[i].itemQuantity.ToString(), itemDetails, items[i].itemQuantity);
                    }
                    else
                    {
                        break;
                    }
                }
            }
        }
        
    }

    public void ClearInventorySelect()
    {
        if (inventorySlots.Length > 0)
        {
            for (int i = 0; i < inventorySlots.Length; i++)
            {
                inventorySlots[i].isSelect = false;
                inventorySlots[i].inventorySlotHighlight.color = new Color(0, 0, 0, 0);
            }
        }
    }

    public void SetInventorySelect()
    {
        if (inventorySlots.Length > 0)
        {
            for (int i = 0; i < inventorySlots.Length; i++)
            {
                if (inventorySlots[i].isSelect)
                {
                    inventorySlots[i].inventorySlotHighlight.color = new Color(1, 1, 1, 1);
                }
            }
        }
    }

    private void ClearInventoryBar()
    {
        if (inventorySlots.Length > 0)
        {
            foreach (var slot in inventorySlots)
            {
                SetSlotInformation(slot,blank16x16sprite, " ", null, 0);
            }
        }
    }

    private void SetSlotInformation(UIInventorySlot slot,Sprite slotSprite, string text, ItemDetails itemDetails, int quantity)
    {
        slot.inventorySlotImage.sprite = slotSprite;
        //slot.inventorySlotHighlight.sprite = hightLight;
        slot.textMeshProUGUI.text = text;
        slot.itemDetails = itemDetails;
        slot.itemQuantity = quantity;
    }

    private void SwitchInventoryBarPosition()
    {
        Vector3 position = Player.Instance.GetPlayerViewportPosition();
        if (position.y > 0.3 && _isInventoryBarPositionBottom == false)
        {
            _rect.pivot = new Vector2(0.5f, 0);
            _rect.anchorMin = new Vector2(0.5f, 0);
            _rect.anchorMax = new Vector2(0.5f, 0);
            _isInventoryBarPositionBottom = true;
        }

        if (position.y < -0.3 && _isInventoryBarPositionBottom == true)
        {
            _rect.pivot = new Vector2(0.5f, 1);
            _rect.anchorMin = new Vector2(0.5f, 1);
            _rect.anchorMax = new Vector2(0.5f, 1);
            _isInventoryBarPositionBottom = false;
        }
    }
}
