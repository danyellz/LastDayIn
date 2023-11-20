 using UnityEngine;
using Fusion;
using Fusion.Sockets;
using System.Collections.Generic;
using System;

namespace FirstDayIn.Network {
    public class FusionConnection : MonoBehaviour, INetworkRunnerCallbacks
    {

        public static FusionConnection instance;
        public bool connectOnAwake = false;
        [HideInInspector] public NetworkRunner runner;
        [SerializeField] NetworkObject playerPrefab;
        private Dictionary<PlayerRef, NetworkObject> spawnedCharacters = new Dictionary<PlayerRef, NetworkObject>();

        public string _playerName = null;

        private void Awake() {
            if (instance == null) {
                instance = this;
            }

            if (connectOnAwake == true) {
                ConnectToRunner("Anonymous");
            }
        }

        public async void ConnectToRunner(string playerName) {
            Debug.Log("ConnectToRunner");

            _playerName = playerName;

            if (runner == null) {
                runner = gameObject.AddComponent<NetworkRunner>();
            }

            await runner.StartGame(new StartGameArgs() {
                GameMode = GameMode.Shared,
                SessionName = "test3",
                PlayerCount = 10,
                SceneManager = gameObject.AddComponent<NetworkSceneManagerDefault>()
            });
        }

        public void OnPlayerJoined(NetworkRunner runner, PlayerRef player) {
            Debug.Log("OnPlayerJoined");
        }
        public void OnPlayerLeft(NetworkRunner runner, PlayerRef player) { 
            // Find and remove the players avatar
            if (spawnedCharacters.TryGetValue(player, out NetworkObject networkObject)) {
                runner.Despawn(networkObject);
            }
        }
        public void OnInput(NetworkRunner runner, NetworkInput input) { }
        public void OnInputMissing(NetworkRunner runner, PlayerRef player, NetworkInput input) { }
        public void OnShutdown(NetworkRunner runner, ShutdownReason shutdownReason) { }
        public void OnConnectedToServer(NetworkRunner runner) {
                Debug.Log("OnConnectedToServer");
                NetworkObject playerObject = runner.Spawn(playerPrefab, Vector3.zero);
                runner.SetPlayerObject(runner.LocalPlayer, playerObject);
        }
        public void OnDisconnectedFromServer(NetworkRunner runner) { }
        public void OnConnectRequest(NetworkRunner runner, NetworkRunnerCallbackArgs.ConnectRequest request, byte[] token) { }
        public void OnConnectFailed(NetworkRunner runner, NetAddress remoteAddress, NetConnectFailedReason reason) { }
        public void OnUserSimulationMessage(NetworkRunner runner, SimulationMessagePtr message) { }
        public void OnSessionListUpdated(NetworkRunner runner, List<SessionInfo> sessionList) { }
        public void OnCustomAuthenticationResponse(NetworkRunner runner, Dictionary<string, object> data) { }
        public void OnHostMigration(NetworkRunner runner, HostMigrationToken hostMigrationToken) { }
        public void OnReliableDataReceived(NetworkRunner runner, PlayerRef player, ArraySegment<byte> data) { }
        public void OnSceneLoadDone(NetworkRunner runner) { }
        public void OnSceneLoadStart(NetworkRunner runner) { }
    }
}
