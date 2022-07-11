using UnityEngine;
using UnityEngine.Events;

public class PlayerController : MonoBehaviour
{
    public static UnityEvent OnScoreChanged = new UnityEvent();
    public static UnityEvent OnCreditsChanged = new UnityEvent();
    public static UnityEvent<IceBreaker> OnIceBreakerChanged = new();

    private Renderer playerRenderer;
    private Rigidbody playerRigidBody;

    [SerializeField] private ParticleSystem explosionParticle;

    private AudioSource playerAudio;
    [SerializeField] private AudioClip creditSound, notEnoughCreditsSound;
    [SerializeField] private AudioClip explodeSound, switchBreakerSound;

    [SerializeField] private IntVariable credits, score;
    [SerializeField] private IceBreaker fracter, codeBreaker, killer;

    [SerializeField] private GameObjectListVariable deactivatedCredits;

    [SerializeField] private BoolVariable isGameOver;

    private float speed = 8.0f;
    private float xRange = 7.7f;
    private float zLowerBound = -3.7f;
    private float zUpperBound = 4.7f;

    private IceBreaker[] deck = new IceBreaker[3];
    private int currentIceBreaker = -1;


    private void Start()
    {
        playerRenderer = GetComponent<Renderer>();
        playerRigidBody = GetComponent<Rigidbody>();
        playerAudio = GetComponent<AudioSource>();

        deck[0] = fracter;
        deck[1] = codeBreaker;
        deck[2] = killer;
    }

    private void FixedUpdate()
    {
        if (isGameOver.value == false)
        {
            MovePlayer();
        }
    }

    private void Update()
    { 
        // Switch to next icebreaker on fire button or spacebar
        if (Input.GetButtonDown("Fire1") || Input.GetKeyDown(KeyCode.Space))
        {
            if (isGameOver.value == false) 
            {
                SwitchIceBreaker();
            }  
        }
    }

    private void MovePlayer()
    {
        Vector2 input = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
        Vector3 direction = new Vector3(input.x, 0, input.y);

        playerRigidBody.AddForce(direction.normalized * speed);

        // Keep player inside the boundaries
        Vector3 position = transform.position;

        if (position.x < -xRange) { position.x = -xRange; }
        else if (position.x > xRange) { position.x = xRange; }

        if (position.z < zLowerBound) { position.z = zLowerBound; }
        else if (position.z > zUpperBound) { position.z = zUpperBound; }

        transform.position = position;
    }

    private void SwitchIceBreaker()
    {
        currentIceBreaker = (currentIceBreaker + 1) % 3;

        playerRenderer.material.SetColor("_Color", deck[currentIceBreaker].color);

        OnIceBreakerChanged.Invoke(deck[currentIceBreaker]);

        playerAudio.PlayOneShot(switchBreakerSound, 1.0f);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Credit"))
        {
            CollideWithCredit(other.gameObject);
        }
        else if (other.CompareTag("Ice"))
        {
            CollideWithIce(other.gameObject.GetComponent<Ice>());
        }
        else if (other.CompareTag("Goal"))
        {
            CollideWithGoal(other.gameObject.GetComponent<Goal>());
        }
    }

    private void CollideWithCredit(GameObject credit)
    {
        // Store a reference to the credit for reactivation
        deactivatedCredits.value.Add(credit);

        credit.SetActive(false);
        credits.value++;

        playerAudio.PlayOneShot(creditSound, 1.0f);

        OnCreditsChanged.Invoke();
    }

    private void CollideWithIce(Ice ice)
    {
        InterfaceWithIce(ice);

        OnCreditsChanged.Invoke();
        OnScoreChanged.Invoke();
        if (IsAnIceBreakerSelected())
        {
            OnIceBreakerChanged.Invoke(deck[currentIceBreaker]);
        }
    }

    private void CollideWithGoal(Goal goal)
    {    
        if (credits.value < goal.InterfaceCost)
        {
            goal.NotDefeated();
            return;
        }
        else
        {
            // Level cleared
            credits.value -= goal.InterfaceCost;
            score.value += goal.PointsValue;

            OnCreditsChanged.Invoke();
            OnScoreChanged.Invoke();

            goal.Explode();

            // TODO Replace with code to handle level cleared
            isGameOver.value = true;
        }
    }

    private void InterfaceWithIce(Ice ice)
    {
        // Player loses the interface if no icebreaker is selected
        if (!IsAnIceBreakerSelected())
        {
            IceWinsInterface(ice);
            return;
        }

        // Check if the icebreaker can defeat the ice
        if (ice.DefeatedBy == deck[currentIceBreaker].name)
        {
            MatchedInterface(ice);
        }
        else
        {
            IceWinsInterface(ice);
        }
    }

    private void MatchedInterface(Ice ice)
    {
        // While the player has credits, use credits to increase icebreaker strength
        // untill the icebreaker strength matches the ice strength
        while (deck[currentIceBreaker].strength < ice.Strength && credits.value != 0)
        {
            credits.value--;
            deck[currentIceBreaker].strength++;
        }

        // If the player has credits left over to pay the interface cost,
        // the player pays the cost and wins the interface.
        if (credits.value >= deck[currentIceBreaker].interfaceCost)
        {
            credits.value -= deck[currentIceBreaker].interfaceCost;
            PlayerWinsInterface(ice);
        }
        else
        {
            IceWinsInterface(ice);
        }
    }

    private void IceWinsInterface(Ice ice)
    {
        ice.WinsInterface();
        if (ice.DoesDestroyPlayer) { this.Explodes(); }
    }

    private void PlayerWinsInterface(Ice ice)
    {
        ice.LosesInterface();
        score.value += ice.PointsValue;
    }

    public void Explodes()
    {
        explosionParticle.Play();
        playerAudio.PlayOneShot(explodeSound, 1.0f);

        playerRenderer.enabled = false;

        foreach (Collider collider in GetComponents<Collider>())
        {
            collider.enabled = false;
        }

        isGameOver.value = true;
    }

    public bool IsAnIceBreakerSelected()
    {
        // If the current icebreaker index is outside the deck array,
        // there is no icebreaker selected
        return currentIceBreaker >= 0 && currentIceBreaker < deck.Length;
    }
}
