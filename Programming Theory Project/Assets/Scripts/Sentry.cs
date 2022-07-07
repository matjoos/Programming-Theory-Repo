using UnityEngine;

public class Sentry : Ice
{
    [SerializeField] private ParticleSystem explosionParticle;
    [SerializeField] private AudioClip sentryDefeatedSound;

    protected override void Start()
    {
        base.Start();

        IceType = "Sentry";
        Strength = 1;
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

    public override void LosesInterface()
    {
        explosionParticle.Play();
        iceAudio.PlayOneShot(sentryDefeatedSound, 1.0f);

        // Switch off the renderer and all colliders.
        // This way the game object still exists (explosion can continue),
        // but doesn't do anything
        GetComponent<Renderer>().enabled = false;
        foreach (Collider collider in GetComponents<Collider>())
        {
            collider.enabled = false;
        } 
    }

    private void MoveSquarePattern()
    {

    }
}
