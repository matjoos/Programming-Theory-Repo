using System;
using UnityEngine;

[CreateAssetMenu]
public class BoolVariable : ScriptableObject, ISerializationCallbackReceiver
{
    public bool initialValue;

    [NonSerialized] public bool value;

    public void OnBeforeSerialize() { }

    public void OnAfterDeserialize()
    {
        value = initialValue;
    }
}
