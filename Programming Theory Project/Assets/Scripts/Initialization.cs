using UnityEngine;

public class Initialization : MonoBehaviour
{
    [SerializeField] private HighscoreArray highscores;
    [SerializeField] private IntVariable score;
    [SerializeField] private IntVariable credits;
    [SerializeField] private BoolVariable isGameOver;

    void Start()
    {
        // Load highscores
        highscores.value = SaveSystem.LoadHighscores()?.highscores;

        // Initialize score and credits
        score.value = 0;
        credits.value = 0;

        // Initialize Game Over state
        isGameOver.value = false;
    }
}
