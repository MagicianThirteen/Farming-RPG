
using UnityEditor;
using UnityEngine;
using UnityEngine.Tilemaps;

[ExecuteAlways]
public class TilemapGridProperties : MonoBehaviour
{
#if UNITY_EDITOR
    [SerializeField] private SO_GridProperties soGridProperties;
    public GridBoolProperty gridProperty;
    private Tilemap tilemap;
    private void OnEnable()
    {
        tilemap = GetComponent<Tilemap>();
        if (!Application.IsPlaying(gameObject))
        {
            if (soGridProperties != null)
            {
                soGridProperties.gridProperties.Clear();
            }
        }
    }

    //1.编辑保存的格子，写入脏数据
    private void OnDisable()
    {
        if (!Application.IsPlaying(gameObject))
        {
            UpdateGridProperties();
            if (soGridProperties != null)
            {
                EditorUtility.SetDirty(soGridProperties);
            }
        }
    }

    private void UpdateGridProperties()
    {
        //界定真实边界
        tilemap.CompressBounds();
        //根据边界获取对应瓦片地图格子，写入SO的List里
        BoundsInt cellBound = tilemap.cellBounds;
        Vector3Int start = cellBound.min;
        Vector3Int end = cellBound.max;
        for (int i = start.x; i < end.x; i++)
        {
            for (int j = start.y; j < end.y; j++)
            {
                TileBase tile = tilemap.GetTile (new Vector3Int(i,j,0));
                if (tile != null&&soGridProperties!=null)
                {
                    soGridProperties.gridProperties.Add(new GridProperty(
                        new GridCoordinate(i,j),gridProperty,true));
                }
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (!Application.IsPlaying(gameObject))
        {
            Debug.Log("设置好了，请设置Disable以保存");
        }
    }
#endif
}
