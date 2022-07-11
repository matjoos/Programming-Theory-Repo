using System;

[Serializable]
public class SaveData
{
    public Highscore[] highscores;

    public SaveData(HighscoreArray activeHighscores)
    {
        highscores = activeHighscores.value;
    }
}
