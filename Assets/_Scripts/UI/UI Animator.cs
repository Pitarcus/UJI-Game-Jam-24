using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;

public class UIAnimator : MonoBehaviour
{
    public enum UIAnimationType
    {
        scale,
        move,
        alpha
    }

    [Header("References")]
    [SerializeField] public CanvasGroup canvasGroup;

    [Header("Parameters")]
    [SerializeField] public UIAnimationType animationType;
    [SerializeField] public float transitionTime;
    [SerializeField] public DG.Tweening.Ease easingType;
    [SerializeField] public float delay;
    [SerializeField] public bool animateOnEnable;

    [SerializeField] public UnityEvent onUIAnimationFinished;

    [Header("Parameters for Alpha")]
    [SerializeField] public bool showCanvasGroup = false;

    private void OnEnable()
    {
        if(animateOnEnable)
        {
            AnimateUI();
        }
    }

    public void AnimateUI()
    {
        switch (animationType)
        {
            case UIAnimationType.scale:
                break;

            case UIAnimationType.move:
                break; 

            case UIAnimationType.alpha:
                ShowGroup(showCanvasGroup);
                break;
        }
    }

    private void UIAnimationFinishedInvoke()
    {
        onUIAnimationFinished?.Invoke();
    }

    #region AlphaAnimation
    private void ShowGroup(bool show)
    {
        if (show)
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



// -------------------- CUSTOM EDITOR FOR THE SCRIPT ---------------------
[CustomEditor(typeof(UIAnimator))]
public class UIAnimatorEditor : Editor
{
    // references
    SerializedProperty canvasGroup;

    // properties
    SerializedProperty animationType;
    SerializedProperty easingType;
    SerializedProperty transitionTime;
    SerializedProperty delay;
    SerializedProperty animateOnEnable;

    // alpha specific properties
    SerializedProperty showCanvasGroup;

    // events
    SerializedProperty onUIAnimationFinished;

    UIAnimator myScript;

    private void OnEnable()
    {
        myScript = target as UIAnimator;

        canvasGroup = serializedObject.FindProperty("canvasGroup");
        animationType = serializedObject.FindProperty("animationType");
        easingType = serializedObject.FindProperty("easingType");
        transitionTime = serializedObject.FindProperty("transitionTime");
        delay = serializedObject.FindProperty("delay");
        animateOnEnable = serializedObject.FindProperty("animateOnEnable");

        showCanvasGroup = serializedObject.FindProperty("showCanvasGroup");

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

        EditorGUILayout.Space(5);

        // Parameters
        EditorGUILayout.PropertyField(animationType);
        EditorGUILayout.PropertyField(transitionTime);
        EditorGUILayout.PropertyField(easingType);
        EditorGUILayout.PropertyField(delay);
        EditorGUILayout.PropertyField(animateOnEnable);

        EditorGUILayout.Space(5);

        // Type sensitive parameters
        if (myScript.animationType == UIAnimator.UIAnimationType.alpha)
        {
            EditorGUILayout.PropertyField(showCanvasGroup);
        }

        EditorGUILayout.Space(5);

        // Events
        EditorGUILayout.PropertyField(onUIAnimationFinished);

        serializedObject.ApplyModifiedProperties();
    }
}
