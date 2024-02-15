using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PauseEvents : MonoBehaviour
{

    public UnityEvent onOutOfPause;
   public UnityEvent onOutOfPauseFree;

    public void SetOutOfPause()
    {
        if(GameManager.Instance.currentGameMode == GameMode.arcade)
            onOutOfPause.Invoke();
        else
        {
            Debug.Log("outofpause free");
            onOutOfPauseFree.Invoke();
        }
    }
}
