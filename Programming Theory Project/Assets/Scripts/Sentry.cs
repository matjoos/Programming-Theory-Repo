using UnityEngine;

public class Sentry : Ice
{
    protected override void Start()
    {
        base.Start();

        IceType = "Sentry";
        Strength = 5;
        PointsValue = 200;
    }

    private void Update()
    {
        MoveSquarePattern();
    }

    public override void WinsInterface()
    {
        // When a sentry wins an interface, the player is destroyed
        playerController.Explodes();
        GameManager.Instance.GameOver();
    }

    private void MoveSquarePattern()
    {

    }
}
