using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;
using TMPro;
using StarterAssets;
using Cinemachine;

public class PlayerObject : NetworkBehaviour {

    public static PlayerObject Local { get; private set; }

    [field: Header("References"), SerializeField] public PlayerMovement Controller { get; private set; }

    [Networked(OnChanged = nameof(UpdatePlayerName))] public NetworkString<_32> PlayerName { get; set; }
    [SerializeField] public TextMeshPro playerLabel;

    [SerializeField] Transform followCameraRoot;

    [Networked] public PlayerRef Ref { get; set; }
	[Networked] public byte Index { get; set; }

    public void Server_Init(PlayerRef pRef, byte index)
	{
        Debug.Assert(Runner.IsServer);

		Ref = pRef;
		Index = index;
	}

    public override void Spawned() {
        base.Spawned();

        if (Object.HasStateAuthority) {
            PlayerRegistry.Server_Add(Runner, Object.InputAuthority, this);
        }

        if (Object.HasInputAuthority) {
            GetComponent<Animator>().enabled = true;
            GetComponent<CharacterController>().enabled = true;
            GetComponent<ThirdPersonController>().enabled = true;

            GameObject virtualCamera = GameObject.Find("PlayerFollowCamera");
            virtualCamera.GetComponent<CinemachineVirtualCamera>().Follow = followCameraRoot;

            Local = this;
            PlayerName = PlayerPrefs.GetString("Username");
        }
    }

    protected static void UpdatePlayerName(Changed<PlayerObject> changed) {
        changed.Behaviour.playerLabel.text = changed.Behaviour.PlayerName.ToString();
    }
}