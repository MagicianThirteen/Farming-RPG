using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "so_GridProperties",menuName = "Scriptable Objects/Grid Properties")]
public class SO_GridProperties : ScriptableObject
{
    //属于哪个场景的，起始格子，格子大小，一堆List<GridProperty>
    public SceneName sceneName;
    public int gridWidth;
    public int gridHeight;
    public int originX;
    public int originY;
    [SerializeField]
    public List<GridProperty> gridProperties;
}
