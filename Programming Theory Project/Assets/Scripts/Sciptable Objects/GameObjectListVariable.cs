using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class GameObjectListVariable : ScriptableObject, ISerializationCallbackReceiver
{
    public List<GameObject> initialValue;

    [NonSerialized] public List<GameObject> value;


    public void OnBeforeSerialize() { }

    public void OnAfterDeserialize()
    {
        value = initialValue;
    }
}
