using UnityEngine;
using TMPro;

public class MainUIManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private TextMeshProUGUI iceBreakerText;
    [SerializeField] private TextMeshProUGUI creditsText;

    [SerializeField] private IntVariable credits;
    [SerializeField] private IntVariable score;

    // TODO Replace methods with events, update UI on change

    public void UpdateScore()
    {
        scoreText.text = "Score: " + score.value;
    }

    public void UpdateIceBreakerText(string name, int strength, Color color)
    {
        iceBreakerText.text = name + " (lvl. " + strength + ")";
        iceBreakerText.color = color;
    }

    public void UpdateCredits()
    {
        creditsText.text = "Credits: " + credits.value;
    }
}
