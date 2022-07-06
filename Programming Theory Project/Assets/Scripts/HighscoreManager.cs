using System;
using System.IO;
using UnityEngine;

public class HighscoreManager : MonoBehaviour
{
    public static HighscoreManager instance;
    public Highscore[] highscores;
    public string playerName;
    private int numberOfScores = 5;

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
}
