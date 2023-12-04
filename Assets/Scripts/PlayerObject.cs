using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;
using TMPro;
using StarterAssets;
using UnityEngine.InputSystem;
using Cinemachine;

public class PlayerObject : NetworkBehaviour {

    public static PlayerObject Local { get; private set; }

    [field: Header("References"), SerializeField] public PlayerMovement Controller { get; private set; }

    [Networked(OnChanged = nameof(UpdatePlayerName))] public NetworkString<_16> PlayerName { get; set; }

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
            GetComponent<NetworkTransform>().enabled = true;
            PlayerRegistry.Server_Add(Runner, Object.InputAuthority, this);
        }

        if (Object.HasInputAuthority) {
            Local = this;
            Rpc_SetNickname(PlayerPrefs.GetString("Username"));
        }
    }

    [Rpc(RpcSources.InputAuthority, RpcTargets.StateAuthority)] void Rpc_SetNickname(string name) {
		PlayerName = name;
	}

    protected static void UpdatePlayerName(Changed<PlayerObject> changed) {
        changed.Behaviour.GetComponent<PlayerData>().SetNickname(changed.Behaviour.PlayerName.Value);
    }
}