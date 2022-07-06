using UnityEngine;

public abstract class Ice : MonoBehaviour
{
    protected PlayerController playerController;

    public string IceType { get; protected set; }
    public int Strength { get; protected set; }
    public int PointsValue { get; protected set; }

    public abstract void WinsInterface();

    protected virtual void Start()
    {
        playerController = GameObject.Find("Player").GetComponent<PlayerController>();
    }
}
