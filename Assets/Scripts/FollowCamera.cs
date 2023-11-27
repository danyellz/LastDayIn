using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using Fusion;
using StarterAssets;
using FirstDayIn.Network;

public class FollowCamera : MonoBehaviour
{
    [SerializeField] Transform playerCameraRoot;

    private void Start() {
        NetworkObject characterObject = GetComponent<NetworkObject>();
        
        if (characterObject.HasInputAuthority) {
            GameObject virtualCamera = GameObject.Find("PlayerFollowCamera");
            virtualCamera.GetComponent<CinemachineVirtualCamera>().Follow = playerCameraRoot;
            GetComponent<ThirdPersonController>().enabled = true;
        }
    }
}
