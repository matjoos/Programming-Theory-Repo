using UnityEngine;

public abstract class Ice : MonoBehaviour
{
    protected AudioSource iceAudio;

    public string IceType { get; protected set; }
    public int Strength { get; protected set; }
    public int PointsValue { get; protected set; }
    public bool DoesDestroyPlayer { get; protected set; }

    public abstract void WinsInterface();

    public abstract void LosesInterface();

    protected virtual void Start()
    {
        iceAudio = GetComponent<AudioSource>();
    }
}
