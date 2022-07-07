using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private MainUIManager mainUIManager;
    private Renderer playerRenderer;
    private Rigidbody playerRigidBody;

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

        InitializeDeck();
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
            SwitchIceBreaker();
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
            CollideWithIce(other.gameObject);
        }
        else if (other.CompareTag("Goal"))
        {
            CollideWithGoal(other.gameObject);
        }
    }

    private void CollideWithCredit(GameObject credit)
    {
        // Store a reference to the credit in game manager for reactivation
        GameManager.Instance.deactivatedPickups.Add(credit);

        credit.SetActive(false);
        credits++;
        mainUIManager.UpdateCredits();
        // Make a sound credit++
    }

    private void CollideWithIce(GameObject ice)
    {
        InterfaceWithIce(ice.GetComponent<Ice>());

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

    private void CollideWithGoal(GameObject goal)
    {
        int interfaceCost = goal.GetComponent<Goal>().InterfaceCost;
        int points = goal.GetComponent<Goal>().PointsValue;

        if (credits < interfaceCost)
        {
            // Not enough credits for goal
            // Make a sound not enough credits
            return;
        }
        else
        {
            // Level cleared
            credits -= interfaceCost;
            score += points;
            mainUIManager.UpdateCredits();
            mainUIManager.UpdateScore();
            // Make a sound level cleared
            // Special effect
            // Wait and end scene
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

        // While-loop passed: icebreaker strength is at ice strength level. 
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
        Destroy(ice.gameObject);
        score += ice.PointsValue;
        // Make a sound ice destroyed
    }

    public void Explodes()
    {
        Destroy(gameObject);
        // Add particle effect
        // Make an explosion sound
    }
}
