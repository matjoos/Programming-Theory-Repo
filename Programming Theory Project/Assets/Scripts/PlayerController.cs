using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private MainUIManager mainUIManager;
    private Renderer playerRenderer;
    private Rigidbody playerRigidBody;

    [SerializeField] private ParticleSystem explosionParticle;

    private AudioSource playerAudio;
    [SerializeField] private AudioClip explodeSound;
    [SerializeField] private AudioClip creditSound;
    [SerializeField] private AudioClip switchBreakerSound;
    [SerializeField] private AudioClip notEnoughCreditsSound;
  
    public int credits;
    public int score;

    private float speed = 8.0f;
    private float xRange = 7.7f;
    private float zLowerBound = -3.7f;
    private float zUpperBound = 4.7f;

    private IceBreaker fracter;
    private IceBreaker codeBreaker;
    private IceBreaker killer;
    private IceBreaker[] deck = new IceBreaker[3];
    private int currentIceBreaker = -1;

    public class IceBreaker
    {
        // ENCAPSULATION
        public string Name { get; set; }
        public int Strength { get; set; }
        public int InterfaceCost { get; set; }
        public Color Color { get; set; }
    }

    private void Start()
    {
        mainUIManager = GameObject.Find("MainUIManager").GetComponent<MainUIManager>();
        playerRenderer = gameObject.GetComponent<Renderer>();
        playerRigidBody = gameObject.GetComponent<Rigidbody>();

        playerAudio = GetComponent<AudioSource>();

        // ABSTRACTION
        InitializeDeck();
    }

    private void FixedUpdate()
    {
        if (!GameManager.Instance.gameOver)
        {
            // ABSTRACTION
            MovePlayer();
        }
    }

    private void Update()
    { 
        // Switch to next icebreaker on fire button or spacebar
        if (Input.GetButtonDown("Fire1") || Input.GetKeyDown(KeyCode.Space))
        {
            if (!GameManager.Instance.gameOver)
            {
                // ABSTRACTION
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

        playerRenderer.material.SetColor("_Color", iceBreaker.Color);
        mainUIManager.UpdateIceBreakerText(iceBreaker.Name, iceBreaker.Strength, iceBreaker.Color);

        playerAudio.PlayOneShot(switchBreakerSound, 1.0f);
    }

    private void InitializeDeck()
    {
        fracter = new IceBreaker
        {
            Name = "Fracter",
            Strength = 0,
            InterfaceCost = 1,
            Color = Color.magenta
        };

        codeBreaker = new IceBreaker
        {
            Name = "CodeBreaker",
            Strength = 0,
            InterfaceCost = 1,
            Color = Color.yellow
        };

        killer = new IceBreaker
        {
            Name = "Killer",
            Strength = 0,
            InterfaceCost = 1,
            Color = Color.cyan
        };

        deck[0] = fracter;
        deck[1] = codeBreaker;
        deck[2] = killer;
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
        // Store a reference to the credit in game manager for reactivation
        GameManager.Instance.deactivatedPickups.Add(credit);

        credit.SetActive(false);
        credits++;
        mainUIManager.UpdateCredits();
        playerAudio.PlayOneShot(creditSound, 1.0f);
    }

    private void CollideWithIce(Ice ice)
    {
        InterfaceWithIce(ice);

        mainUIManager.UpdateCredits();
        mainUIManager.UpdateScore();

        // Update UI with icebreaker level
        // Nothing happens if no icebreaker is selected
        if (currentIceBreaker < 0 || currentIceBreaker > 2)
        {
            return;
        }

        IceBreaker iceBreaker = deck[currentIceBreaker];
        mainUIManager.UpdateIceBreakerText(iceBreaker.Name, iceBreaker.Strength, iceBreaker.Color);
    }

    private void CollideWithGoal(Goal goal)
    {    
        if (credits < goal.InterfaceCost)
        {
            goal.NotDefeated();
            return;
        }
        else
        {
            // Level cleared
            credits -= goal.InterfaceCost;
            score += goal.PointsValue;
            mainUIManager.UpdateCredits();
            mainUIManager.UpdateScore();

            goal.Explode();

            GameManager.Instance.PlayerFinishedGame();
        }
    }

    private void InterfaceWithIce(Ice ice)
    {
        // Player loses the interface if no icebreaker is selected
        if (currentIceBreaker < 0 || currentIceBreaker > 2)
        {
            ice.WinsInterface();
            return;
        }

        switch (ice.IceType)
        {
            case "Barrier":
                if (deck[currentIceBreaker].Name == "Fracter")
                {
                    MatchedInterface(ice);
                }
                else
                {
                    ice.WinsInterface();
                }
                break;

            case "CodeGate":
                if (deck[currentIceBreaker].Name == "CodeBreaker")
                {
                    MatchedInterface(ice);
                }
                else
                {
                    ice.WinsInterface();
                }
                break;

            case "Sentry":
                if (deck[currentIceBreaker].Name == "Killer")
                {
                    MatchedInterface(ice);
                }
                else
                {
                    ice.WinsInterface();
                }
                break;         
        }
    }

    private void MatchedInterface(Ice ice)
    {
        while (deck[currentIceBreaker].Strength < ice.Strength && credits != 0)
        {
            credits--;
            deck[currentIceBreaker].Strength++;
        }

        // While-loop passed: icebreaker strength is at ice strength level
        // and/or credits = 0. 
        // If the player has credits left over to pay the interface cost,
        // the player pays the cost and wins the interface.
        if (credits >= deck[currentIceBreaker].InterfaceCost)
        {
            credits -= deck[currentIceBreaker].InterfaceCost;
            this.WinsInterface(ice);
        }
        else
        {
            ice.WinsInterface();
        }
    }

    private void WinsInterface(Ice ice)
    {
        ice.LosesInterface();
        score += ice.PointsValue;
    }

    public void Explodes()
    {
        explosionParticle.Play();
        playerAudio.PlayOneShot(explodeSound, 1.0f);
        playerRenderer.enabled = false;
    }
}
