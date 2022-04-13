
using UnityEngine;
using System.Collections.Generic;
using UnityEditor;

[CustomPropertyDrawer(typeof(ItemCodeDescriptionAttribute))]//这里是为了告诉这个画画的它要为哪个属性画画
public class ItemCodeDescriptionDrawer : PropertyDrawer
{
   public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
   {
      return EditorGUI.GetPropertyHeight(property) * 2;
   }

   public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
   {
      EditorGUI.BeginProperty(position,label,property);
      //检查属性类型是不是整数，因为是Itemcode
      if (property.propertyType == SerializedPropertyType.Integer)
      {
         //当itemcode变化时也要反应
         EditorGUI.BeginChangeCheck();
         //这里才是真正画画的地方
         //Draw Itemcode
         var newValue = EditorGUI.IntField(new Rect(position.x, position.y, position.width, position.height / 2),
            label, property.intValue);
         //Draw ItemDescription
         EditorGUI.LabelField(new Rect(position.x,position.y+position.height/2,position.width,position.height/2),
            "Item Description",GetItemDescription(property.intValue));
         if (EditorGUI.EndChangeCheck())
         {
            property.intValue = newValue;
         }
      }
      EditorGUI.EndProperty();
   }

   private string GetItemDescription(int itemCode)
   {
      SO_ItemList soItemList;
      soItemList=AssetDatabase.LoadAssetAtPath("Assets/Scriptable Object Assets/Item/so_ItemList.asset",typeof(SO_ItemList)) as SO_ItemList;
      ItemDetails itemDetails = soItemList.itemDetails.Find(x => x.itemCode == itemCode);
      if (itemDetails != null)
      {
         return itemDetails.itemDescription;
      }
      else
      {
         return "没有这个物品哦";
      }
   }
}
