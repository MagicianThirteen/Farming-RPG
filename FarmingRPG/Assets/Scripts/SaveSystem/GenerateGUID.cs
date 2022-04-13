using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteAlways]
public class GenerateGUID : MonoBehaviour
{
    [SerializeField]
    private string _guid="";
    
    public string GUID
    {
        get => _guid;
        set => _guid = value;
    }
    private void Awake()
    {
        if (!Application.IsPlaying(gameObject))
        {
            if (_guid == null)
            {
                _guid = System.Guid.NewGuid().ToString();
            }
            
        }
    }
}
