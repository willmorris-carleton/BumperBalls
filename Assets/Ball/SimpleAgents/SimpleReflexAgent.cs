using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Ball))]
public abstract class SimpleReflexAgent : MonoBehaviour
{
    [SerializeReference]
    Game m_game;
    
    [SerializeReference]
    Ball m_ball = null;

    void FixedUpdate() {
        //Get positions and velocities for all balls
        List<Vector3> otherBallPositions = new List<Vector3>();
        List<Vector3> otherBallVelocities = new List<Vector3>();
        foreach (Ball ball in m_game.balls) {
            if (ball.ID != m_ball.ID && !ball.isDead()) {
                otherBallPositions.Add(ball.transform.localPosition);
                otherBallVelocities.Add(ball.rb.velocity);
            }
        }

        PerformAction(m_ball, otherBallPositions, otherBallVelocities);
    }

    //Perform an action given a ball and rest of environment that the agent can see
    protected abstract void PerformAction(Ball ball, List<Vector3> otherBallPositions, List<Vector3> otherBallVelocities);
}
