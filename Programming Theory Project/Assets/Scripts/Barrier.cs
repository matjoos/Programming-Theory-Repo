using UnityEngine;

// INHERITANCE
public class Barrier : Ice
{
    [SerializeField] private ParticleSystem explosionParticle;
    [SerializeField] private AudioClip barrierWinsSound;
    [SerializeField] private AudioClip barrierDefeatedSound;

    // POLYMORPHISM
    protected override void Start()
    {
        base.Start();

        IceType = "Barrier";
        Strength = 1;
        PointsValue = 50;
    }

    // POLYMORPHISM
    public override void WinsInterface()
    {
        // Nothing happens to the player when a barrier wins an interface
        iceAudio.PlayOneShot(barrierWinsSound, 1.0f);
        return;
    }

    // POLYMORPHISM
    public override void LosesInterface()
    {
        explosionParticle.Play();
        iceAudio.PlayOneShot(barrierDefeatedSound, 1.0f);
        GetComponent<Renderer>().enabled = false;
        foreach (Collider collider in GetComponents<Collider>())
        {
            collider.enabled = false;
        }
    }
}
