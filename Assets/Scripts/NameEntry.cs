using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using FirstDayIn.Network;

public class NameEntry : MonoBehaviour
{
    [SerializeField] GameObject canvas;
    [SerializeField] TMP_InputField nameInputField;
    [SerializeField] Button submitButton;

    public void ActivateButton() {
        submitButton.interactable = true;
    }

    public void SubmitName() {
        Debug.Log("SubmitName " + nameInputField.text);
        FusionConnection.instance.ConnectToLobby(nameInputField.text);
        canvas.SetActive(false);
    }
}
