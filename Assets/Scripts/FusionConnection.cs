using UnityEngine;
using UnityEngine.UI;
using Fusion;
using Fusion.Sockets;
using System.Collections.Generic;
using System;

namespace FirstDayIn.Network {
    public class FusionConnection : NetworkBehaviour, INetworkRunnerCallbacks
    {

        public static FusionConnection instance;
        [HideInInspector] public NetworkRunner runner;
        [SerializeField] NetworkObject playerPrefab;

        public string _playerName = null;
        private List<SessionInfo> _sessions = new List<SessionInfo>(); 

        [SerializeField] GameState stateManager;
        public GameState.EGameState _state;

        [Header("SessionList")]
        public GameObject roomListCanvas;  
        public Button refreshButton;
        public Transform sessionListContent;
        public GameObject sessionEntryPrefab;

        private void Awake() {
            if (instance == null) {
                Debug.Log("FusionConnection Awake()");
                instance = this;
                stateManager = gameObject.AddComponent<GameState>();
            } 
        }

        private void Spawned() {
            Debug.Log("FusionConnection Spawned()");
            if (Runner.IsServer) {
			    stateManager.Server_SetState(GameState.EGameState.Pregame);
		    }
        }

        public async void CreateSession() {
            Debug.Log("CreateSession");

            roomListCanvas.SetActive(false);
            int randomInt = UnityEngine.Random.Range(1000,9999);
            string randomSessionName = "Room-" + randomInt.ToString();

            if (runner == null) {
                runner = gameObject.AddComponent<NetworkRunner>();
            }

            await runner.StartGame(new StartGameArgs() {
                GameMode = GameMode.Shared,
                SessionName = randomSessionName,
                PlayerCount = 10,
            });
        }

         public async void ConnectToSession(string sessionName) {
            Debug.Log("ConnectToSession");

            roomListCanvas.SetActive(false);
            if (runner == null) {
                runner = gameObject.AddComponent<NetworkRunner>();
            }

            await runner.StartGame(new StartGameArgs() {
                GameMode = GameMode.Shared,
                SessionName = sessionName,
            });
        }

        public void ConnectToLobby(string playerName) {
            Debug.Log("OnConnectToLobby " + playerName);

            roomListCanvas.SetActive(true);
            _playerName = playerName;
            _state = GameState.EGameState.Pregame;

            if (runner == null) {
                runner = gameObject.AddComponent<NetworkRunner>();
            }

            runner.JoinSessionLobby(SessionLobby.Shared); 
        }

        public void RefreshSessionListUI() {
             Debug.Log("RefreshSessionListUI" + _sessions.Count.ToString());

            foreach (Transform child in sessionListContent) {
                Destroy(child.gameObject);
            }

            foreach (SessionInfo session in _sessions) {
                Debug.Log("SessionFound " + session.Name);

                if (session.IsVisible) {
                    
                    GameObject entry = GameObject.Instantiate(sessionEntryPrefab, sessionListContent);
                    SessionEntryPrefab script = entry.GetComponent<SessionEntryPrefab>();

                    Debug.Log("SessionLoaded " + session.Name);
                    script.sessionName.text = session.Name;
                    script.playerCount.text = session.PlayerCount + "/" + session.MaxPlayers;

                    if (session.IsOpen == false || session.PlayerCount >= session.MaxPlayers) {
                        script.joinButton.interactable = false;
                    } else {
                        script.joinButton.interactable = true;
                    }
                }
            }
        }

        public void OnConnectedToServer(NetworkRunner runner) {
                Debug.Log("OnConnectedToServer");
                NetworkObject playerObject = runner.Spawn(playerPrefab, Vector3.zero);
                runner.SetPlayerObject(runner.LocalPlayer, playerObject);
        }

        public void OnSessionListUpdated(NetworkRunner runner, List<SessionInfo> sessionList) {
            Debug.Log("OnSessionListUpdated");
            _sessions.Clear();
            _sessions = sessionList;
        }


        public void OnPlayerJoined(NetworkRunner runner, PlayerRef player) {
            Debug.Log("OnPlayerJoined");
        }

        public void OnPlayerLeft(NetworkRunner runner, PlayerRef player) { }
        public void OnInput(NetworkRunner runner, NetworkInput input) { }
        public void OnInputMissing(NetworkRunner runner, PlayerRef player, NetworkInput input) { }
        public void OnShutdown(NetworkRunner runner, ShutdownReason shutdownReason) { }
    
        public void OnDisconnectedFromServer(NetworkRunner runner) { }
        public void OnConnectRequest(NetworkRunner runner, NetworkRunnerCallbackArgs.ConnectRequest request, byte[] token) { }
        public void OnConnectFailed(NetworkRunner runner, NetAddress remoteAddress, NetConnectFailedReason reason) { }
        public void OnUserSimulationMessage(NetworkRunner runner, SimulationMessagePtr message) { }
        public void OnCustomAuthenticationResponse(NetworkRunner runner, Dictionary<string, object> data) { }
        public void OnHostMigration(NetworkRunner runner, HostMigrationToken hostMigrationToken) { }
        public void OnReliableDataReceived(NetworkRunner runner, PlayerRef player, ArraySegment<byte> data) { }
        public void OnSceneLoadDone(NetworkRunner runner) { }
        public void OnSceneLoadStart(NetworkRunner runner) { }
    }
}
