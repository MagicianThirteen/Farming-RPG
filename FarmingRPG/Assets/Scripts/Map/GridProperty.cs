using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable ]
public class GridProperty
{
   //坐标，属性，有没有
   public GridCoordinate gridCoordinate;
   public GridBoolProperty gridBoolProperty;
   public bool gridBoolValue = false;

   public GridProperty(GridCoordinate gridCoordinate,
       GridBoolProperty gridBoolProperty, bool gridBoolValue)
   {
       this.gridCoordinate = gridCoordinate;
       this.gridBoolProperty = gridBoolProperty;
       this.gridBoolValue = gridBoolValue;
   }
}
