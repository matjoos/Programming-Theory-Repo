using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cursor : MonoBehaviour
{
    [SerializeField] private GameObject cursor;
    private bool isActive = false;
    private readonly float cursorBlinkTime = 0.5f; //seconds

    private void Start()
    {
        InvokeRepeating("Blink", cursorBlinkTime, cursorBlinkTime);
    }

    private void Blink()
    {
        cursor.SetActive(isActive);
        isActive = !isActive;
    }
}
