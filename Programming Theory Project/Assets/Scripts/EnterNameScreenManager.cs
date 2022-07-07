using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class EnterNameScreenManager : MonoBehaviour
{
    [SerializeField] private TMP_InputField inputField;

    private void Start()
    {
        inputField.Select();
        inputField.ActivateInputField();
    }

    public void OnEndEdit(string name)
    {
        HighscoreManager.instance.playerName = name;

        HighscoreManager.instance.AddScoreToHighscoreTable();

        HighscoreManager.instance.SaveHighscore();
  
        SceneManager.LoadScene("highscore");
    }

    
}
