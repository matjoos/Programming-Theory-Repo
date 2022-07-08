using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [SerializeField] private IntVariable score;
   
    public List<GameObject> deactivatedPickups;

    [SerializeField] private GameObject pauseTextObject;
    [SerializeField] private GameObject gameOverTextObject;
    [SerializeField] private GameObject thankYouTextObject;

    private int pauseTimeScale = 0;
    public bool gameOver = false;
    private bool gameOverDone = false;
    private float gameOverTime = 3.0f; //seconds

    private void Start()
    {
        Instance = this;
    }

    private void Update()
    {
        if (!gameOver && Input.GetKeyDown(KeyCode.P))
        {
            PauseOrUnpauseGame();
        }

        if (gameOver && gameOverDone && Input.anyKeyDown)
        {
            CheckForHighscoreAndChangeScene();
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
        GameObject.Find("Main Camera").GetComponent<AudioSource>().Stop();

        gameOver = true;
        gameOverTextObject.SetActive(true);

        Invoke("WaitForGameOverDone", gameOverTime);
    }

    private void WaitForGameOverDone()
    {
        gameOverDone = true;
    }

    private void CheckForHighscoreAndChangeScene()
    {
        // The player made the list if the score is higher
        // than the lowest one on the list.
        HighscoreManager.Highscore[] highscores = HighscoreManager.instance.highscores;

        int lowestPosition = highscores.Length - 1;
        int lowestHighscore = highscores[lowestPosition].score;

        if (score.value > lowestHighscore)
        {
            SceneManager.LoadSceneAsync("entername");
        }
        else
        {
            SceneManager.LoadSceneAsync("highscore");
        }      
    }

    public void PlayerFinishedGame()
    {
        thankYouTextObject.SetActive(true);
        GameOver();
    }
}
