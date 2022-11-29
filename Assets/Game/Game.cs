using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Game : MonoBehaviour
{

    [SerializeReference]
    GameObject mapObject;
    Renderer mapRenderer = null;
    
    [SerializeField]
    TextMeshProUGUI text;
    public float gameLength = 60.0f;

    public List<Ball> balls = new List<Ball>();

    public List<Vector3> possibleStartingPositions = new List<Vector3>();
    List<Vector3> availableStartingPositions = new List<Vector3>();

    float timeStarted = 0f;

    private void Awake() {
        for (int i = 0; i < balls.Count; i++) {
            balls[i].ID = (BallID)i;
            balls[i].SetBallColor(BallColorSettingsManager.GetColor(balls[i].ID));
        }
    }

    private void Start() {
        StartNewGame();
        mapRenderer = mapObject.GetComponent<Renderer>();
    }

    private void StartNewGame() {
        timeStarted = Time.time;

        foreach (Ball ball in balls) {
            ball.Respawn(GetStartingPosition());
        }
    }

    private void EndGame() {
        
        //If there is a winner...
        if (NumberBallsAlive() == 1) {
            int winnerIndex = GetWinnerIndex();
            SetMapColour(BallColorSettingsManager.GetColor(balls[winnerIndex].ID));
            if (balls[winnerIndex].TryGetComponent<BallAgent>(out BallAgent ballAgent)) {
                //ballAgent.SetReward(1);
                ballAgent.EndEpisode();
                ballAgent.enabled = false;
            }
        }
        //If there is a tie then no winner...
        else {
            SetMapColour(Color.white);

            //End Agent episodes for those that are still enabled because of a tie
            for (int i = 0; i < balls.Count; i++) {
                if (balls[i].TryGetComponent<BallAgent>(out BallAgent ballAgent)) {
                    if (ballAgent.enabled) {
                        ballAgent.EndEpisode();
                        ballAgent.enabled = false;
                    }
                }
            }
        }
    }

    int GetWinnerIndex() {
        for (int i = 0; i < balls.Count; i++) {
            if (!balls[i].isDead()) return i;
        }
        return -1; //Should not happen
    }

    void SetMapColour(Color color) {
        mapRenderer.material.SetColor("_LineColor", color);
    }

    private void Update() {

        text.SetText(Mathf.RoundToInt(gameLength - (Time.time - timeStarted)).ToString());

        if (Time.time - timeStarted > gameLength || NumberBallsAlive() <= 1) {
            EndGame();
            StartNewGame();
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

    public int NumberBallsAlive() {
        int n = 0;
        for (int i = 0; i < balls.Count; i++) {
            if (!balls[i].isDead()) n++;
            //Debug.Log(i + ": " + (balls[i].isDead() ? "Dead" : "Alive"));
        }
        return n;
    }
}
