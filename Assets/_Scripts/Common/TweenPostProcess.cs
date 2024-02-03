using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

[RequireComponent(typeof(Volume))]
public class TweenPostProcess : MonoBehaviour
{
    [SerializeField] private float transitionTime;
    [SerializeField] private Ease easingType;

    private Volume postProcessVolume;

    private void Awake()
    {
        postProcessVolume = transform.GetComponent<Volume>();
    }

   public void TweenVolumeWeight(int currentLevel, float initalWeight, float endWeight, float transitionTime)
    {
        DOVirtual.Float(initalWeight, endWeight, transitionTime, SetVolumeWeight).SetEase(easingType);
    }

    public void FlashEffect()
    {
        Sequence seq = DOTween.Sequence();

        seq.Append(DOVirtual.Float(0, 1, transitionTime, SetVolumeWeight).SetEase(easingType));
        seq.Append(DOVirtual.Float(1, 0, transitionTime * 2, SetVolumeWeight).SetEase(easingType));
    }

    private void SetVolumeWeight(float value)
    {
        postProcessVolume.weight = value;
    }
}
