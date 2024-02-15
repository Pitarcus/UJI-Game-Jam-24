using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.XR;

public enum GameMode
{
    arcade,
    free
}

public class GameManager : MonoBehaviour
{
    [Header("References")]
    public LightingManager lightingManager;
    public GameObject HUD;

    [Header("Parameters")]
    [SerializeField] public float maxTime;

    [Header("Events")]
    [SerializeField] public UnityEvent onTimerStarted; // called when the player is playing (probably after catching the first object)
    [SerializeField] public UnityEvent onTimerEnded;

    [SerializeField] public UnityEvent onEscapePressed;

    private float _currentTime;
    private bool _startedTimer = false;

    private StickyBallMechanic _stickyBall;

    private bool _useTimer;

    private bool _isOnPause = false;

    public GameMode currentGameMode;

    public static GameManager Instance { get; private set; }

    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }
        else
        {
            GameObject.Destroy(this);
        }

    }

    private void Start()
    {
        SetGameMode();
        _currentTime = maxTime;
    }

    private void SetGameMode()
    {
        // Is on game scene
        _stickyBall = GameObject.FindGameObjectWithTag("StickyBall").GetComponent<StickyBallMechanic>();
        lightingManager = GameObject.FindGameObjectWithTag("LightManager").GetComponent<LightingManager>();
        HUD = GameObject.FindGameObjectWithTag("HUD");

        currentGameMode = GameObject.FindGameObjectWithTag("GameMode").GetComponent<GameModeInformer>().selectedGameMode;
        Debug.Log("Current game mode: " + currentGameMode);
        if (currentGameMode == GameMode.arcade)
        {
            HUD.SetActive(true);
            _useTimer = true;

            lightingManager.StopCycle();
        }
        else
        {
            HUD.SetActive(false);
            _useTimer = false;

            lightingManager.StartCycle();
        }
    }

    public void ToggleTimer()
    {
        _useTimer = !_useTimer;
    }

    public void SetOutOfPause()
    {
        _isOnPause = false;
        ToggleTimer();
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape) && !_isOnPause)
        {
            onEscapePressed.Invoke();
            ToggleTimer();
            _isOnPause = true;
        }

        // Timer stuff
        if (!_useTimer)
        {
            return;
        }
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
        if (!_useTimer || _startedTimer)
            return;

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
