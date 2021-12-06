using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 用来定义物品的一些属性和行为，以及初始化
/// 
/// </summary>
public class Item : MonoBehaviour
{
    [ItemCodeDescription]
    [SerializeField]
    private int _itemCode;
    public int ItemCode
    {
        get => _itemCode;
        set => _itemCode = value;
    }
    private SpriteRenderer _spriteRenderer;
    
    // Start is called before the first frame update
    void Start()
    {
        //物品初始化
        if (_itemCode != 0)
        {
            Init(_itemCode);
        }
    }

    public void Init(int itemCode)
    {
        //根据不同的item类型，添加不同的组件
        ItemDetails itemDetails=InventoryManager.Instance.GetItemDetails(itemCode);
        if (itemDetails != null)
        {
            if (itemDetails.itemType == ItemType.Reapable_scenary)
            {
                gameObject.AddComponent<ItemNudge>();
            }
        }
    }
     
    
}
