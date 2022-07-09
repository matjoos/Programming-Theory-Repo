using System;
using UnityEngine;

[CreateAssetMenu]
public class IceBreaker : ScriptableObject, ISerializationCallbackReceiver
{
    public string initialName;
    public int initialStrength;
    public int initialInterfaceCost;
    public Color initialColor;

    [NonSerialized] public new string name;
    [NonSerialized] public int strength;
    [NonSerialized] public int interfaceCost;
    [NonSerialized] public Color color;

    public void OnBeforeSerialize() { }

    public void OnAfterDeserialize()
    {
        name = initialName;
        strength = initialStrength;
        interfaceCost = initialInterfaceCost;
        color = initialColor;
    }
}
