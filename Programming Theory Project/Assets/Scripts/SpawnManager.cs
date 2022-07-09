using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    private float creditSpawnDelay = 3.0f; //seconds
    private bool isSpawning = false;

    [SerializeField] private IntVariable credits;
    [SerializeField] private GameObjectListVariable deactivatedCredits;

    private void Update()
    {
        //Respawn credits if there are no more in the game
        GameObject[] creditsInGame = GameObject.FindGameObjectsWithTag("Credit");

        if (credits.value == 0 && creditsInGame.Length == 0 && !isSpawning)
        {
            isSpawning = true; 
            Invoke("SpawnCredits", creditSpawnDelay);
        }
    }

    private void SpawnCredits()
    {
        foreach (GameObject credit in deactivatedCredits.value)
        {
            credit.SetActive(true);
        }

        // All credits reactivated, clear list
        deactivatedCredits.value.Clear();

        isSpawning = false;
    }
}
