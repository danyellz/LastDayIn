using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;
// using Fusion.KCC;
// using Helpers.Physics;
// using Helpers.Bits;
// using System.Linq;

[OrderBefore(typeof(NetworkTransformAnchor))]
public class PlayerMovement : NetworkBehaviour
{
	public static PlayerMovement Local { get; protected set; }

    [Networked] public bool IsSuspect { get; set; }

    [Networked(OnChanged = nameof(OnDeadChanged))] public bool IsDead { get; set; }

    // public override void Spawned() {
    //   Debug.Log("PlayerMovement Spawned()");

	// 	  if (Object.HasInputAuthority) {
	// 		  Local = this;
	// 	  }
    // }

    public void EndInteraction() {
        
    }

    public void Server_UpdateDeadState() {

	}

    static void OnDeadChanged(Changed<PlayerMovement> changed) {
		  PlayerMovement self = changed.Behaviour;
    }
}
