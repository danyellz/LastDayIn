using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Fusion;
using UnityEngine;
using FirstDayIn.Network;

public class GameState : NetworkBehaviour
{
	public enum EGameState { Off, Pregame, Play, Meeting, VoteResults, CrewWin, ImpostorWin }

    [Networked] public EGameState Previous { get; set; }
	[Networked] public EGameState Current { get; set; }

    protected StateMachine<EGameState> StateMachine = new StateMachine<EGameState>();

    public void Spawned() {
        Debug.Log("GameState Awake()");

        StateMachine[EGameState.Off].onExit = newState =>
		{
			Debug.Log($"Exited {EGameState.Off} to {newState}");

			if (FusionConnection.instance.runner.IsServer) { }

			if (FusionConnection.instance.runner.IsPlayer) // [PLAYER] Off -> *
			{
				// GameManager.im.gameUI.InitPregame(Runner);
			}
		};

		StateMachine[EGameState.Pregame].onEnter = state =>
		{
			Debug.Log($"Entered {EGameState.Pregame} from {state}");
		};

		StateMachine[EGameState.Play].onEnter = state =>
		{
			Debug.Log($"Entered {EGameState.Play} from {state}");
		};

		StateMachine[EGameState.Meeting].onEnter = state =>
		{
			Debug.Log($"Entered {EGameState.Meeting} from {state}");
		};

		StateMachine[EGameState.Meeting].onExit = newState =>
		{
			Debug.Log($"Exited {EGameState.Meeting} to {newState}");
		};

		StateMachine[EGameState.VoteResults].onEnter = state =>
		{
            Debug.Log($"Entered {EGameState.VoteResults} from {state}");
		};

		StateMachine[EGameState.CrewWin].onEnter = state =>
		{
			Debug.Log($"Entered {EGameState.CrewWin} from {state}");
		};

		StateMachine[EGameState.CrewWin].onExit = newState => 
        {
            Debug.Log($"Exited {EGameState.CrewWin} to {newState}");
        };

		StateMachine[EGameState.ImpostorWin].onEnter = state =>
		{
			Debug.Log($"Entered {EGameState.ImpostorWin} from {state}");
		};

		StateMachine[EGameState.ImpostorWin].onExit = newState => {
            Debug.Log($"Exited {EGameState.ImpostorWin} to {newState}");
        };
    }

    public void Server_SetState(EGameState st)
	{
		if (Current == st) return;
		Previous = Current;
		Current = st;
	}
}
