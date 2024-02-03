using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ScoreUpdate : MonoBehaviour
{
    private StickyBallMechanic stickyBall;
   
    private TextMeshProUGUI textMeshProUGUI;

    void Awake()
    {
        stickyBall = GameObject.FindGameObjectWithTag("StickyBall").GetComponent<StickyBallMechanic>();
        //stickyBall.onIncreaseSize.AddListener(UpdateScoreUI);

        textMeshProUGUI = GetComponent<TextMeshProUGUI>();
    }

    private void OnDestroy()
    {
        //stickyBall.onIncreaseSize.RemoveAllListeners();
    }

    public void UpdateScoreUI(int size)
    {
        textMeshProUGUI.text = "Current size: " + size;
    }
}
