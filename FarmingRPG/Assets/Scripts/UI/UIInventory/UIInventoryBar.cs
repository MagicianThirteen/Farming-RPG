using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIInventoryBar : MonoBehaviour
{
    private bool _isInventoryBarPositionBottom=false;

    private RectTransform _rect;
    // Update is called once per frame
    private void Start()
    {
        _rect = GetComponent<RectTransform>();
    }

    void Update()
    {
        SwitchInventoryBarPosition();
    }

    private void SwitchInventoryBarPosition()
    {
        Vector3 position = Player.Instance.GetPlayerViewportPosition();
        if (position.y > 0.3 && _isInventoryBarPositionBottom == false)
        {
            _rect.pivot = new Vector2(0.5f, 0);
            _rect.anchorMin = new Vector2(0.5f, 0);
            _rect.anchorMax = new Vector2(0.5f, 0);
            _isInventoryBarPositionBottom = true;
        }

        if (position.y < -0.3 && _isInventoryBarPositionBottom == true)
        {
            _rect.pivot = new Vector2(0.5f, 1);
            _rect.anchorMin = new Vector2(0.5f, 1);
            _rect.anchorMax = new Vector2(0.5f, 1);
            _isInventoryBarPositionBottom = false;
        }
    }
}
