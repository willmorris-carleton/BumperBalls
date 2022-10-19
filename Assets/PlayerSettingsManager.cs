using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum PlayerID {
    Player1,
    Player2,
    Player3,
    Player4
}

public class PlayerSettingsManager : MonoBehaviour
{
    public static PlayerSettingsManager Instance;

    public Color[] PlayerColors;

    void OnValidate() {
        Instance = this;
    }

    public static Color GetPlayerColor(PlayerID playerId) {
        return Instance.PlayerColors[(int) playerId];
    }
}
