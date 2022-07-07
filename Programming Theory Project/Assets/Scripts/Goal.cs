using UnityEngine;

public class Goal : MonoBehaviour
{
    public int InterfaceCost { get; private set; }
    public int PointsValue { get; private set; }

    private AudioSource goalAudio;

    [SerializeField] private ParticleSystem fireworks;
    [SerializeField] private AudioClip notEnoughCreditsSound;
    [SerializeField] private AudioClip levelClearedSound;

    private void Start()
    {
        InterfaceCost = 1;
        PointsValue = 500;

        goalAudio = GetComponent<AudioSource>();
    }

    public void Explode()
    {
        fireworks.Play();
        goalAudio.PlayOneShot(levelClearedSound, 1.0f);
        GetComponent<Renderer>().enabled = false;
        foreach (Collider collider in GetComponents<Collider>())
        {
            collider.enabled = false;
        }
    }

    public void NotDefeated()
    {
        goalAudio.PlayOneShot(notEnoughCreditsSound);
    }
}
