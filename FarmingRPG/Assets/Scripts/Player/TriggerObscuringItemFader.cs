using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerObscuringItemFader : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        ObscuringItemFader[] _faders =
            other.gameObject.GetComponentsInChildren<ObscuringItemFader>();
        if (_faders.Length > 0)
        {
            for (int i = 0; i < _faders.Length; i++)
            {
                _faders[i].FadeOut();
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        ObscuringItemFader[] _faders =
            other.gameObject.GetComponentsInChildren<ObscuringItemFader>();
        if (_faders.Length > 0)
        {
            for (int i = 0; i < _faders.Length; i++)
            {
                _faders[i].FadeIn();
            }
        }
    }
}
