using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Fusion;
using UnityEngine;
using FirstDayIn.Network;

public class GameState : NetworkBehaviour
{
	public enum EGameState { Off, Pregame, Play, Meeting, VoteResults, CrewWin, ImpostorWin }

    [Networked(OnChanged = nameof(Server_SetState))] public EGameState Previous { get; set; }
    [Networked(OnChanged = nameof(Server_SetState))] public EGameState Current { get; set; }

    [Networked] TickTimer Delay { get; set; }
	[Networked] EGameState DelayedState { get; set; }

    protected StateMachine<EGameState> StateMachine = new StateMachine<EGameState>();

    private void Start() {
        Debug.Log("GameState Spawned()");

        StateMachine[EGameState.Off].onExit = newState =>
		{
			Debug.Log($"Exited {EGameState.Off} to {newState}");

			if (GameManager.instance.runner.IsServer) { }

			if (GameManager.instance.runner.IsPlayer) // [PLAYER] Off -> *
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

    public void UpdateState() {
        if (Runner.IsForward) {
			StateMachine.Update(Current, Previous);
        }
    }

    public override void FixedUpdateNetwork() {
    //     Debug.Log("FixedUpdateNetwork()");

		// if (Runner.IsServer) {
		// 	if (Delay.Expired(Runner)) {
		// 		Delay = TickTimer.None;
		// 		// Server_SetState(DelayedState);
		// 	}
		// }

		if (Runner.IsForward) {
			StateMachine.Update(Current, Previous);
        }
	}

    public void Server_DelaySetState(EGameState newState, float delay)
	{
		Debug.Log($"Delay state change to {newState} for {delay}s");
		Delay = TickTimer.CreateFromSeconds(Runner, delay);
		DelayedState = newState;
	}

    protected static void Server_SetState(Changed<GameState> changed) {
        Debug.Log($"Server_SetState to {changed.Behaviour.Previous}");

        if (changed.Behaviour.Current == GameManager.instance._state) return;
		changed.Behaviour.Previous = changed.Behaviour.Current;
		changed.Behaviour.Current = GameManager.instance._state;
    }
}
