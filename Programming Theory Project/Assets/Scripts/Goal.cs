using UnityEngine;

public class Goal : MonoBehaviour
{
    public int InterfaceCost { get; private set; }
    public int PointsValue { get; private set; }

    private void Start()
    {
        InterfaceCost = 1;
        PointsValue = 500;
    }
}
