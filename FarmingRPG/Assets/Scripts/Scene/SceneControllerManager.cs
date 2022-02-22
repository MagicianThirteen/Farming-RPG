using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class SceneControllerManager : SingletonMonobehaviour<SceneControllerManager>
{
    private bool isFade = false;
    [FormerlySerializedAs("faterCanvasGroup")] public CanvasGroup faderCanvasGroup;
    public Image fadeImage;
    [SerializeField]private float fadeDuration = 1f;
    public SceneName startSceneName;
    public void FadeAndLoadScene(string sceneName, Vector3 bornPosition)
    {
        //还没渐变完不换场景
        if (!isFade)
        {
            StartCoroutine(FadeAndSwitchScene(sceneName, bornPosition));
        }
    }

    private IEnumerator FadeAndSwitchScene(string sceneName, Vector3 bornPosition)
    {
        EventHandler.CallBeforeSceneUnloadFadeOutEvent();
        yield return StartCoroutine(Fade(1));

        Player.Instance.transform.position = bornPosition;
       
        EventHandler.CallBeforeSceneUnloadEvent();
        yield return SceneManager.UnloadSceneAsync(SceneManager.GetActiveScene().buildIndex);

        yield return StartCoroutine(LoadSceneAndSetActive(sceneName));
        EventHandler.CallAfterSceneLoadEvent();

        yield return StartCoroutine(Fade(0));
        EventHandler.CallAfterSceneLoadFadeInEvent();
    }

    IEnumerator Fade(float target)
    {
        //设置标志位 false
        isFade = true;
        //点击事件屏蔽
        faderCanvasGroup.blocksRaycasts = true;
        //渐变颜色
        float speed = Mathf.Abs(faderCanvasGroup.alpha - target) / fadeDuration;
        while (!Mathf.Approximately(faderCanvasGroup.alpha,target))
        {
            faderCanvasGroup.alpha = Mathf.MoveTowards(faderCanvasGroup.alpha, target, Time.deltaTime * speed);
            yield return null;
        }
        //设置标志位 true
        isFade = false;
        //放开点击事件屏蔽
        faderCanvasGroup.blocksRaycasts = false;
    }

    IEnumerator LoadSceneAndSetActive(string sceneName)
    {
        yield return SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);
        Scene current = SceneManager.GetSceneAt(SceneManager.sceneCount-1);
        SceneManager.SetActiveScene(current);
    }

    private IEnumerator Start()
    {
        // Set the initial alpha to start off with a black screen.
        fadeImage.color = new Color(0f, 0f, 0f, 1f);
        faderCanvasGroup.alpha = 1f;
        // Start the first scene loading and wait for it to finish.
        yield return StartCoroutine(LoadSceneAndSetActive(startSceneName.ToString()));

        // If this event has any subscribers, call it.
        EventHandler.CallAfterSceneLoadEvent();
        
        // Once the scene is finished loading, start fading in.
        StartCoroutine(Fade(0f));
    }
}
