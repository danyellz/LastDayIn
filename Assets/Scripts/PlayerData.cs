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
    [SerializeField] Transform followCameraRoot;

    private void Awake() {
		pObj = GetComponent<PlayerObject>();
	}

    public void SetNickname(string nickname) {
        NetworkObject networkObject = GetComponent<NetworkObject>();
        playerLabel.text = nickname;

        if (networkObject.HasInputAuthority) {
            GameObject virtualCamera = GameObject.Find("PlayerFollowCamera");
            virtualCamera.GetComponent<CinemachineVirtualCamera>().Follow = followCameraRoot;

            GetComponent<Animator>().enabled = true;
            GetComponent<CharacterController>().enabled = true;
            GetComponent<PlayerInput>().enabled = true;
            GetComponent<ThirdPersonController>().enabled = true;
        }
    }
}
