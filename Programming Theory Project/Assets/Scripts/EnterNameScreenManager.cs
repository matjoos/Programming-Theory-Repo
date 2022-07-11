using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class EnterNameScreenManager : MonoBehaviour
{
    [SerializeField] private TMP_InputField inputField;
    [SerializeField] private HighscoreArray highscores;
    [SerializeField] private IntVariable currentScore;

    private string playerName;

    private void Start()
    {
        inputField.Select();
        inputField.ActivateInputField();
    }

    public void OnEndEdit(string inputName)
    {
        playerName = inputName;
  
        AddScoreToHighscoreTable();

        SaveSystem.SaveHighscores(highscores);

        SceneManager.LoadScene("highscore");
    }

    public void AddScoreToHighscoreTable()
    {
        string localPlayerName = playerName;
        int localScore = currentScore.value;
        string playerToBump;
        int scoreToBump;

        for (int i = 0; i < highscores.value.Length; i++)
        {
            // Check if the player scored better than the highscores
            if (localScore > highscores.value[i].score)
            {
                // Save the data of the surpassed player in variables
                playerToBump = highscores.value[i].playerName;
                scoreToBump = highscores.value[i].score;

                // Add score to the highscore table
                highscores.value[i].playerName = localPlayerName;
                highscores.value[i].score = localScore;

                // Put surpassed player in existing variables to bump down the list
                localPlayerName = playerToBump;
                localScore = scoreToBump;
            }
        }
    }
}
