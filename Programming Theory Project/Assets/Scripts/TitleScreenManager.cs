using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleScreenManager : MonoBehaviour
{
    [SerializeField] GameObject cursor;
    bool isCursorActive = false;
    readonly float cursorBlinkTime = 0.5f; //seconds
    readonly float showHighscoreTime = 10.0f; //seconds

    void Start()
    {
        InvokeRepeating("BlinkCursor", cursorBlinkTime, cursorBlinkTime);
        Invoke("ShowHighscoreScreen", showHighscoreTime);
    }

    void Update()
    {
        if (Input.anyKeyDown)
        {
            SceneManager.LoadScene("main");
        }
    }

    void BlinkCursor()
    {
        cursor.SetActive(isCursorActive);
        isCursorActive = !isCursorActive;
    }

    void ShowHighscoreScreen()
    {
        SceneManager.LoadScene("highscore");
    }
}
