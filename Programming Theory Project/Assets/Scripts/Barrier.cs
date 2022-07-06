using UnityEngine;

public class Barrier : Ice
{  
    protected override void Start()
    {
        base.Start();

        IceType = "Barrier";
        Strength = 1;
        PointsValue = 50;
    }

    public override void WinsInterface()
    {
        // Nothing happens when a barrier wins an interface
        return;

        // TODO Maybe make a specific sound
    }
}
