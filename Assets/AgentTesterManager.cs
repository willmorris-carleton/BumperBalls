using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class AgentTesterManager : MonoBehaviour
{

    [SerializeField]
    Game m_game;

    [Range(1f, 100f)]
    public float speedMultiplier = 10f;

    [SerializeField]
    TextMeshProUGUI text;


    void Start() {
        m_game.gameOverDelegate += UpdateText;
    }


    void Update() {
        Time.timeScale = speedMultiplier;
    }

    void UpdateText() {
        string s = "";
        s += "Games played: " + m_game.numGames + "\n";
        for (int i = 0; i < m_game.balls.Count; i++) {
            s += getAgentName(m_game.balls[i]) + " Wins: " + m_game.balls[i].gamesWon + ", Deaths: " + m_game.balls[i].gamesDied + "\n";
        }
        text.text = s;
    }

    string getAgentName(Ball ball) {
        if (isMLAgent(ball)) return "MLAgent " + ball.ID;
        return "SimpleAgent " + ball.ID;
    }

    bool isMLAgent(Ball ball) {
        return ball.TryGetComponent<BallAgent>(out BallAgent ballAgent);
    }

}
