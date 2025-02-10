using UnityEngine;
using System;
using System.IO;
using System.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using MessagePack;

[RequireComponent(typeof(GameManager))]
public class StateManager : MonoBehaviour
{
    private string m_saveDirectory;
    private List<uint> m_saves;

    private State m_activeState;
    public State ActiveState => m_activeState;

    [SerializeField]
    private StateAsset m_stateOverride;

    private void Awake()
    {
        if (m_stateOverride != null) m_activeState = m_stateOverride.State;

        m_saveDirectory = Application.persistentDataPath + "/saves/";
        Directory.CreateDirectory(m_saveDirectory);

        m_saves = new List<uint>();
        foreach (string s in Directory.GetFiles(m_saveDirectory))
        {
            m_saves.Add(uint.Parse(Path.GetFileNameWithoutExtension(s)));
        }
    }

    private void Start()
    {
        if (m_activeState == null) NewState();
    }

    private void Update()
    {

    }

    public void NewState()
    {
        m_activeState = new State();
        m_activeState.SaveID = AssignSaveID();
        m_activeState.SaveCreated = DateTime.Now;
    }

    private uint AssignSaveID()
    {
        uint i = 0;
        while (m_saves.Contains(i)) i++;
        return i;
    }

    public Coroutine Save() 
    {
        if (m_activeState == null) return null;
        m_activeState.LastSaved = DateTime.Now;
        return StartCoroutine(ISave());
    }

    private IEnumerator ISave()
    {
        string path = m_saveDirectory + m_activeState.SaveID + ".dat";
        using FileStream fs = File.Create(path);
        Task t = MessagePackSerializer.SerializeAsync(fs, m_activeState);
        yield return new WaitUntil(() => t.IsCompleted);
    }

    public Coroutine Load(uint saveID)
    {
        string path = m_saveDirectory + saveID + ".dat";
        if (!File.Exists(path)) return null;
        return StartCoroutine(ILoad(path));
    }

    private IEnumerator ILoad(string path)
    {
        using FileStream fs = File.OpenRead(path);
        ValueTask<State> t = MessagePackSerializer.DeserializeAsync<State>(fs);
        yield return new WaitUntil(() => t.IsCompleted);
        m_activeState = t.Result;
    }
}
