using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private MainUIManager mainUIManager;

    private Renderer playerRenderer;
    private Rigidbody playerRigidBody;

    [SerializeField] private ParticleSystem explosionParticle;

    private AudioSource playerAudio;
    [SerializeField] private AudioClip creditSound, notEnoughCreditsSound;
    [SerializeField] private AudioClip explodeSound, switchBreakerSound;

    [SerializeField] private IntVariable credits, score;
    [SerializeField] private GameObjectListVariable deactivatedCredits;

    [SerializeField] private IceBreaker fracter, codeBreaker, killer;

    private float speed = 8.0f;
    private float xRange = 7.7f;
    private float zLowerBound = -3.7f;
    private float zUpperBound = 4.7f;

    private IceBreaker[] deck = new IceBreaker[3];
    private int currentIceBreaker = -1;

    private void Start()
    {
        mainUIManager = GameObject.Find("MainUIManager").GetComponent<MainUIManager>();
        playerRenderer = gameObject.GetComponent<Renderer>();
        playerRigidBody = gameObject.GetComponent<Rigidbody>();

        playerAudio = GetComponent<AudioSource>();

        deck[0] = fracter;
        deck[1] = codeBreaker;
        deck[2] = killer;
    }

    private void FixedUpdate()
    {
        if (!GameManager.Instance.gameOver)
        {
            MovePlayer();
        }
    }

    private void Update()
    { 
        // Switch to next icebreaker on fire button or spacebar
        if (Input.GetButtonDown("Fire1") || Input.GetKeyDown(KeyCode.Space))
        {
            if (!GameManager.Instance.gameOver) // TODO Replace with SO boolean/event?
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

        IceBreaker iceBreaker = deck[currentIceBreaker];

        playerRenderer.material.SetColor("_Color", iceBreaker.color);

        // TODO SO as channel for UI
        mainUIManager.UpdateIceBreakerText(iceBreaker.name, iceBreaker.strength, iceBreaker.color);

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

        // TODO Event for UIManager. PlayerController does nothing, UI updates when
        // value changes
        mainUIManager.UpdateCredits();
        playerAudio.PlayOneShot(creditSound, 1.0f);
    }

    private void CollideWithIce(Ice ice)
    {
        InterfaceWithIce(ice);

        // TODO Have UI monitor value changes
        mainUIManager.UpdateCredits();
        mainUIManager.UpdateScore();

        // TODO UI listens for when to update 

        // Update UI with icebreaker level
        // Nothing happens if no icebreaker is selected
        if (currentIceBreaker < 0 || currentIceBreaker > 2)
        {
            return;
        }

        IceBreaker iceBreaker = deck[currentIceBreaker];
        mainUIManager.UpdateIceBreakerText(iceBreaker.name, iceBreaker.strength, iceBreaker.color);
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

            // TODO Remove method calls, have UI update on change
            mainUIManager.UpdateCredits();
            mainUIManager.UpdateScore();

            goal.Explode();

            // TODO Replace with event
            GameManager.Instance.PlayerFinishedGame();
        }
    }

    private void InterfaceWithIce(Ice ice)
    {
        // Player loses the interface if no icebreaker is selected
        // TODO Abstraction
        if (currentIceBreaker < 0 || currentIceBreaker > 2)
        {
            IceWinsInterface(ice);
            return;
        }

        switch (ice.IceType)
        {
            case "Barrier":
                if (deck[currentIceBreaker].name == "Fracter")
                {
                    MatchedInterface(ice);
                }
                else
                {
                    IceWinsInterface(ice);
                    
                }
                break;

            case "CodeGate":
                if (deck[currentIceBreaker].name == "CodeBreaker")
                {
                    MatchedInterface(ice);
                }
                else
                {
                    IceWinsInterface(ice);
                }
                break;

            case "Sentry":
                if (deck[currentIceBreaker].name == "Killer")
                {
                    MatchedInterface(ice);
                }
                else
                {
                    IceWinsInterface(ice);
                }
                break;         
        }
    }

    private void MatchedInterface(Ice ice)
    {
        while (deck[currentIceBreaker].strength < ice.Strength && credits.value != 0)
        {
            credits.value--;
            deck[currentIceBreaker].strength++;
        }

        // While-loop passed: icebreaker strength is at ice strength level
        // and/or credits = 0. 
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

        // TODO Replace with event or SO bool
        GameManager.Instance.GameOver();
    }
}
