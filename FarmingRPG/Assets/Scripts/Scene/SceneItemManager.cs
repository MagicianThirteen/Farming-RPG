using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(GenerateGUID))]
public class SceneItemManager : SingletonMonobehaviour<SceneItemManager>,Isaveable
{
    //guid,GameObjectSave,注册，不注册，保存，还原
    [SerializeField]
    private string _generateGUID;
    public GameObjectSave gameObjectSave;
    private GameObject itemParentGo;
    public GameObject itemPrefab;
    
    protected override void Awake()
    {
        base.Awake();
        _generateGUID = GetComponent<GenerateGUID>().GUID;
        gameObjectSave = new GameObjectSave();//这里是各自的gameObjectSave
    }

    private void OnEnable()
    {
        ISaveableRegister();
        EventHandler.AfterSceneLoadEvent += AfterSceneLoad;
    }

    public void AfterSceneLoad()
    {
        itemParentGo=GameObject.FindWithTag(Tags.ItemParentTransform);
    }

    private void OnDisable()
    {
        ISaveableDeregister();
        EventHandler.AfterSceneLoadEvent -= AfterSceneLoad;
    }

    public string IsaveableUniqueID { get=>_generateGUID; set=>_generateGUID=value; }
    public void ISaveableRegister()
    {
        SaveLoadManager.Instance.iSaveableObjectList.Add(this);
    }

    public void ISaveableDeregister()
    {
        SaveLoadManager.Instance.iSaveableObjectList.Remove(this);
    }

    public void ISaveableStoreScene(string sceneName)
    {
        //移掉之前的，找到现在的，组装成List<sceneItem>，组合进gameObjectSave
        gameObjectSave.sceneData.Remove(sceneName);
        List<SceneItem> sceneItems = new List<SceneItem>();
        Item[] allItems = FindObjectsOfType<Item>();
        foreach (var items in allItems)
        {
            SceneItem sceneItem = new SceneItem();
            sceneItem.itemCode = items.ItemCode;
            sceneItem.itemDetail = items.name;
            sceneItem.position = new Vector3Serializable(items.transform.position.x,
                items.transform.position.y, items.transform.position.z);
            sceneItems.Add(sceneItem);
        }

        SceneSave sceneSave = new SceneSave();
        sceneSave.sceneItemList = sceneItems;
        gameObjectSave.sceneData.Add(sceneName,sceneSave);
    }

    private void DestoryAllItems()
    {
        Item[] allItems = FindObjectsOfType<Item>();
        foreach (var item in allItems)
        {
            Destroy(item.gameObject);
        }
    }

    private void InstantiateItems(List<SceneItem> items)
    {
        foreach (var item in items)
        {
            Vector3 position = new Vector3(item.position.x, item.position.y, item.position.z);
            GameObject itemgo=Instantiate(itemPrefab,position, Quaternion.identity,itemParentGo.transform);
            Item sceneItem = itemgo.GetComponent<Item>();
            sceneItem.ItemCode = item.itemCode;
            sceneItem.Init(item.itemCode);
        }
    }

    public void ISaveableRestoreScene(string sceneName)
    {
        //从gameSaveObject里取出，在把场景上有的销毁，再根据取出的生成
        /*gameObjectSave.sceneData.TryGetValue(sceneName, out SceneSave sceneSave);
        List<SceneItem> sceneItems = sceneSave.sceneItemList;
        DestoryAllItems();
        InstantiateItems(sceneItems);*/
        if (gameObjectSave.sceneData.TryGetValue(sceneName, out SceneSave sceneSave))
        {
            if (sceneSave.sceneItemList != null)
            {
                // scene list items found - destroy existing items in scene
                DestoryAllItems();

                // now instantiate the list of scene items
                InstantiateItems(sceneSave.sceneItemList);
            }
        }
    }
}
