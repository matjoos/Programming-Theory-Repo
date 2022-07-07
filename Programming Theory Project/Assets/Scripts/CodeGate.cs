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
        // If credits already 0, destroy player
        if (playerController.credits == 0)
        {
            playerController.Explodes();
            GameManager.Instance.GameOver();
        }
        else
        {
            playerController.credits = 0;
        }
    }
}
