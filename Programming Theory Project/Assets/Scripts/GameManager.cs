using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField] private IntVariable score;
    [SerializeField] private HighscoreArray highscores;
   
    [SerializeField] private GameObject pauseTextObject;
    [SerializeField] private GameObject gameOverTextObject;

    [SerializeField] private AudioSource backgroundMusic;

    [SerializeField] private BoolVariable isGameOver;

    private int pauseTimeScale = 0;
    private bool isGameOverAnimationDone = false;
    private float gameOverTime = 3.0f; //seconds

    private void Update()
    {
        if (isGameOver.value == false && Input.GetKeyDown(KeyCode.P))
        {
            PauseOrUnpauseGame();
        }

        if (isGameOverAnimationDone && Input.anyKeyDown)
        {
            BranchOnScore();
        }

        if (isGameOver.value == true)
        {
            GameOver();
        }
    }

    private void PauseOrUnpauseGame()
    {
        Time.timeScale = pauseTimeScale;
        _ = pauseTimeScale == 0 ? pauseTimeScale = 1 : pauseTimeScale = 0;
        pauseTextObject.SetActive(!pauseTextObject.activeInHierarchy);
    }

    public void GameOver()
    {
        backgroundMusic.Stop();

        gameOverTextObject.SetActive(true);

        Invoke(nameof(WaitForGameOverDone), gameOverTime);
    }

    private void WaitForGameOverDone()
    {
        isGameOverAnimationDone = true;
    }

    private void BranchOnScore()
    {
        // The player made the highscore list and needs to enter a name
        // if the score is higher than the lowest one on the list.
        int lowestPosition = highscores.value.Length - 1;
        int lowestHighscore = highscores.value[lowestPosition].score;

        if (score.value > lowestHighscore)
        {
            SceneManager.LoadSceneAsync("entername");
        }
        else
        {
            SceneManager.LoadSceneAsync("highscore");
        }      
    }
}
