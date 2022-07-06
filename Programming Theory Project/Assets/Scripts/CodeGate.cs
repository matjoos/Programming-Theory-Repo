using UnityEngine;

public class CodeGate : Ice
{ 
    protected override void Start()
    {
        base.Start();

        IceType = "CodeGate";
        Strength = 2;
        PointsValue = 100;
    }

    public override void WinsInterface()
    {
        // When a code gate wins an interface, reduce credits to 0
        playerController.credits = 0;
    }
}
