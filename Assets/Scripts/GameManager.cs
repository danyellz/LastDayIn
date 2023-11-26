using UnityEngine;
using UnityEngine.UI;
using Fusion;
using Fusion.Sockets;
using System.Collections.Generic;
using System;
using TMPro;
using System.Linq;
using static PlayerRegistry;

namespace FirstDayIn.Network {
    public class GameManager: Fusion.NetworkBehaviour, INetworkRunnerCallbacks
    {

        public static GameManager instance;
        [HideInInspector] public NetworkRunner runner;
        [SerializeField] NetworkObject playerPrefab;

        public string _playerName = null;

        private List<SessionInfo> _sessions = new List<SessionInfo>(); 

        [Header("HUD")]
        public GameObject hudCanvas;

        public Transform topButtons;
        [SerializeField] public TMP_Text playerCountLabel;
        public Button startGameButton;

        [Header("SessionList")]
        public GameObject roomListCanvas;  
        public Button createButton;
        public Button refreshButton;
        public Transform sessionListContent;
        public GameObject sessionEntryPrefab;

        public bool isCreateSession = false;

        private void Awake() {
            if (instance == null) {
                Debug.Log("GameManager Awake()");
                instance = this;
            } 

            refreshButton.interactable = false;
            createButton.interactable = false;
        }

        public async void CreateSession() {
            Debug.Log("CreateSession");
            isCreateSession = true;

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

            Debug.Log("Session Created - Session Name: " + randomSessionName);
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

        public void StartGame() {
            Debug.Log($"PlayerCount: {PlayerRegistry.Count}");
            GameState.Instance.Server_SetState(GameState.EGameState.Play);
            hudCanvas.SetActive(false);
        }

        public void PlayerRegistry_Add(PlayerRef pRef, PlayerObject pObj) {
            PlayerRegistry.Server_Add(runner, pRef, pObj);
        }

        public override void FixedUpdateNetwork() {}

        public void OnConnectedToServer(NetworkRunner runner) {
                Debug.Log("OnConnectedToServer");
                NetworkObject playerObject = runner.Spawn(playerPrefab);
                runner.SetPlayerObject(runner.LocalPlayer, playerObject);

                GameState.Instance.Server_SetState(GameState.EGameState.Pregame);
                hudCanvas.SetActive(true);
        }

        public void OnSessionListUpdated(NetworkRunner runner, List<SessionInfo> sessionList) {
            Debug.Log("OnSessionListUpdated");
            _sessions = sessionList;
            createButton.interactable = true;
            refreshButton.interactable = true;
        }


        public void OnPlayerJoined(NetworkRunner runner, PlayerRef player) {
            SessionInfo sessionInfo = runner.SessionInfo;
            string playerCount = sessionInfo.PlayerCount.ToString();
            GameObject pCountLabel = GameObject.Find("PlayerCountLabel");

            Debug.Log("Player joined! Count = " + playerCount);

            pCountLabel.GetComponent<TextMeshProUGUI>().text = playerCount + "/10";
        }

        public void OnPlayerLeft(NetworkRunner runner, PlayerRef player) {
            SessionInfo sessionInfo = runner.SessionInfo;
            string playerCount = sessionInfo.PlayerCount.ToString();
            GameObject pCountLabel = GameObject.Find("PlayerCountLabel");

            Debug.Log("Player left! Count = " + playerCount);

            pCountLabel.GetComponent<TextMeshProUGUI>().text = playerCount + "/10";
        }
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
