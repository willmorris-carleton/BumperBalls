using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum BallID {
    Ball1,
    Ball2,
    Ball3,
    Ball4
}

public class BallColorSettingsManager : MonoBehaviour
{
    public static BallColorSettingsManager Instance;

    public Color[] BallColors;

    void OnValidate() {
        Instance = this;
    }

    public static Color GetColor(BallID ballId) {
        Debug.Log(ballId);
        return Instance.BallColors[(int) ballId];
    }
}
