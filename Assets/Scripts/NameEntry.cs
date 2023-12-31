using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class NameEntry : MonoBehaviour
{
    [SerializeField] GameObject canvas;

    [SerializeField] GameObject lobbyCanvas;
    [SerializeField] TMP_InputField nameInputField;
    [SerializeField] Button submitButton;

    public void ActivateButton() {
        submitButton.interactable = true;
    }

    public void SubmitName() {
        Debug.Log("SubmitName " + nameInputField.text);
        PlayerPrefs.SetString("Username", nameInputField.text);
        canvas.SetActive(false);
        lobbyCanvas.SetActive(true);
    }
}
