using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    MainUIManager mainUIManager;
    Renderer playerRenderer;
    Rigidbody playerRigidBody;

    float speed = 8.0f;
    float xRange = 7.7f;
    float zLowerBound = -3.7f;
    float zUpperBound = 4.7f;

    IceBreaker fracter;
    IceBreaker codeBreaker;
    IceBreaker killer;
    IceBreaker[] deck = new IceBreaker[3];
    int currentIceBreaker = -1;

    public class IceBreaker
    {
        public string Name { get; set; }
        public int Strength { get; set; }
        public int InterfaceCost { get; set; }
        public Color Color { get; set; }
    }

    void Start()
    {
        mainUIManager = GameObject.Find("MainUIManager").GetComponent<MainUIManager>();
        playerRenderer = gameObject.GetComponent<Renderer>();
        playerRigidBody = gameObject.GetComponent<Rigidbody>();

        InitializeDeck();
    }

    void FixedUpdate()
    {
        Vector2 input = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
        Vector3 direction = new Vector3(input.x, 0, input.y);
        
        playerRigidBody.AddForce(direction.normalized * speed);

        // Keep player inside the boundaries
        Vector3 position = transform.position;

        if (position.x < -xRange) { position.x = -xRange; }
        if (position.x > xRange) { position.x = xRange; }

        if (position.z < zLowerBound) { position.z = zLowerBound; }
        if (position.z > zUpperBound) { position.z = zUpperBound; }

        transform.position = position;
    }

    void Update()
    { 
        // Switch to next icebreaker on fire button or spacebar
        if (Input.GetButtonDown("Fire1") || Input.GetKeyDown(KeyCode.Space))
        {
            SwitchIceBreaker();
        }
    }

    void SwitchIceBreaker()
    {
        currentIceBreaker = (currentIceBreaker + 1) % 3;

        IceBreaker iceBreaker = deck[currentIceBreaker];

        playerRenderer.material.SetColor("_Color", iceBreaker.Color);
        mainUIManager.UpdateIceBreakerText(iceBreaker.Name, iceBreaker.Strength, iceBreaker.Color);
    }

    void InitializeDeck()
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
}
