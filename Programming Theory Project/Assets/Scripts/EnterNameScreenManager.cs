using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class EnterNameScreenManager : MonoBehaviour
{
    [SerializeField] private TMP_InputField inputField;

    private void Start()
    {
        inputField.Select();
        inputField.ActivateInputField();
    }
}
