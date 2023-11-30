using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using Fusion;
using StarterAssets;

public class FollowCamera : NetworkBehaviour
{
    [SerializeField] Transform playerCameraRoot;

    public override void Spawned() {
        base.Spawned();
        
        Debug.Log("FollowCamera Start()");
        NetworkObject characterObject = GetComponent<NetworkObject>();
        
        if (characterObject.HasInputAuthority) {
            GameObject virtualCamera = GameObject.Find("PlayerFollowCamera");
            virtualCamera.GetComponent<CinemachineVirtualCamera>().Follow = playerCameraRoot;

            GetComponent<Animator>().enabled = true;
            GetComponent<ThirdPersonController>().enabled = true;
        }
    }
}
