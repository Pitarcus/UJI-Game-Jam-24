using UnityEngine.SceneManagement;
using UnityEngine;
using System.Collections;
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

    private bool changing = false;

    private void Start()
    {
        changing = false;
    }
    public void ChangeScene(int index)
    {
        if (!changing)
        {
            changing = true;
            Time.timeScale = 1;
            if (transition != null)
            {
                transition.SetTrigger("Start");
            }
            if (asyncLoad)
                StartCoroutine(LoadLevel(index));
            else
                LoadLevelImmeditate(index);
        }
    }
    IEnumerator LoadLevel(int levelIndex)
    {
        yield return new WaitForSeconds(transitionTime);

        Scene activeScene = SceneManager.GetActiveScene();

        //SceneManager.LoadScene(levelIndex);
        AsyncOperation asyncLoadLevel = SceneManager.LoadSceneAsync(levelIndex, LoadSceneMode.Single);

        // Wait until the level finishes loading
        while (!asyncLoadLevel.isDone)
            yield return null;
        // Wait a frame so every Awake and Start method is called
        yield return new WaitForEndOfFrame();
    }

    void LoadLevelImmeditate(int levelIndex)
    {
        SceneManager.LoadScene(levelIndex, LoadSceneMode.Single);
    }
}
