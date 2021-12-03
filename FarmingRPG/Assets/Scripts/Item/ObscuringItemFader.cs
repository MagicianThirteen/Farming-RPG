using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObscuringItemFader : MonoBehaviour
{
    private SpriteRenderer _renderer;
    private void Awake()
    {
        _renderer = GetComponent<SpriteRenderer>();
    }

    public void FadeIn()
    {
        StartCoroutine(FadeInRoutine());
    }

    public void FadeOut()
    {
        StartCoroutine(FadeOutRoutine());
    }

    //淡入变得更亮
    IEnumerator FadeInRoutine()
    {
        float currAlpha = _renderer.color.a;
        float distance = 1f - currAlpha;
        while (1f-currAlpha>0.01f)
        {
            currAlpha = currAlpha + distance / Settings.fadeInSeconds * Time.deltaTime;
            _renderer.color = new Color(1f, 1f, 1f, currAlpha);
            yield return null;
        }

        _renderer.color = new Color(1f, 1f, 1f, 1f);
    }

    //淡出变得更暗
    IEnumerator FadeOutRoutine()
    {
        float currAlpha = _renderer.color.a;
        float distance = currAlpha-Settings.targetAlpha;
        while (currAlpha-Settings.targetAlpha>0.01f)
        {
            currAlpha = currAlpha - distance / Settings.fadeOutSeconds * Time.deltaTime;
            _renderer.color = new Color(1f, 1f, 1f, currAlpha);
            yield return null;
        }

        _renderer.color = new Color(1f, 1f, 1f, Settings.targetAlpha);
    }
}
