using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameModeInformer : MonoBehaviour
{
    public GameMode selectedGameMode = GameMode.arcade;

    public static GameModeInformer instance;

    private void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void SetToArcade()
    {
        selectedGameMode = GameMode.arcade;
    }

    public void SetToFree()
    {
        selectedGameMode = GameMode.free;
    }
}
