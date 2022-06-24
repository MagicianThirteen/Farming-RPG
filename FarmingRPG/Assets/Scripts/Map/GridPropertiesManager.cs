using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(GenerateGUID))]
public class GridPropertiesManager : SingletonMonobehaviour<GridPropertiesManager>,Isaveable
{
    //功能：获取GUID，转换数据，放入GameObjectSave中存储，能保存和还原，制作一个获取当前格子的方法
    public SO_GridProperties[] soGridPropertiesArray;
    private Dictionary<string, GridPropertyDetails> gridPropertyDetailsDictionary ;
    private GameObjectSave gameObjectSave;
    public Grid grid;
    public string IsaveableUniqueID { get; set; }

    protected override void Awake()
    {
        base.Awake();
        IsaveableUniqueID = GetComponent<GenerateGUID>().GUID;
        gameObjectSave = new GameObjectSave();
    }

    void Start()
    {
        //一开始初始化好每个场景的格子数据
        InitialiseGridProperties();
    }

    public void AfterSceneLoad()
    {
        grid = FindObjectOfType<Grid>();
    }
    
    public void ISaveableRegister()
    {
        SaveLoadManager.Instance.iSaveableObjectList.Add(this);
    }

    public void ISaveableDeregister()
    {
        SaveLoadManager.Instance.iSaveableObjectList.Remove(this);
    }
    
    private void OnEnable()
    {
        ISaveableRegister();
        EventHandler.AfterSceneLoadEvent += AfterSceneLoad;
    }

    private void OnDisable()
    {
        ISaveableDeregister();
        EventHandler.AfterSceneLoadEvent -= AfterSceneLoad;
    }
    
    /// <summary>
    /// 转成Dictionary<string,GridPropertyDetails>
    /// </summary>
    private void InitialiseGridProperties()
    {
        //遍历so,获取里面的地图数据,转成里面需要Dictionary,添加到对应的SceneSave里
        if (soGridPropertiesArray.Length > 0)
        {
            foreach (var so in soGridPropertiesArray)
            {
                Dictionary<string, GridPropertyDetails> gridPropertyDetailsMap =
                    new Dictionary<string, GridPropertyDetails>();
                foreach (var gridProperty in so.gridProperties)
                {
                    int x = gridProperty.gridCoordinate.x;
                    int y = gridProperty.gridCoordinate.y;
                    GridPropertyDetails gridPropertyDetails = GetGridPropertyDetails(x, y, gridPropertyDetailsMap);
                    if (gridPropertyDetails == null)
                        gridPropertyDetails = new GridPropertyDetails();
                    switch (gridProperty.gridBoolProperty)
                    {
                        case GridBoolProperty.diggable:
                            gridPropertyDetails.isDiggable = gridProperty.gridBoolValue;
                            break;
                        case GridBoolProperty.isPath:
                            gridPropertyDetails.isPath = gridProperty.gridBoolValue;
                            break;
                        case GridBoolProperty.canDropItem:
                            gridPropertyDetails.canDropItem = gridProperty.gridBoolValue;
                            break;
                        case GridBoolProperty.canPlaceFurniture:
                            gridPropertyDetails.canPlaceFurniture = gridProperty.gridBoolValue;
                            break;
                        case GridBoolProperty.isNPCObstacle:
                            gridPropertyDetails.isNPCObstacle = gridProperty.gridBoolValue;
                            break;
                        default:
                            break;
                    }
                    SetGridPropertyDetails(x,y,gridPropertyDetails,gridPropertyDetailsMap);
                }
                //为什么这里要重新创造sceneSave
                SceneSave sceneSave = new SceneSave();
                sceneSave.gridPropertyDetailsDictionary = gridPropertyDetailsMap;
                //这里是第一次进游戏的时候，要保存的第一个场景的数据
                if (so.sceneName.ToString() == SceneControllerManager.Instance.startSceneName.ToString())
                {
                    this.gridPropertyDetailsDictionary = gridPropertyDetailsMap;
                }
                string sceneName = so.sceneName.ToString();
                gameObjectSave.sceneData.Add(sceneName,sceneSave);
            }
           
            
        }
        
        
    }

    public GridPropertyDetails GetGridPropertyDetails(int x, int y)
    {
        string key = "x" + x + "y" + y;
        GridPropertyDetails gridPropertyDetails;
        if (!this.gridPropertyDetailsDictionary.TryGetValue(key,out gridPropertyDetails))
        {
            return null;
        }
        else
        {
            return gridPropertyDetails;
        }
    }

    private GridPropertyDetails GetGridPropertyDetails(int x, int y, Dictionary<string, GridPropertyDetails>
        gridPropertyDetailsMap)
    {
        string key = "x" + x + "y" + y;
        GridPropertyDetails gridPropertyDetails;
        if (!gridPropertyDetailsMap.TryGetValue(key,out gridPropertyDetails))
        {
            return null;
        }
        else
        {
            return gridPropertyDetails;
        }
    }

    private void SetGridPropertyDetails(int x, int y, GridPropertyDetails gridPropertyDetails,
        Dictionary<string,GridPropertyDetails> propertyDetailsMap)
    {
        string key = "x" + x + "y" + y;
        propertyDetailsMap[key] = gridPropertyDetails;
    }

    public void  ISaveableStoreScene(string sceneName)
    {
        //移除之前的，添加进新的
        gameObjectSave.sceneData.Remove(sceneName);
        SceneSave sceneSave = new SceneSave();
        sceneSave.gridPropertyDetailsDictionary = gridPropertyDetailsDictionary;
        gameObjectSave.sceneData.Add(sceneName,sceneSave);
    }

    public void ISaveableRestoreScene(string sceneName)
    {
        if(gameObjectSave.sceneData.TryGetValue(sceneName,out SceneSave sceneSave))
        {
            if (sceneSave.gridPropertyDetailsDictionary != null)
            {
                gridPropertyDetailsDictionary = sceneSave.gridPropertyDetailsDictionary;
            }
        }
    }
}
