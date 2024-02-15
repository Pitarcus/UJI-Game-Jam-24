using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SceneLoaderManager : MonoBehaviour
{

    [SerializeField]
    public GameObject loadingScreen;
    [SerializeField]
    public Slider progressBar;

    public static SceneLoaderManager instance;

    private void Awake()
    {
        if(instance != null) 
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
        DontDestroyOnLoad(gameObject);
    }
}
