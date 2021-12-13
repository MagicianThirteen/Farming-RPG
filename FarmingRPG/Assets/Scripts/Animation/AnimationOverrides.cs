using System;
using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;

public class AnimationOverrides : MonoBehaviour
{
    private Dictionary<AnimationClip, SO_AnimationType> animationTypeDictionaryByAnimation;
    private Dictionary<string, SO_AnimationType> animationTypeDictionaryByCompositeAttributeKey;
    [SerializeField] private SO_AnimationType[] _soAnimationTypes;
    [SerializeField] private GameObject character;

    private void Awake()
    {
        CreateAnimationTypeDictionarys();
    }

    private void CreateAnimationTypeDictionarys()
    {
        animationTypeDictionaryByAnimation = new Dictionary<AnimationClip, SO_AnimationType>();
        animationTypeDictionaryByCompositeAttributeKey = new Dictionary<string, SO_AnimationType>();
        foreach (var so in _soAnimationTypes)
        {
            animationTypeDictionaryByAnimation.Add(so.animationClip,so);
            string key = so.characterPart.ToString() + so.partVariantColour.ToString() + so.partVariantType.ToString() +
                         so.animationName.ToString();
            animationTypeDictionaryByCompositeAttributeKey.Add(key,so);
        }
    }

    public void ApplyCharacterCustomisationParameters(List<CharacterAttribute> characterAttributes)
    {
        Animator[] animators = character.GetComponentsInChildren<Animator>();
        Animator currAnimator=new Animator();
        foreach (var characterAttribute in characterAttributes)
        {
            string partName = characterAttribute.characterPart.ToString();
            foreach (var animator in animators)
            {
                string animatorName = animator.name;
                if (partName == animatorName)
                {
                    currAnimator = animator;
                    break;//跳出当前循环
                }
            }

            List<KeyValuePair<AnimationClip, AnimationClip>> swapAnimationClips =
                new List<KeyValuePair<AnimationClip, AnimationClip>>();
            AnimatorOverrideController aoc = new AnimatorOverrideController(currAnimator.runtimeAnimatorController);
            //获得当前控制器有的动画片段
            List<AnimationClip> animationsList = new List<AnimationClip>(aoc.animationClips);
            //再次遍历当前有的动画片段
            foreach (var animationClip in animationsList)
            {
                SO_AnimationType foundSoAnimationType = ScriptableObject.CreateInstance<SO_AnimationType>();
                SO_AnimationType swapSoAnimationType = ScriptableObject.CreateInstance<SO_AnimationType>();
                animationTypeDictionaryByAnimation.TryGetValue(animationClip, out foundSoAnimationType);
                if (foundSoAnimationType!= null)
                {
                    string key = characterAttribute.characterPart.ToString() +
                                 characterAttribute.partVariantColour.ToString() +
                                 characterAttribute.partVariantType.ToString() + foundSoAnimationType.animationName;
                    animationTypeDictionaryByCompositeAttributeKey.TryGetValue(key, out swapSoAnimationType);
                    if (swapSoAnimationType.animationClip != null)
                    {
                        swapAnimationClips.Add(new KeyValuePair<AnimationClip, AnimationClip>(animationClip,swapSoAnimationType.animationClip));
                    }
                }
            }
            //将当前动画控制器里的动画片段替换成这个，然后把替换好的动画控制器，给当前控制器
            aoc.ApplyOverrides(swapAnimationClips);
            currAnimator.runtimeAnimatorController = aoc;
        }
        
    }
}
