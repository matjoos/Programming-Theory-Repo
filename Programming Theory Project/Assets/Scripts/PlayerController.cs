using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] Rigidbody playerRb;
    float speed = 10.0f;
    float xRange = 8.0f;
    float zRange = 5.0f;

    void Start()
    {
       
    }

    void FixedUpdate()
    {
        Vector2 input = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
        Vector3 direction = new Vector3(input.x, 0, input.y);
        Vector3 newPosition = transform.position + direction * speed * Time.deltaTime;

        // Keep player inside the boundaries
        if (newPosition.x < -xRange) { newPosition.x = -xRange; }
        if (newPosition.x > xRange) { newPosition.x = xRange; }

        if (newPosition.z < -zRange) { newPosition.z = -zRange; }
        if (newPosition.z > zRange) { newPosition.z = zRange; }

        transform.position = newPosition;
    }

    void SwitchIceBreaker()
    {

    }
}
