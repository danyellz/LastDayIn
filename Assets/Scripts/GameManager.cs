using UnityEngine;
using UnityEngine.UI;
using Fusion;
using Fusion.Sockets;
using System.Collections.Generic;
using System;
using TMPro;
using System.Linq;
using static GameState;

namespace FirstDayIn.Network {
    public class GameManager: Fusion.NetworkBehaviour, INetworkRunnerCallbacks
    {

        public static GameManager instance;

        public static GameState GameState { get; private set; }

        // private List<SessionInfo> _sessions = new List<SessionInfo>(); 

        [Header("HUD")]
        public GameObject hudCanvas;

        // public Transform topButtons;
        [SerializeField] public TMP_Text playerCountLabel;
        public Button startGameButton;

        // [Header("Join Session")]

        // [Header("SessionList")]
        // public Button refreshButton;
        // public Transform sessionListContent;
        // public GameObject sessionEntryPrefab;

        private void Awake() {
            if (instance == null) {
                Debug.Log("GameManager Awake()");

                GameState = GetComponent<GameState>();
                instance = this;
            }
        }

        public override void Spawned() {
            Debug.Log("GameManager Spawned()");
		    base.Spawned();

            if (Runner.IsServer) {
                GameState.Server_SetState(GameState.EGameState.Pregame);
                hudCanvas.SetActive(true);
            }

            Runner.AddCallbacks(this);
	    }

        public override void Despawned(NetworkRunner runner, bool hasState) {
            Debug.Log("GameManager Despawned()");
		    base.Despawned(runner, hasState);
		    runner.RemoveCallbacks(this);
	    }

        public void StartGame() {
            Debug.Log($"PlayerCount: {PlayerRegistry.Count}");
            hudCanvas.SetActive(false);
            GameState.Server_SetState(GameState.EGameState.Play);
        }


        // public void RefreshSessionListUI() {
        //      Debug.Log("RefreshSessionListUI" + _sessions.Count.ToString());

        //     foreach (Transform child in sessionListContent) {
        //         Destroy(child.gameObject);
        //     }

        //     foreach (SessionInfo session in _sessions) {
        //         Debug.Log("SessionFound " + session.Name);

        //         if (session.IsVisible) {
                    
        //             GameObject entry = GameObject.Instantiate(sessionEntryPrefab, sessionListContent);
        //             SessionEntryPrefab script = entry.GetComponent<SessionEntryPrefab>();

        //             Debug.Log("SessionLoaded " + session.Name);
        //             script.sessionName.text = session.Name;
        //             script.playerCount.text = session.PlayerCount + "/" + session.MaxPlayers;

        //             if (session.IsOpen == false || session.PlayerCount >= session.MaxPlayers) {
        //                 script.joinButton.interactable = false;
        //             } else {
        //                 script.joinButton.interactable = true;
        //             }
        //         }
        //     }
        // }

        public void OnConnectedToServer(NetworkRunner runner) {
                Debug.Log("OnConnectedToServer");
        }

        public void OnSessionListUpdated(NetworkRunner runner, List<SessionInfo> sessionList) {
            Debug.Log("OnSessionListUpdated");
            // sessionList.Clear();
            // _sessions = sessionList;
        }


        public void OnPlayerJoined(NetworkRunner runner, PlayerRef player) {
            Debug.Log("OnPlayerJoined()");
        }
        public void OnPlayerLeft(NetworkRunner runner, PlayerRef player) {
            Debug.Log("OnPlayerLeft()");
        }
        public void OnInput(NetworkRunner runner, NetworkInput input) { 
            Debug.Log("OnInput");
        }
        public void OnInputMissing(NetworkRunner runner, PlayerRef player, NetworkInput input) {
            Debug.Log("OnInputMissing");
        }
        public void OnShutdown(NetworkRunner runner, ShutdownReason shutdownReason) {
            Debug.Log("OnShutdown");
        }
    
        public void OnDisconnectedFromServer(NetworkRunner runner) {
            Debug.Log("OnDisconnectedFromServer");
        }
        public void OnConnectRequest(NetworkRunner runner, NetworkRunnerCallbackArgs.ConnectRequest request, byte[] token) {
            Debug.Log("OnConnectRequest");
        }
        public void OnConnectFailed(NetworkRunner runner, NetAddress remoteAddress, NetConnectFailedReason reason) {
            Debug.Log("OnConnectFailed");
        }
        public void OnUserSimulationMessage(NetworkRunner runner, SimulationMessagePtr message) {
            Debug.Log("OnUserSimulationMessage");
        }
        public void OnCustomAuthenticationResponse(NetworkRunner runner, Dictionary<string, object> data) {
            Debug.Log("OnCustomAuthenticationResponse");
        }
        public void OnHostMigration(NetworkRunner runner, HostMigrationToken hostMigrationToken) {
            Debug.Log("OnHostMigration");
        }
        public void OnReliableDataReceived(NetworkRunner runner, PlayerRef player, ArraySegment<byte> data) {
            Debug.Log("OnReliableDataReceived");
        }
        public void OnSceneLoadDone(NetworkRunner runner) {
            Debug.Log("OnSceneLoadDone");
        }
        public void OnSceneLoadStart(NetworkRunner runner) {
            Debug.Log("OnSceneLoadStart");
        }
    }
}
