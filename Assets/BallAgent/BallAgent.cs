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
    private BallController m_ball;

    public override void OnEpisodeBegin()
    {
        m_ball.transform.localPosition = m_game.GetStartingPosition();
    }

    public override void CollectObservations(VectorSensor sensor)
    {
        //Position of every ball
        sensor.AddObservation(m_ball.transform.localPosition);

        //Velocity of every ball
        sensor.AddObservation(m_ball.rb.velocity);
    }

    public override void OnActionReceived(ActionBuffers actions) {
        float sideMovement = actions.ContinuousActions[0];
        float forwardMovement = actions.ContinuousActions[1];

        if (!m_ball.isDead()) {
            m_ball.SetMovementDirection(new Vector3(sideMovement, 0, forwardMovement));

            AddReward(-0.001f);
        }
    }

    public override void Heuristic(in ActionBuffers actionsOut)
    {
        ActionSegment<float> continuousActions = actionsOut.ContinuousActions;
        continuousActions[0] = Input.GetAxis("Horizontal1");
        continuousActions[1] = Input.GetAxis("Vertical1");
    }

}
