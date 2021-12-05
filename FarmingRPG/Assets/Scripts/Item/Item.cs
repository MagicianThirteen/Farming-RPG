using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 用来定义物品的一些属性和行为，以及初始化
/// 
/// </summary>
public class Item : MonoBehaviour
{
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
        
    }
     
    
}
