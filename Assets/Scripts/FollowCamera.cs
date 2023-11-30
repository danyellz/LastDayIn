using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using Fusion;
using StarterAssets;

public class FollowCamera : NetworkBehaviour
{
    [SerializeField] Transform playerCameraRoot;

    public void Start() {
        // base.Spawned();
        
        Debug.Log($"FollowCamera Start() {Object}");

        // NetworkObject networkObject = GetComponent<NetworkObject>();
        
        // if (Runner.LocalPlayer == Object.InputAuthority) {
            GameObject virtualCamera = GameObject.Find("PlayerFollowCamera");
            virtualCamera.GetComponent<CinemachineVirtualCamera>().Follow = playerCameraRoot;

            GetComponent<Animator>().enabled = true;
            GetComponent<CharacterController>().enabled = true;
            GetComponent<ThirdPersonController>().enabled = true;
        // }
    }
}
