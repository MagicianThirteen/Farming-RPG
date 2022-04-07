using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 序列化场景物品的位置
/// </summary>
[System.Serializable]
public class Vector3Serializable
{
    public float x, y, z;

    public Vector3Serializable(float x, float y, float z)
    {
        this.x = x;
        this.y = y;
        this.z = z;
    }

    public Vector3Serializable()
    {
        
    }
}
