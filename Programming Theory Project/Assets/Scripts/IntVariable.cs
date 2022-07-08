using System;
using UnityEngine;

[CreateAssetMenu]
public class IntVariable : ScriptableObject, ISerializationCallbackReceiver
{
    public int initialValue;

    [NonSerialized]
    public int value;

    public void OnBeforeSerialize() { }

    public void OnAfterDeserialize()
    {
        value = initialValue;
    }
}
