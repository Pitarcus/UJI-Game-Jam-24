using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UIElements;

[System.Serializable]
public class UIAnimator : MonoBehaviour
{
    public enum UIAnimationType
    {
        scale,
        move,
        shake,
        alpha

    }

    [Header("References")]
    [SerializeField] public CanvasGroup canvasGroup;
    [SerializeField] public RectTransform transformToMove;

    [Header("Parameters")]
    [SerializeField] public UIAnimationType animationType;
    [SerializeField] public float transitionTime;
    [SerializeField] public DG.Tweening.Ease easingType;
    [SerializeField] public float delay;
    [SerializeField] public bool animateOnEnable;
    [SerializeField] public bool playInverted = false;

    [SerializeField] public UnityEvent onUIAnimationFinished;


    [Header("Parameters for Move")]
    [SerializeField] public Vector3 toScale;

    [Header("Parameters for Alpha")]

    [Header("Parameters for Move")]
    [SerializeField] public Vector2 toPosition;
    private Vector2 _originalPosition;

    [Header("Parameters for Shake")]
    public float shakeStrength;

    private void OnEnable()
    {
        if (animationType == UIAnimationType.move)
        {
            _originalPosition = transformToMove.anchoredPosition;
        }
        if (animateOnEnable)
        {
            AnimateUI();
        }
    }

    public void AnimateUI()
    {
        switch (animationType)
        {
            case UIAnimationType.scale:
                Scale();
                break;

            case UIAnimationType.move:
                MoveTransform();
                break;

            case UIAnimationType.shake:
                Shake();
                break;

            case UIAnimationType.alpha:
                AnimateGroupAlpha();
                break;
        }
    }

    private void UIAnimationFinishedInvoke()
    {
        onUIAnimationFinished?.Invoke();
    }

    #region ScaleAnimation

    private void Scale()
    {
        transformToMove.DOScale(toScale, transitionTime).SetEase(easingType).SetDelay(delay);
    }

    #endregion

    #region MoveAnimation
    private void MoveTransform()
    {
        if(!playInverted)
            transformToMove.DOAnchorPos(toPosition, transitionTime).SetEase(easingType).SetDelay(delay).OnComplete(UIAnimationFinishedInvoke);
        else
            transformToMove.DOAnchorPos(_originalPosition, transitionTime).SetEase(easingType).SetDelay(delay).OnComplete(UIAnimationFinishedInvoke);
    }

    #endregion

    #region ScaleAnimation
    private void Shake()
    {
        transformToMove.DOShakeAnchorPos(transitionTime, shakeStrength).SetEase(easingType).SetDelay(delay);
    }
    #endregion

    #region AlphaAnimation
    private void AnimateGroupAlpha()
    {
        if (!playInverted)
        {
            TweenAlpha(0, 1, transitionTime);
        }
        else
        {
            TweenAlpha(1, 0, transitionTime);
        }
    }

    void TweenAlpha(float init, float end, float time)
    {
        DOVirtual.Float(init, end, transitionTime, SetAlpha).SetEase(easingType).SetDelay(delay).OnComplete(UIAnimationFinishedInvoke);
    }

    void SetAlpha(float value)
    {
        canvasGroup.alpha = value;
    }
    #endregion
}


#if UNITY_EDITOR
// -------------------- CUSTOM EDITOR FOR THE SCRIPT ---------------------
[CustomEditor(typeof(UIAnimator))]
public class UIAnimatorEditor : Editor
{
    // references
    SerializedProperty canvasGroup;
    SerializedProperty transformToMove;

    // properties
    SerializedProperty animationType;
    SerializedProperty easingType;
    SerializedProperty transitionTime;
    SerializedProperty delay;
    SerializedProperty animateOnEnable;
    SerializedProperty playInverted;


    // move specific properties
    SerializedProperty toPosition;
    SerializedProperty shakeStrength;
    SerializedProperty toScale;

    // events
    SerializedProperty onUIAnimationFinished;

    UIAnimator myScript;

    private void OnEnable()
    {
        myScript = target as UIAnimator;

        canvasGroup = serializedObject.FindProperty("canvasGroup");
        transformToMove = serializedObject.FindProperty("transformToMove");

        animationType = serializedObject.FindProperty("animationType");
        easingType = serializedObject.FindProperty("easingType");
        transitionTime = serializedObject.FindProperty("transitionTime");
        delay = serializedObject.FindProperty("delay");
        animateOnEnable = serializedObject.FindProperty("animateOnEnable");
        shakeStrength = serializedObject.FindProperty("shakeStrength");


        toPosition = serializedObject.FindProperty("toPosition");
        playInverted = serializedObject.FindProperty("playInverted");
        toScale = serializedObject.FindProperty("toScale");

        onUIAnimationFinished = serializedObject.FindProperty("onUIAnimationFinished");
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        // References
        if (myScript.animationType == UIAnimator.UIAnimationType.alpha)
        {
            EditorGUILayout.PropertyField(canvasGroup);

        }
        else if (myScript.animationType == UIAnimator.UIAnimationType.move ||
            myScript.animationType == UIAnimator.UIAnimationType.shake ||
            myScript.animationType == UIAnimator.UIAnimationType.scale)
        {
            EditorGUILayout.PropertyField(transformToMove);
        }

        EditorGUILayout.Space(5);

        // Parameters
        EditorGUILayout.PropertyField(animationType);
        EditorGUILayout.PropertyField(transitionTime);
        EditorGUILayout.PropertyField(easingType); 
        EditorGUILayout.PropertyField(delay);
        EditorGUILayout.PropertyField(animateOnEnable);
        EditorGUILayout.PropertyField(playInverted);
        

        EditorGUILayout.Space(5);

        // Type sensitive parameters
        if (myScript.animationType == UIAnimator.UIAnimationType.move)
        {
            EditorGUILayout.PropertyField(toPosition);
        }
        else if (myScript.animationType == UIAnimator.UIAnimationType.shake)
        {
            EditorGUILayout.PropertyField(shakeStrength);
        }
        else if (myScript.animationType == UIAnimator.UIAnimationType.scale)
        {
            EditorGUILayout.PropertyField(toScale);
        }

        EditorGUILayout.Space(5);

        // Events
        EditorGUILayout.PropertyField(onUIAnimationFinished);

        serializedObject.ApplyModifiedProperties();
    }
}
#endif