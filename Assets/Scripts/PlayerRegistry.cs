using System;
using System.Collections.Generic;
using UnityEngine;
using Fusion;
using Fusion.Sockets;
using Helpers.Collections;
using System.Linq;

public class PlayerRegistry : Fusion.NetworkBehaviour, INetworkRunnerCallbacks
{
	public const byte CAPACITY = 10;

	public static PlayerRegistry Instance;

	public static int Count => Instance.ObjectByRef.Count;

	[Networked, Capacity(CAPACITY)]
	NetworkDictionary<PlayerRef, PlayerObject> ObjectByRef { get; }

	private void Start()
	{
        Debug.Log("PlayerRegistry Start()");
		Instance = this;
	}

	public override void Despawned(NetworkRunner runner, bool hasState)
	{
        Debug.Log("PlayerRegistry Despawned()");
		base.Despawned(runner, hasState);
		Instance = null;
	}

	public static void Server_Add(NetworkRunner runner, PlayerRef pRef, PlayerObject pObj)
	{
        Debug.Log("PlayerRegistry Server_Add()");

		if (Instance.GetAvailable(out byte index))
		{
			Instance.ObjectByRef.Add(pRef, pObj);
			pObj.Server_Init(pRef, index);
            Debug.Log($"PlayerRegistry Count {Count}");
		}
		else
		{
			Debug.LogWarning($"Unable to register player {pRef}", pObj);
		}
	}

    public static void Server_Remove(NetworkRunner runner, PlayerRef pRef)
	{
		Debug.Assert(pRef.IsValid);

		if (Instance.ObjectByRef.Remove(pRef) == false)
		{
			Debug.LogWarning("Could not remove player from registry");
		}
	}

	public static void ForEach(System.Action<PlayerObject> action)
	{
		foreach(var kvp in Instance.ObjectByRef)
		{
			action.Invoke(kvp.Value);
		}
	}

    bool GetAvailable(out byte index)
	{
		if (ObjectByRef.Count == 0)
		{
			index = 0;
			return true;
		}
		else if (ObjectByRef.Count == CAPACITY)
		{
			index = default;
			return false;
		}

		byte[] indices = ObjectByRef.OrderBy(kvp => kvp.Value.Index).Select(kvp => kvp.Value.Index).ToArray();
		
		for (int i = 0; i < indices.Length - 1; i++)
		{
			if (indices[i + 1] > indices[i] + 1)
			{
				index = (byte)(indices[i] + 1);
				return true;
			}
		}

		index = (byte)(indices[indices.Length - 1] + 1);
		return true;
	}

    public static PlayerObject[] GetRandom(int count) {
		List<PlayerObject> players = new List<PlayerObject>();
		foreach (var kvp in Instance.ObjectByRef)
		{
			players.Add(kvp.Value);
		}
		
		return players.Grab(count).ToArray();
	}

    public void OnConnectedToServer(NetworkRunner runner) { }
    public void OnSessionListUpdated(NetworkRunner runner, List<SessionInfo> sessionList) { }
    public void OnPlayerJoined(NetworkRunner runner, PlayerRef player) { }
    public void OnPlayerLeft(NetworkRunner runner, PlayerRef player) {
        Debug.Log("PlayerRegisry OnPlayerLeft()");
        Server_Remove(runner, player);
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