using DG.Tweening;
using UnityEngine;

public class UIAnimatorSequence : MonoBehaviour
{
    [System.Serializable]
    private class UIAnimatorSequenceElement
    {
        public UIAnimator UIAnimator;
        public float delay;
    }

    [SerializeField] UIAnimatorSequenceElement[] animators;
    [SerializeField] bool playOnEnable = false;
    //[SerializeField] bool playInverted = false;
    [SerializeField] [Tooltip("Invert the animations on each call of the sequence")] bool toggleInverted = false;

    //private bool invertedSequence = false;
    private bool playedOnce = false;

    private void OnEnable()
    {
        if(playOnEnable)
        {
            PlaySequence();
        }
    }

    public void PlaySequence()
    {
        Sequence sequence = DOTween.Sequence();

        foreach (UIAnimatorSequenceElement animator in animators) {
            sequence.AppendInterval(animator.delay);

            if (toggleInverted && playedOnce)
            {
                // Invert sequence when it should be inverting
                animator.UIAnimator.playInverted = !animator.UIAnimator.playInverted;
            }

            sequence.AppendCallback(animator.UIAnimator.AnimateUI);
        }

        sequence.Play();

        playedOnce = true;
    }
}
