using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TimerUIUpdate : MonoBehaviour
{
    private GameManager _gameManager;

    private TextMeshProUGUI _textMeshProUGUI;

    // Start is called before the first frame update
    void Start()
    {
        _gameManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
        _textMeshProUGUI = GetComponent<TextMeshProUGUI>();
    }

    void Update()
    {
        //int timeLeft = [int]_gameManager.GetCurrentTime();

        int seconds = ((int)_gameManager.GetCurrentTime() % 60);
        int minutes = ((int)_gameManager.GetCurrentTime() / 60);
        
        _textMeshProUGUI.text = "Time left: " + string.Format("{0:00}:{1:00}", minutes, seconds);
    }
}
