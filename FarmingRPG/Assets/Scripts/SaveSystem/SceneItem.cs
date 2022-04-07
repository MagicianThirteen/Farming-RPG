using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class SceneItem
{
     public int itemCode;
     public Vector3Serializable position;
     public string itemDetail;

     public SceneItem()
     {
          position = new Vector3Serializable();
     }
}
