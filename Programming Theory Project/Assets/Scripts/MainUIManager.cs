using UnityEngine;
using TMPro;

public class MainUIManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private TextMeshProUGUI iceBreakerText;
    [SerializeField] private TextMeshProUGUI creditsText;

    [SerializeField] private IntVariable credits;
    [SerializeField] private IntVariable score;
    [SerializeField] private IceBreaker selectedIcebreaker;


    private void OnEnable()
    {
        PlayerController.OnScoreChanged.AddListener(UpdateScore);
        PlayerController.OnCreditsChanged.AddListener(UpdateCredits);
        PlayerController.OnIceBreakerChanged.AddListener(UpdateIceBreakerText);
    }

    private void OnDisable()
    {
        PlayerController.OnScoreChanged.RemoveListener(UpdateScore);
        PlayerController.OnCreditsChanged.RemoveListener(UpdateCredits);
        PlayerController.OnIceBreakerChanged.RemoveListener(UpdateIceBreakerText);
    }

    private void UpdateScore()
    {
        scoreText.text = "Score: " + score.value;
    }

    private void UpdateCredits()
    {
        creditsText.text = "Credits: " + credits.value;
    }

    private void UpdateIceBreakerText(IceBreaker iceBreaker)
    {
        iceBreakerText.text = iceBreaker.name + " (lvl. " + iceBreaker.strength + ")";
        iceBreakerText.color = iceBreaker.color;
    }
}
