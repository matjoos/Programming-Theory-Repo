using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    private PlayerController playerController;
   
    public List<GameObject> deactivatedPickups;

    [SerializeField] private GameObject pauseTextObject;
    [SerializeField] private GameObject gameOverTextObject;

    private int pauseTimeScale = 0;
    public bool gameOver = false;
    private bool gameOverDone = false;
    private float gameOverTime = 2.0f; //seconds

    private void Start()
    {
        Instance = this;
        playerController = GameObject.Find("Player").GetComponent<PlayerController>();
    }

    private void Update()
    {
        if (!gameOver && Input.GetKeyDown(KeyCode.P))
        {
            PauseOrUnpauseGame();
        }

        if (gameOver && gameOverDone && Input.anyKeyDown)
        {
            CheckForHighscore();
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
        gameOver = true;
        gameOverTextObject.SetActive(true);
        StartCoroutine(WaitForGameOverDone());
    }

    private IEnumerator WaitForGameOverDone()
    {
        yield return new WaitForSeconds(gameOverTime);
        gameOverDone = true;
    }

    private void CheckForHighscore()
    {
        // The player made the list if the score is higher
        // than the lowest one on the list.
        HighscoreManager.Highscore[] highscores = HighscoreManager.instance.highscores;

        int lowestPosition = highscores.Length - 1;
        int lowestHighscore = highscores[lowestPosition].score;

        if (playerController.score > lowestHighscore)
        {
            // Made the list, enter name
            Debug.Log("Made the list, enter name");
        }
        else
        {
            SceneManager.LoadSceneAsync("highscore");
        }      
    }
}
