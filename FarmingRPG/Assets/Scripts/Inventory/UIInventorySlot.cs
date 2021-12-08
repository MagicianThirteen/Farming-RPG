
using System;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UIInventorySlot:MonoBehaviour,IBeginDragHandler,IDragHandler,IEndDragHandler
{
    [SerializeField] private GameObject dragItem;
    public Image inventorySlotHighlight;
    public Image inventorySlotImage;
    public TextMeshProUGUI textMeshProUGUI;
    [HideInInspector]public ItemDetails itemDetails;
    [HideInInspector]public int itemQuantity;
    public UIInventoryBar inventoryBar;
    private Transform itemParentTransform;
    private Camera mainCamera;
    public int slotNum;

    private void Start()
    {
        mainCamera=Camera.main;
        itemParentTransform = GameObject.FindWithTag(Tags.ItemParentTransform).transform;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (itemDetails != null)
        {
            //玩家不准动
            Player.Instance.DisablePlayerInputAndResetMovement();
            dragItem = Instantiate(inventoryBar.dragItemGo, inventoryBar.transform);
            dragItem.GetComponentInChildren<Image>().sprite = itemDetails.itemSprite;
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (dragItem != null)
        {
            Player.Instance.DisablePlayerInputAndResetMovement();
            dragItem.transform.position = Input.mousePosition;
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (dragItem != null)
        {
            Destroy(dragItem);
            //判断当前是在栏内还是栏外
            if (eventData.pointerCurrentRaycast.gameObject != null &&
                eventData.pointerCurrentRaycast.gameObject.GetComponent<UIInventorySlot>() != null)
            {
                UIInventorySlot toSlot = eventData.pointerCurrentRaycast.gameObject.GetComponent<UIInventorySlot>();
                int toSlotNum = toSlot.slotNum;
                InventoryManager.Instance.SwapInventorySlot(InventoryLocation.player,slotNum,toSlotNum);
            }
            else
            {
                if (itemDetails != null)
                {
                    DropItemInScene();
                }
                
            }
        }
    }

    private void DropItemInScene()
    {
        Vector3 currPos = mainCamera.ScreenToWorldPoint(new Vector3(Input.mousePosition.x,Input.mousePosition.y,-mainCamera.transform.position.z));
        GameObject item = Instantiate(inventoryBar.itemGo, itemParentTransform);
        Item _item=item.GetComponent<Item>();
        _item.ItemCode = itemDetails.itemCode;
        _item.GetComponentInChildren<SpriteRenderer>().sprite = itemDetails.itemSprite;
        item.transform.position = currPos;
        //清除库存的方法
        InventoryManager.Instance.MoveItem(InventoryLocation.player,itemDetails.itemCode);
        Player.Instance.EnablePlayerInput();
    }
}
