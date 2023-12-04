using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;

public class NetworkStartBridge : MonoBehaviour
{

    public static NetworkStartBridge Instance { get; set; }
    public NetworkDebugStart starter;
    
   private void Start() {
        Instance = this;
   }

    public void CreateSession(string sessionName) {
        Debug.Log("CreateSession");
        starter.DefaultRoomName = sessionName;
        starter.StartHost();
    }

        // TODO: - Add Session Name Manual Entry.
    public void ConnectToSession(string sessionName) {
        Debug.Log("ConnectToSession");
        starter.DefaultRoomName = sessionName;
        starter.StartClient();
    }
}
