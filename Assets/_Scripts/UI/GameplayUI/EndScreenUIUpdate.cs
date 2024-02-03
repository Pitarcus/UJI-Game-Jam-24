using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class EndScreenUIUpdate : MonoBehaviour
{
    private int score;

    private TextMeshProUGUI textMeshProUGUI;

    private void OnEnable()
    {
        score = StickyBallMechanic.Instance.sizeLevel;
        textMeshProUGUI = GetComponent<TextMeshProUGUI>();
    }

    public void RevealScore()
    {
        DOVirtual.Float(0, (float)score, 2f, UpdateText).OnComplete(ShakeText);
    }

    private void UpdateText(float score)
    {
        textMeshProUGUI.text = score.ToString("0");
    }

    private void ShakeText()
    {
        textMeshProUGUI.rectTransform.DOShakeAnchorPos(0.3f, 30, 20, 80);
    }

}
