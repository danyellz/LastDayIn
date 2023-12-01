using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;
using TMPro;
using StarterAssets;
using FirstDayIn.Network;

public class PlayerObject : NetworkBehaviour {

    public static PlayerObject Local { get; private set; }

    [field: Header("References"), SerializeField] public PlayerMovement Controller { get; private set; }

    [Networked(OnChanged = nameof(UpdatePlayerName))] public NetworkString<_32> PlayerName { get; set; }
    [SerializeField] public TextMeshPro playerLabel;

    [Networked] public PlayerRef Ref { get; set; }
	[Networked] public byte Index { get; set; }

    public void Server_Init(PlayerRef pRef, byte index)
	{
        Debug.Assert(Runner.IsServer);

		Ref = pRef;
		Index = index;
	}

    public void Awake() {
        GetComponent<Animator>().enabled = true;
        GetComponent<CharacterController>().enabled = true;
        GetComponent<ThirdPersonController>().enabled = true;
    }

    public void Start() {
        PlayerName = PlayerPrefs.GetString("Username");
        Local = this;
    }

    public override void Spawned() {
        base.Spawned();

        Debug.Log("PlayerObject Spawned()");

        if (Runner.IsServer) {
            PlayerRegistry.Server_Add(Runner, Object.InputAuthority, this);
        }
    }

    protected static void UpdatePlayerName(Changed<PlayerObject> changed) {
        changed.Behaviour.playerLabel.text = changed.Behaviour.PlayerName.ToString();
    }
}