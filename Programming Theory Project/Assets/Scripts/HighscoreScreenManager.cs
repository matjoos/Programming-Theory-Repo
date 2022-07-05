using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class HighscoreScreenManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI[] nameTable = new TextMeshProUGUI[5];
    [SerializeField] private TextMeshProUGUI[] scoreTable = new TextMeshProUGUI[5];

    readonly float returnToTitleTime = 5.0f; //seconds

    void Start()
    {
        // Print highscores
        for (int i = 0; i < nameTable.Length; i++)
        {
            nameTable[i].SetText(HighscoreManager.instance.highscores[i].name);
            scoreTable[i].SetText("" + HighscoreManager.instance.highscores[i].score);
        }

        // Return to title screen after X seconds
        Invoke("ReturnToTitleScreen", returnToTitleTime);
    }

    void Update()
    {
        if (Input.anyKeyDown)
        {
            ReturnToTitleScreen();
        }
    }

    void ReturnToTitleScreen()
    {
        SceneManager.LoadScene("titlescreen");
    }
}
