using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleScreenManager : MonoBehaviour
{
    [SerializeField] private GameObject cursor;
    private bool isCursorActive = false;
    private readonly float cursorBlinkTime = 0.5f; //seconds
    private readonly float showHighscoreTime = 10.0f; //seconds

    private void Start()
    {
        InvokeRepeating("BlinkCursor", cursorBlinkTime, cursorBlinkTime);
        Invoke("ShowHighscoreScreen", showHighscoreTime);
    }

    private void Update()
    {
        if (Input.anyKeyDown)
        {
            SceneManager.LoadSceneAsync("main");
        }
    }

    private void BlinkCursor()
    {
        cursor.SetActive(isCursorActive);
        isCursorActive = !isCursorActive;
    }

    private void ShowHighscoreScreen()
    {
        SceneManager.LoadSceneAsync("highscore");
    }
}
