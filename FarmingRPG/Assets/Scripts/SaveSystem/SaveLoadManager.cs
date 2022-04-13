using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SaveLoadManager : SingletonMonobehaviour<SaveLoadManager>
{
   public List<Isaveable> iSaveableObjectList;

   protected override void Awake()
   {
      base.Awake();
      iSaveableObjectList = new List<Isaveable>();
      StoreCurrentSceneData();
   }

   public void StoreCurrentSceneData()
   {
      foreach (var iSaveable in iSaveableObjectList)
      {
         iSaveable.ISaveableStoreScene(SceneManager.GetActiveScene().name);
      }
   }

   public void RestoreCurrentSceneData()
   {
      foreach (var iSaveable in iSaveableObjectList)
      {
         iSaveable.ISaveableRestoreScene(SceneManager.GetActiveScene().name);
      }
   }
}
