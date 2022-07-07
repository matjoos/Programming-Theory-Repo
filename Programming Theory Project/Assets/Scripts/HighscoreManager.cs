using System;
using System.IO;
using UnityEngine;

public class HighscoreManager : MonoBehaviour
{
    public static HighscoreManager instance;
    public Highscore[] highscores;
    public string playerName;
    private int numberOfScores = 5;
    public int currentScore;

    [Serializable]
    public class Highscore
    {
        public string name;
        public int score;
    }

    [Serializable]
    private class SaveData
    {
        public Highscore[] highscores;
    }

    private void Awake()
    {
        if (instance == null)
        {
            //Initialize highscore table
            highscores = new Highscore[numberOfScores];

            for (int i = 0; i < highscores.Length; i++)
            {
                highscores[i] = new Highscore();
            }

            // Create persistent singleton
            instance = this;
            DontDestroyOnLoad(gameObject);

            //Load highscores
            LoadHighscore();
        }
    }

    public void SaveHighscore()
    {
        SaveData saveData = new SaveData();
        saveData.highscores = highscores;

        string json = JsonUtility.ToJson(saveData);

        File.WriteAllText(Application.persistentDataPath + "/highscore.json", json);
    }

    public void LoadHighscore()
    {
        string filePath = Application.persistentDataPath + "/highscore.json";

        if (File.Exists(filePath))
        {
            string json = File.ReadAllText(filePath);

            SaveData saveData = JsonUtility.FromJson<SaveData>(json);

            if (saveData.highscores != null)
            {
                highscores = saveData.highscores;
            }
        }
    }

    public void AddScoreToHighscoreTable()
    {
        string localPlayerName = playerName;
        int localScore = currentScore;
        string playerToBump;
        int scoreToBump;

        for (int i = 0; i < highscores.Length; i++)
        {
            // Check if the player scored better than the highscores
            if (localScore > highscores[i].score)
            {
                // Save the data of the surpassed player in variables
                playerToBump = highscores[i].name;
                scoreToBump = highscores[i].score;

                // Add score to the highscore table
                highscores[i].name = localPlayerName;
                highscores[i].score = localScore;

                // Put surpassed player in existing variables to bump down the list
                localPlayerName = playerToBump;
                localScore = scoreToBump;
            }
        }
    }
}
