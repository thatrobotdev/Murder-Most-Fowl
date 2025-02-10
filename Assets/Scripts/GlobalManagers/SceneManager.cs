using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

[RequireComponent(typeof(GameManager))]
public class SceneManager : MonoBehaviour
{
    [SerializeField]
    private string m_startScene;
    public string StartScene => m_startScene;

    private AsyncOperation m_nextScene;
    public AsyncOperation NextScene => m_nextScene;

    private void Start()
    {
        StartCoroutine(LoadSceneAndSwap(m_startScene));
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            QuitApplication();
        }
    }

    public IEnumerator LoadScene(string sceneName)
    {
        m_nextScene = UnityEngine.SceneManagement.SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Single);
        m_nextScene.allowSceneActivation = false;

        while (!m_nextScene.isDone && m_nextScene.progress < 0.9f) yield return null;
    }

    public IEnumerator LoadSceneAndSwap(string sceneName)
    {
        yield return LoadScene(sceneName);
        SwapScene();
    }

    public void SwapScene()
    {
        if (m_nextScene == null) return;
        
        m_nextScene.allowSceneActivation = true;
    }

    public bool IsSceneReady()
    {
        return m_nextScene != null && m_nextScene.isDone;
    }

    public void QuitApplication()
    {
        #if UNITY_STANDALONE
            Application.Quit();
        #endif
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
        #endif
    }
}