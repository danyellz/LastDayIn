using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;

public class PlayerObject : NetworkBehaviour {

    public static PlayerObject Local { get; private set; }

    [field: Header("References"), SerializeField] public PlayerMovement Controller { get; private set; }

    [Networked] public PlayerRef Ref { get; set; }
	[Networked] public byte Index { get; set; }

    private void Start() {
        Debug.Log("PlayerObject Start()");

		if (Object.HasStateAuthority)
		{
			PlayerRegistry.Server_Add(Runner, Object.InputAuthority, this);
		}

		if (Object.HasInputAuthority)
		{
			Local = this;
		}
	}

    public void Server_Init(PlayerRef pRef, byte index)
	{
        Debug.Log("PlayerObject Server_Init()");
		Debug.Assert(Runner.IsServer);

		Ref = pRef;
		Index = index;
	}
}