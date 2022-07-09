using System;
using UnityEngine;

[CreateAssetMenu]
public class FloatVariable : ScriptableObject, ISerializationCallbackReceiver
{
    public float initialValue;

    [NonSerialized] public float value;


    public void OnBeforeSerialize(){ }

    public void OnAfterDeserialize()
    {
        value = initialValue;
    }
}
