
using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UIInventorySlot:MonoBehaviour,IBeginDragHandler,IDragHandler,IEndDragHandler,IPointerEnterHandler,IPointerExitHandler,IPointerClickHandler
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
    [SerializeField] private GameObject textBoxGo;
    private GameObject textBox;
    private Canvas parentCanvas;
    public bool isSelect=false;
    

    private void Start()
    {
        mainCamera=Camera.main;
        parentCanvas = GetComponentInParent<Canvas>();
        itemParentTransform = GameObject.FindWithTag(Tags.ItemParentTransform).transform;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        DestroyBox();
        if (itemDetails != null)
        {
            //玩家不准动
            Player.Instance.DisablePlayerInputAndResetMovement();
            inventoryBar.SetInventorySelect();
            dragItem = Instantiate(inventoryBar.dragItemGo, inventoryBar.transform);
            dragItem.GetComponentInChildren<Image>().sprite = itemDetails.itemSprite;
            SetSlotSelect();
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
                DestroyBox();
                ClearSlotSelect();
            }
            else
            {
                if (itemDetails != null&&itemDetails.canBeDropped==true)//这个条件要再放下之前判断
                {
                    DropItemInScene();
                }
                
            }
        }
    }

    private void DropItemInScene()
    {
        if (itemDetails != null && isSelect)
        {
            Vector3 currPos = mainCamera.ScreenToWorldPoint(new Vector3(Input.mousePosition.x,Input.mousePosition.y,-mainCamera.transform.position.z));
            GameObject item = Instantiate(inventoryBar.itemGo, itemParentTransform);
            Item _item=item.GetComponent<Item>();
            _item.ItemCode = itemDetails.itemCode;
            _item.GetComponentInChildren<SpriteRenderer>().sprite = itemDetails.itemSprite;
            item.transform.position = currPos;
            //清除库存的方法
            InventoryManager.Instance.MoveItem(InventoryLocation.player,itemDetails.itemCode);
            if (InventoryManager.Instance.FindItemInInventory(InventoryLocation.player, _item.ItemCode) == -1)
            {
                ClearSlotSelect();
            }
            Player.Instance.EnablePlayerInput();
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        UIInventorySlot slot = eventData.pointerEnter.gameObject.GetComponent<UIInventorySlot>();
        if (slot != null && slot.itemDetails != null)
        {
            ItemDetails itemDetails = slot.itemDetails;
            textBox = Instantiate(textBoxGo, parentCanvas.transform);
            textBox.transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z);
            UIInventoryTextBox box = textBox.GetComponent<UIInventoryTextBox>();
            string itemTypeDescription = InventoryManager.Instance.CovertItemTypeToString(itemDetails.itemType);
            box.SetTextBoxInfomation(itemDetails.itemDescription,itemTypeDescription,"",itemDetails.itemLongDescription,"","");
            RectTransform rect = textBox.GetComponent<RectTransform>();
            if (inventoryBar.IsInventoryBarPositionBottom)
            {
                rect.pivot = new Vector2(0.5f, 0);
                textBox.transform.position =
                    new Vector3(transform.position.x, transform.position.y + 20, transform.position.z);
            }
            else
            {
                rect.pivot = new Vector2(0.5f, 1);
                textBox.transform.position =
                    new Vector3(transform.position.x, transform.position.y-20,transform.position.z);
            }
            
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        DestroyBox();
    }

    private void DestroyBox()
    {
        if (textBox != null)
        {
            Destroy(textBox);
        }
        
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            if (isSelect)
            {
                ClearSlotSelect();
            }
            else
            {
                if (itemQuantity > 0)
                {
                    SetSlotSelect();
                }
            }
        }
    }

    private void ClearSlotSelect()
    {
        //清除所选择的，通过bar脚本
        isSelect = false;
        inventoryBar.ClearInventorySelect();
        //清除记录的数据，通过Inventory脚本
        InventoryManager.Instance.ClearSelectItem(InventoryLocation.player);
        //清除玩家头上所选道具
        Player.Instance.ClearCarriedItem();
        //播放替换动画
        List<CharacterAttribute> characterAttributes = new List<CharacterAttribute>();
        CharacterAttribute attribute = new CharacterAttribute();
        attribute.characterPart = CharacterPartAnimator.arms;
        attribute.partVariantColour = PartVariantColour.none;
        attribute.partVariantType = PartVariantType.none;
        characterAttributes.Add(attribute);
        Player.Instance._animationOverrides.ApplyCharacterCustomisationParameters(characterAttributes);
        
    }

    private void SetSlotSelect()
    {
        //先清一遍之前的，选中所选的，通过bar脚本
        inventoryBar.ClearInventorySelect();
        isSelect = true;
        inventoryBar.SetInventorySelect();
        //保存记录的数据，通过Inventory脚本
        InventoryManager.Instance.SaveSelectItem(InventoryLocation.player,itemDetails.itemCode);
        //玩家举着所选道具
        int itemcode = InventoryManager.Instance.selectItemInInventoryLists[(int) InventoryLocation.player];
        if (itemcode != -1)
        {
            Player.Instance.ShowCarriedItem(itemcode);
        }
        //播放替换动画
        List<CharacterAttribute> characterAttributes = new List<CharacterAttribute>();
        CharacterAttribute attribute = new CharacterAttribute();
        attribute.characterPart = CharacterPartAnimator.arms;
        attribute.partVariantColour = PartVariantColour.none;
        attribute.partVariantType = PartVariantType.carry;
        characterAttributes.Add(attribute);
        Player.Instance._animationOverrides.ApplyCharacterCustomisationParameters(characterAttributes);
    }
}
