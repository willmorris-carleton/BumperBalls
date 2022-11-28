using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Game : MonoBehaviour
{
    public float gameLength = 60.0f;

    public List<Ball> balls = new List<Ball>();

    public List<Vector3> possibleStartingPositions = new List<Vector3>();
    List<Vector3> availableStartingPositions;

    float timeStarted = 0f;

    private void Start() {
        timeStarted = Time.time;
    }

    private void Update() {
        for (int i = 0; i < balls.Count; i++) {
            //Debug.Log(i + ": " + (balls[i].isDead() ? "Dead" : "Alive"));
        }
    }

    public Vector3 GetStartingPosition() {
        if (availableStartingPositions.Count == 0) {
            availableStartingPositions = new List<Vector3>(possibleStartingPositions);
        }

        int randomIndex = Random.Range(0, availableStartingPositions.Count);
        Vector3 pos = availableStartingPositions[randomIndex];
        availableStartingPositions.RemoveAt(randomIndex);
        return pos;
    }
}
