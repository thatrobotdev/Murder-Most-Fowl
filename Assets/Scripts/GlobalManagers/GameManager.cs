using UnityEngine;

[RequireComponent(typeof(SceneManager), typeof(StateManager))]
public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    private SceneManager m_sceneManager;
    public SceneManager SceneManager => m_sceneManager;

    private StateManager m_stateManager;
    public StateManager StateManager => m_stateManager;


    private void Awake()
    {
        if (Instance != null && Instance != this) Destroy(this);
        else Instance = this;

        m_sceneManager = GetComponent<SceneManager>();
        m_stateManager = GetComponent<StateManager>();
    }
}