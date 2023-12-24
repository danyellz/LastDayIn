using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;
using Unity.VisualScripting;
using UnityEngine.InputSystem;
using Cinemachine;
using StarterAssets;

[OrderBefore(typeof(NetworkTransformAnchor))]
public class PlayerMovement : NetworkBehaviour
{
	public static PlayerMovement Local { get; protected set; }

    [SerializeField] Transform followCameraRoot;

    [Networked] public bool IsSuspect { get; set; }

    [Networked(OnChanged = nameof(OnDeadChanged))] public bool IsDead { get; set; }

    [Networked(OnChanged = nameof(OnLightingChanged))] public bool IsLightingUpdated { get; set; }

    [Networked(OnChanged = nameof(OnReadyForSpawn))] public bool IsReadyForSpawn { get; set; }

    public override void Spawned() {
      Debug.Log("PlayerMovement Spawned()");

	    if (Object.HasInputAuthority) {
		    Local = this;
	    }

        NetworkObject playerObject = GetComponent<NetworkObject>();
        IsReadyForSpawn = playerObject.HasInputAuthority;
    }

    public void EndInteraction() {
        
    }

    public void Server_UpdateDeadState() {

	}

    static void OnDeadChanged(Changed<PlayerMovement> changed) {
		  PlayerMovement self = changed.Behaviour;
    }

    static void OnLightingChanged(Changed<PlayerMovement> changed) {
        foreach (GameObject light in GameObject.FindGameObjectsWithTag("Light")) {
			light.GetComponent<Light>().color = Color.red;
		}
    }

    static void OnReadyForSpawn(Changed<PlayerMovement> changed) {
        GameObject virtualCamera = GameObject.FindGameObjectWithTag("FollowCam");
        virtualCamera.GetComponent<CinemachineVirtualCamera>().Follow = changed.Behaviour.followCameraRoot;

        changed.Behaviour.Object.GetComponent<NetworkRigidbody>().enabled = changed.Behaviour.IsReadyForSpawn;
        changed.Behaviour.Object.GetComponent<CharacterController>().enabled = changed.Behaviour.IsReadyForSpawn;
        changed.Behaviour.Object.GetComponent<PlayerInput>().enabled = changed.Behaviour.IsReadyForSpawn;
        changed.Behaviour.Object.GetComponent<ThirdPersonController>().enabled = changed.Behaviour.IsReadyForSpawn;
    }
}
