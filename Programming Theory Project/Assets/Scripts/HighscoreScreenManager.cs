using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class HighscoreScreenManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI[] nameTable = new TextMeshProUGUI[5];
    [SerializeField] private TextMeshProUGUI[] scoreTable = new TextMeshProUGUI[5];

    [SerializeField] private HighscoreArray highscores;

    private float returnToTitleTime = 5.0f; //seconds

    private void Start()
    {
        // Display highscores
        for (int i = 0; i < nameTable.Length; i++)
        {
            nameTable[i].SetText(highscores.value[i].playerName);
            scoreTable[i].SetText(highscores.value[i].score.ToString());
        }

        // Return to title screen after X seconds
        Invoke("ReturnToTitleScreen", returnToTitleTime);
    }

    private void Update()
    {
        if (Input.anyKeyDown)
        {
            ReturnToTitleScreen();
        }
    }

    private void ReturnToTitleScreen()
    {
        SceneManager.LoadSceneAsync("titlescreen");
    }
}
