
using System;
using UnityEngine;
using Cinemachine;

public class SwitchConfineBoundingShape : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        SwitchBoundingShape();
    }

    private void SwitchBoundingShape()
    {
        //找到边界
        PolygonCollider2D polygonCollider2D =
            GameObject.FindGameObjectWithTag(Tags.BoundsConfiner).GetComponent<PolygonCollider2D>();
        //告诉相机边界在哪
        CinemachineConfiner cinemachineConfiner = GetComponent<CinemachineConfiner>();
        cinemachineConfiner.m_BoundingShape2D = polygonCollider2D;
        cinemachineConfiner.m_ConfineScreenEdges = true;
        //当边界改变时清楚之前的缓存路径
        cinemachineConfiner.InvalidatePathCache();
    }
    
}
