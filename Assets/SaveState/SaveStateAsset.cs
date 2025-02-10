using UnityEngine;
using System;

[CreateAssetMenu(fileName = "SaveState", menuName = "Scriptable Objects/SaveState")]
public class SaveStateAsset : ScriptableObject
{
    public int SaveID;
    public DateTime SaveCreated;
    public DateTime LastSave;
    public int TimePlayedSeconds;



}
