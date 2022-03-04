using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(BoxCollider2D))]
public class SceneTeleport : MonoBehaviour
{
    public SceneName sceneGoTo;
    [SerializeField]private Vector3 scenePostion;
    private void OnTriggerEnter2D(Collider2D other)
    {
        Player player = other.GetComponent<Player>();
        if (player != null)
        {
            float x = Mathf.Approximately(scenePostion.x, 0) ? player.transform.position.x : scenePostion.x;
            float y = Mathf.Approximately(scenePostion.y, 0) ? player.transform.position.y : scenePostion.y;
            float z = 0;
            scenePostion = new Vector3(x, y, z);
            SceneControllerManager.Instance.FadeAndLoadScene(sceneGoTo.ToString(),scenePostion);

        }
        
    }
}
