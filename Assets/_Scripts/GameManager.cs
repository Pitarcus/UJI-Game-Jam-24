using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GameManager : MonoBehaviour
{
    [Header("References")]
    [SerializeField] public LightingManager lightingManager;

    [Header("Parameters")]
    [SerializeField] public float maxTime;

    [Header("Events")]
    [SerializeField] public UnityEvent onTimerStarted; // called when the player is playing (probably after catching the first object)
    [SerializeField] public UnityEvent onTimerEnded; 

    private float _currentTime;
    private bool _startedTimer = false;

    private StickyBallMechanic _stickyBall;

    private void Awake()
    {
        _stickyBall = GameObject.FindGameObjectWithTag("StickyBall").GetComponent<StickyBallMechanic>();
    }

    private void Start()
    {
        _currentTime = maxTime;

        lightingManager.StopCycle();
    }

    private void Update()
    {
        if (_startedTimer)
        {
            if (_currentTime > 0)
            {
                _currentTime -= Time.deltaTime;
            }
            else
            {
                // END GAME
                onTimerEnded.Invoke();
                lightingManager.StopCycle();
            }
        }
    }

    public void StartTimer(int currentSize)
    {
        _startedTimer = true;

        lightingManager.StartCycle();

        onTimerStarted.Invoke();
        _stickyBall.onIncreaseSize.RemoveListener(StartTimer);
    }

    public float GetCurrentTime()
    {
        return _currentTime;
    }
}
