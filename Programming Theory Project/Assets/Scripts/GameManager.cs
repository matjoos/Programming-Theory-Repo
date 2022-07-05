using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    int pauseTimeScale = 0;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            PauseOrUnpauseGame();
        }
    }

    void PauseOrUnpauseGame()
    {
        Time.timeScale = pauseTimeScale;
        _ = pauseTimeScale == 0 ? pauseTimeScale = 1 : pauseTimeScale = 0;
    }
}
