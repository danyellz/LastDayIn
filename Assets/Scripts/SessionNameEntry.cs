using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using static NetworkStartBridge;

public class SessionNameEntry : MonoBehaviour
{
    [SerializeField] GameObject canvas;
    [SerializeField] TMP_InputField sessionInputField;

    [SerializeField] Button createButton;
    [SerializeField] Button joinButton;

    public void Start() {
        createButton.interactable = false;
        joinButton.interactable = false;
        sessionInputField.onValueChanged.AddListener(delegate { InputFieldChanged(); });
    }

    public void CreateSession() {
        Debug.Log("SessionNameEntry CreateSession()" + sessionInputField.text);
        canvas.SetActive(false);
        NetworkStartBridge.Instance.CreateSession(sessionInputField.text);
    }

    public void JoinSession() {
        Debug.Log("SessionNameEntry JoinSession()" + sessionInputField.text);
        canvas.SetActive(false);
        NetworkStartBridge.Instance.ConnectToSession(sessionInputField.text);
    }

    public void InputFieldChanged() {
        if (sessionInputField.text == "") {
            createButton.interactable = false;
            joinButton.interactable = false;
        } else {
            createButton.interactable = true;
            joinButton.interactable = true;
        }
    }
}