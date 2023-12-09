using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;
using TMPro;
using StarterAssets;
using UnityEngine.InputSystem;
using Cinemachine;

public class PlayerData : MonoBehaviour
{
	PlayerObject pObj;
    [SerializeField] public TextMeshPro playerLabel;

    private void Awake() {
		pObj = GetComponent<PlayerObject>();
	}

    public void SetNickname(string nickname) {
        NetworkObject networkObject = GetComponent<NetworkObject>();
        playerLabel.text = nickname;
    }
}
