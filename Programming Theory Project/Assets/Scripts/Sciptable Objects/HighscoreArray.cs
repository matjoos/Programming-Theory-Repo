using System;
using UnityEngine;

[Serializable]
public class HighscoreArray : ScriptableObject
{
    public Highscore[] value = new Highscore[5];

    public HighscoreArray()
    {
        for (int i = 0; i < value.Length; i++)
        {
            value[i] = new Highscore();
        }
    }
}
