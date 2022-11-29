using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;

public class BallAgent : Agent
{

    [SerializeReference]
    private Game m_game;

    [SerializeReference]
    private Ball m_ball;

    public override void CollectObservations(VectorSensor sensor)
    {
        //Position and velocity of itself
        sensor.AddObservation(m_ball.transform.localPosition);
        sensor.AddObservation(m_ball.rb.velocity);

        //Position and Velocity of every other ball
        foreach (Ball ball in m_game.balls) {
            if (ball.ID != m_ball.ID) {
                sensor.AddObservation(ball.transform.localPosition);
                sensor.AddObservation(ball.rb.velocity);
            }
        }
    }

    public override void OnActionReceived(ActionBuffers actions) {
        float sideMovement = actions.ContinuousActions[0];
        float forwardMovement = actions.ContinuousActions[1];

        m_ball.SetMovementDirection(new Vector3(sideMovement, 0, forwardMovement));
        AddReward(0.001f); //Reward for surviving
    }


    public override void Heuristic(in ActionBuffers actionsOut)
    {
        ActionSegment<float> continuousActions = actionsOut.ContinuousActions;
        continuousActions[0] = Input.GetAxis("Horizontal1");
        continuousActions[1] = Input.GetAxis("Vertical1");
    }

}
