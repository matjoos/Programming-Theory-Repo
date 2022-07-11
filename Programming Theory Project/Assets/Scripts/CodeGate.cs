using UnityEngine;

public class CodeGate : Ice
{
    [SerializeField] private ParticleSystem explosionParticle;
    [SerializeField] private AudioClip codeGateWinsSound;
    [SerializeField] private AudioClip codeGateDefeatedSound;

    [SerializeField] private IntVariable credits;

    protected override void Start()
    {
        base.Start();

        IceType = "CodeGate";
        Strength = 2;
        PointsValue = 100;
        DoesDestroyPlayer = false;
        DefeatedBy = "CodeBreaker";
    }

    public override void WinsInterface()
    {
        // When a code gate wins an interface, reduce credits to 0
        credits.value = 0;

        iceAudio.PlayOneShot(codeGateWinsSound, 1.0f);
    }

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
