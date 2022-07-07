using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    private PlayerController playerController;

    private readonly float creditSpawnDelay = 3.0f; //seconds
    private bool isSpawning = false;

    // Start is called before the first frame update
    private void Start()
    {
        playerController = GameObject.Find("Player").GetComponent<PlayerController>();
    }

    // Update is called once per frame
    private void Update()
    {
        //Respawn credits if there are no more in the game
        GameObject[] credits = GameObject.FindGameObjectsWithTag("Credit");

        if (playerController.credits == 0 && credits.Length == 0 && !isSpawning)
        {
            StartCoroutine(WaitAndSpawnCredit(credits));
        }
    }

    private IEnumerator WaitAndSpawnCredit(GameObject[] credits)
    {
        isSpawning = true;
        yield return new WaitForSeconds(creditSpawnDelay);

        foreach (GameObject credit in GameManager.Instance.deactivatedPickups)
        {
            credit.SetActive(true);
        }

        // All credits reactivated, clear list
        GameManager.Instance.deactivatedPickups = new List<GameObject>();

        isSpawning = false;
    }
}
