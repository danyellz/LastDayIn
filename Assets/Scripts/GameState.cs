using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Fusion;
using UnityEngine;
using TMPro;
using static PlayerRegistry;
using FirstDayIn.Network;

public class GameState : NetworkBehaviour
{
	public enum EGameState { Off, Pregame, Play, Meeting, VoteResults, CrewWin, ImpostorWin }

    public static GameState Instance;

    [Networked] public EGameState Previous { get; set; }
	[Networked] public EGameState Current { get; set; }

    [Networked] TickTimer Delay { get; set; }
	[Networked] EGameState DelayedState { get; set; }

    protected StateMachine<EGameState> StateMachine = new StateMachine<EGameState>();

	public override void Spawned() {
		Debug.Log("GameState Spawned()");

		 Instance = this;

        StateMachine[EGameState.Off].onExit = newState =>
		{
			Debug.Log($"Exited {EGameState.Off} to {newState}");

			if (GameManager.instance._runner.IsPlayer) // [PLAYER] Off -> *
			{
				// GameManager.im.gameUI.InitPregame(Runner);
			}
		};

		StateMachine[EGameState.Pregame].onEnter = state =>
		{
			Debug.Log($"Entered {EGameState.Pregame} from {state}");

            if (Runner.IsServer) { // [SERVER] * -> Pregame
                PlayerRegistry.ForEach(pObj => {
                    pObj.Controller.IsDead = false;
                    pObj.Controller.IsSuspect = false;
                    pObj.Controller.EndInteraction();
                    pObj.Controller.Server_UpdateDeadState();
                    Debug.Log($"pObj Updated {pObj}");
                });
            }

				// GameManager.rm.Purge();
		};

		StateMachine[EGameState.Play].onEnter = state =>
		{
			Debug.Log($"Entered {EGameState.Play} from {state}");

            if (Runner.IsServer) { // [SERVER] * -> Play 
                if (state == EGameState.Pregame) {
                    PlayerObject[] objs = PlayerRegistry.GetRandom(1);
                    foreach (PlayerObject p in objs) {
                        p.Controller.IsSuspect = true;
                        Debug.Log($"[SPOILER] {p.playerLabel.text} is suspect");
                    }
                }
            }

            Debug.Log($"PlayerCount {PlayerRegistry.Count}");
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


    public override void FixedUpdateNetwork() {
        if (Runner.IsServer) {
			if (Current == EGameState.Pregame) {
				SessionInfo sessionInfo = Runner.SessionInfo;
            	string playerCount = sessionInfo.PlayerCount.ToString();
            	GameObject pCountLabel = GameObject.Find("PlayerCountLabel");
            	pCountLabel.GetComponent<TextMeshProUGUI>().text = PlayerRegistry.Count + "/10";
			}
				
            if (Delay.Expired(Runner)) {
                    Delay = TickTimer.None;
                    Server_SetState(DelayedState);
            }
        }

        if (Runner.IsForward) {
			StateMachine.Update(Current, Previous);
        }
	}

    public void Server_SetState(EGameState st)
	{
        Debug.Log($"GameState Server_SetState() {st}");
		if (Current == st) return;
		Previous = Current;
		Current = st;
	}

    public void Server_DelaySetState(EGameState newState, float delay)
	{
		Debug.Log($"Delay state change to {newState} for {delay}s");
		Delay = TickTimer.CreateFromSeconds(Runner, delay);
		DelayedState = newState;
	}
}
