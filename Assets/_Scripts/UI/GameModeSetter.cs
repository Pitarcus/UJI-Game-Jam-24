using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameModeSetter : MonoBehaviour
{

    private GameModeInformer _Informer;
    private void Start()
    {
        _Informer = GameObject.FindGameObjectWithTag("GameMode").GetComponent<GameModeInformer>();
    }

    public void SetArcade()
    {
        _Informer.SetToArcade();
    }

    public void SetFree()
    {
        _Informer.SetToFree();
    }
}
