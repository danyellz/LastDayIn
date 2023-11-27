using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using FirstDayIn.Network;

public class SessionNameEntry : MonoBehaviour
{
    [SerializeField] GameObject canvas;
    [SerializeField] TMP_InputField sessionInputField;
    [SerializeField] Button joinButton;

    public void ActivateButton() {
        joinButton.interactable = true;
    }

    public void CreateSession() {
        Debug.Log("SessionNameEntry CreateSession()" + sessionInputField.text);
        GameManager.instance.ConnectToSession(sessionInputField.text);
        canvas.SetActive(false);
    }

    public void JoinSession() {
        Debug.Log("SessionNameEntry JoinSession()" + sessionInputField.text);
        GameManager.instance.ConnectToSession(sessionInputField.text);
        canvas.SetActive(false);
    }
}