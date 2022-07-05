using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MainUIManager : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI scoreText;
    [SerializeField] TextMeshProUGUI iceBreakerText;
    [SerializeField] TextMeshProUGUI creditsText;


    public void UpdateScore(int score)
    {
        scoreText.text = "Score: " + score;
    }

    public void UpdateIceBreakerText(string name, int strength, Color color)
    {
        iceBreakerText.text = name + "(" + strength + ")";
        iceBreakerText.color = color;
    }

    public void UpdateCredits(int credits)
    {
        creditsText.text = "Credits: " + credits;
    }

}
