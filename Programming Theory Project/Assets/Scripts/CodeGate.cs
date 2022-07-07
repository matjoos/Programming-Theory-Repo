using UnityEngine;

// INHERITANCE
public class CodeGate : Ice
{
    [SerializeField] private ParticleSystem explosionParticle;
    [SerializeField] private AudioClip codeGateWinsSound;
    [SerializeField] private AudioClip codeGateDefeatedSound;

    // POLYMORPHISM
    protected override void Start()
    {
        base.Start();

        IceType = "CodeGate";
        Strength = 2;
        PointsValue = 100;
    }

    // POLYMORPHISM
    public override void WinsInterface()
    {
        // When a code gate wins an interface, reduce credits to 0
        playerController.credits = 0;

        iceAudio.PlayOneShot(codeGateWinsSound, 1.0f);
    }

    // POLYMORPHISM
    public override void LosesInterface()
    {
        explosionParticle.Play();
        iceAudio.PlayOneShot(codeGateDefeatedSound, 1.0f);
        GetComponent<Renderer>().enabled = false;
        foreach (Collider collider in GetComponents<Collider>())
        {
            collider.enabled = false;
        }
    }
}
