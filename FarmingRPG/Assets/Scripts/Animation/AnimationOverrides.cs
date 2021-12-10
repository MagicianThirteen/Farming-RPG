using System;
using System.Collections;
using System.Collections.Generic;
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
        Animator currAnimator;
        //找到当前是玩家身上哪个动画控制器（通过传过来的characterAttributes.CharacterPartAnimator)
        if (character != null)
        {
            Animator[] animators = character.GetComponentsInChildren<Animator>();
            
        }
    }
}
