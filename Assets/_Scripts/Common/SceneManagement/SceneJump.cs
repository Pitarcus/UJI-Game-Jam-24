using UnityEngine.SceneManagement;
using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;
//using UnityEngine.InputSystem;

public class SceneJump : MonoBehaviour
{
    //PlayerInput input;
    [SerializeField]
    private Animator transition;
    [SerializeField]
    private float transitionTime;
    [SerializeField]
    private bool asyncLoad = true;
    [SerializeField]
    private bool asyncLoadWithLoadingScreen = false;
    [SerializeField] private int SceneToUnload = 0;

    


    // Private memebers
    private bool _changing = false;
    private float _sceneProgress = 0f;

   
    private GameObject loadingScreen;
    private Slider progressBar;

    public static SceneJump Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null)
        {
            //Destroy(gameObject);
            return;
        }

        Instance = this;
        if(SceneManager.GetActiveScene().buildIndex == 0)
            SceneManager.LoadSceneAsync(1, LoadSceneMode.Additive);

        // DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        _changing = false;

        loadingScreen = SceneLoaderManager.instance.loadingScreen;
        progressBar = SceneLoaderManager.instance.progressBar;
    }

    public void ChangeSceneSelf(int index)
    {
        if (!_changing)
        {
            _changing = true;

            Time.timeScale = 1;
            if (transition != null)
            {
                transition.SetTrigger("Start");
            }

            if (asyncLoad)
                StartCoroutine(LoadLevel(index));

            else if (asyncLoadWithLoadingScreen)
                StartCoroutine(LoadLevelWithLoadingScreen(index));

            else
                LoadLevelImmeditate(index);
        }
    }

    public void ChangeScene(int index)
    {
        if(Instance != this)
        {
            Instance.SceneToUnload = SceneToUnload;
            Instance.ChangeScene(index);
            Debug.Log("Telling main scene manager to change scene");
            Debug.Log(index);
            return;
        }
        Debug.Log("IM THE INSTANCE");
        if (!_changing)
        {
            _changing = true;

            Time.timeScale = 1;
            if (transition != null)
            {
                transition.SetTrigger("Start");
            }

            if (asyncLoad)
                StartCoroutine(LoadLevel(index));

            else if (asyncLoadWithLoadingScreen)
                StartCoroutine(LoadLevelWithLoadingScreen(index));

            else 
                LoadLevelImmeditate(index);
        }
    }
    IEnumerator LoadLevel(int levelIndex)
    {
        yield return new WaitForSeconds(transitionTime);

        Scene activeScene = SceneManager.GetActiveScene();
        SceneManager.UnloadSceneAsync(SceneToUnload);
        AsyncOperation asyncLoadLevel = SceneManager.LoadSceneAsync(levelIndex, LoadSceneMode.Additive);

        // Wait until the level finishes loading
        while (!asyncLoadLevel.isDone)
            yield return null;
        // Wait a frame so every Awake and Start method is called
        yield return new WaitForEndOfFrame();

        _changing = false;
    }

    IEnumerator LoadLevelWithLoadingScreen(int levelIndex)
    {
        loadingScreen.GetComponent<UIAnimatorSequence>().PlaySequence();
        progressBar.value = 0;

        yield return new WaitForSeconds(transitionTime);

        SceneManager.UnloadSceneAsync(SceneToUnload);
        AsyncOperation asyncLoadLevel = SceneManager.LoadSceneAsync(levelIndex, LoadSceneMode.Additive);

        // Wait until the level finishes loading
        while (!asyncLoadLevel.isDone)
        {
            _sceneProgress += asyncLoadLevel.progress;
            _sceneProgress *= 100;

            progressBar.value = _sceneProgress;

            yield return null;
        }


        yield return new WaitForEndOfFrame();
        yield return new WaitForSeconds(1f);

        loadingScreen.GetComponent<UIAnimatorSequence>().PlaySequence();

        _changing = false;
        _sceneProgress = 0f;
    }

    void LoadLevelImmeditate(int levelIndex)
    {
        SceneManager.LoadScene(levelIndex, LoadSceneMode.Single);

        _changing = false;
    }
}
