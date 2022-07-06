using UnityEngine;
using TMPro;

public class MainUIManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private TextMeshProUGUI iceBreakerText;
    [SerializeField] private TextMeshProUGUI creditsText;

    private PlayerController playerController;

    private void Start()
    {
        playerController = GameObject.Find("Player").GetComponent<PlayerController>();
    }

    public void UpdateScore()
    {
        scoreText.text = "Score: " + playerController.score;
    }

    public void UpdateIceBreakerText(string name, int strength, Color color)
    {
        iceBreakerText.text = name + " (lvl. " + strength + ")";
        iceBreakerText.color = color;
    }

    public void UpdateCredits()
    {
        creditsText.text = "Credits: " + playerController.credits;
    }
}
