using UnityEngine;
using System;
using MessagePack;

[MessagePackObject(keyAsPropertyName: true)]
[Serializable]
public class State
{
    public uint SaveID;
    public uint TimePlayedSeconds;
    public UDateTime SaveCreated;
    public UDateTime LastSaved;

    public bool TestTag;
}
