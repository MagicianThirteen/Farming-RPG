using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface Isaveable 
{
    public string IsaveableUniqueID { get; set; }

    void ISaveableRegister();

    void ISaveableDeregister();

    void ISaveableStoreScene(string sceneName);

    void ISaveableRestoreScene(string sceneName);

}
