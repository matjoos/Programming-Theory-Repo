using UnityEngine;

public class Sentry : Ice
{
    [SerializeField] private ParticleSystem explosionParticle;
    [SerializeField] private AudioClip sentryDefeatedSound;

    private Vector3 currentDirection = Vector3.forward;
    private float zTopBound = 3.5f;
    private float zLowerBound = -2.5f;
    private float xRange = 4.5f;
    private float sentrySpeed = 1.0f;

    protected override void Start()
    {
        base.Start();

        IceType = "Sentry";
        Strength = 1;
        PointsValue = 200;
        DoesDestroyPlayer = true;
        DefeatedBy = "Killer";
    }

    private void Update()
    {
        // The sentry moves in a square pattern,
        // changing direction when reaching boundaries
        if (currentDirection == Vector3.forward && transform.position.z >= zTopBound)
        {
            currentDirection = Vector3.left;
        }
        else if (currentDirection == Vector3.left && transform.position.x <= -xRange)
        {
            currentDirection = Vector3.back;
        }
        else if (currentDirection == Vector3.back && transform.position.z <= zLowerBound)
        {
            currentDirection = Vector3.right;
        }
        else if (currentDirection == Vector3.right && transform.position.x >= xRange)
        {
            currentDirection = Vector3.forward;
        }

        transform.position += currentDirection * Time.deltaTime * sentrySpeed;
    }

    public override void WinsInterface()
    {
        // When a sentry wins an interface, the player is destroyed.
        // The DoesDestroyPlayer bool handles this.
    }

    public override void LosesInterface()
    {
        explosionParticle.Play();
        iceAudio.PlayOneShot(sentryDefeatedSound, 1.0f);

        GetComponent<Renderer>().enabled = false;

        foreach (Collider collider in GetComponents<Collider>())
        {
            collider.enabled = false;
        } 
    }
}
