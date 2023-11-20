using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;
using TMPro;
using FirstDayIn.Network;

public class PlayerInfo : NetworkBehaviour
{
    [Networked(OnChanged = nameof(UpdatePlayerName))] public NetworkString<_32> PlayerName { get; set; }
    [SerializeField] TextMeshPro playerLabel;

    private void Start() {
        if (this.HasStateAuthority) {
            PlayerName = FusionConnection.instance._playerName;
        }
    }

    protected static void UpdatePlayerName(Changed<PlayerInfo> changed) {
        changed.Behaviour.playerLabel.text = changed.Behaviour.PlayerName.ToString();
    }
}
