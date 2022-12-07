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

    public float minHitSpeedForReward = 2.0f;
    public float maxHitSpeedForReward = 8.0f;

    float minHitSpeedSqr;
    float maxHitSpeedSqr;

    Ball lastBallCollidedWith = null;
    float timeLastCollided = 0.0f;
    public float timeSinceHitToCountAsKill = 0.20f;

    public override void Initialize() {
        minHitSpeedSqr = minHitSpeedForReward*minHitSpeedForReward;
        maxHitSpeedSqr = maxHitSpeedForReward*maxHitSpeedForReward;
        m_ball.onDeathDelegate += OnBallDeath;
        m_ball.onWinDelegate += OnBallWin;
    }

    public override void CollectObservations(VectorSensor sensor)
    {
        //Position and velocity of itself
        sensor.AddObservation(m_ball.transform.localPosition.x / m_game.GetMapRadius());
        sensor.AddObservation(m_ball.transform.localPosition.z / m_game.GetMapRadius());
        sensor.AddObservation(m_ball.rb.velocity.x);
        sensor.AddObservation(m_ball.rb.velocity.z);

        //Position and Velocity of every other ball
        foreach (Ball ball in m_game.balls) {
            if (ball.ID != m_ball.ID) {
                sensor.AddObservation(ball.transform.localPosition.x / m_game.GetMapRadius());
                sensor.AddObservation(ball.transform.localPosition.z / m_game.GetMapRadius());
                sensor.AddObservation(ball.rb.velocity.x);
                sensor.AddObservation(ball.rb.velocity.z);
                sensor.AddOneHotObservation(ball.isDead() ? 1 : 0, 1);
            }
        }
    }

    public override void OnActionReceived(ActionBuffers actions) {
        float sideMovement = actions.ContinuousActions[0];
        float forwardMovement = actions.ContinuousActions[1];

        m_ball.SetMovementDirection(new Vector3(sideMovement, 0, forwardMovement));

        bool isCloseToEdge = (m_ball.transform.localPosition.magnitude / m_game.GetMapRadius()) >= 0.8f;

        if (isCloseToEdge) {
            AddReward(-0.001f);
        }
        else {
            AddReward(0.001f); //Reward for surviving
        }
    }


    public override void Heuristic(in ActionBuffers actionsOut)
    {
        ActionSegment<float> continuousActions = actionsOut.ContinuousActions;
        continuousActions[0] = Input.GetAxis("Horizontal1");
        continuousActions[1] = Input.GetAxis("Vertical1");
    }

    void OnCollisionEnter(Collision other) {
        Ball otherBall = other.gameObject.GetComponent<Ball>();
        if (otherBall != null) {

            //Save last hit ball
            lastBallCollidedWith = otherBall;
            timeLastCollided = Time.time;
            
            //If the hit was big enough, then agent closer to center recieves reward
            float relativeVelocitySqrMag = other.relativeVelocity.sqrMagnitude;

            if (relativeVelocitySqrMag > minHitSpeedSqr) {
                if (m_ball.transform.localPosition.sqrMagnitude < otherBall.transform.localPosition.sqrMagnitude) {
                    //float reward = Mathf.Lerp(0.01f, 0.1f, (relativeVelocitySqrMag - minHitSpeedSqr) / (maxHitSpeedSqr - minHitSpeedSqr));
                    AddReward(0.01f);
                    //Debug.Log("Speed: " + relativeVelocitySqrMag.ToString() + " R: " + reward.ToString());
                }
            }
        }
    }

    void OnBallDeath() {
        AddReward(-1f);
        EndEpisode();
        this.enabled = false;

        //This ball was killed so reward other ball for kill
        /*
        if (lastBallCollidedWith != null && !lastBallCollidedWith.isDead() && Time.time - timeLastCollided < timeSinceHitToCountAsKill) {
            //Debug.Log(lastBallCollidedWith.ID + " killed " + m_ball.ID);
            if (lastBallCollidedWith.TryGetComponent<BallAgent>(out BallAgent otherBallAgent)) {
                otherBallAgent.SetReward(1f);
            }
        }
        */
    }

    void OnBallWin() {
        AddReward(1f);
        EndEpisode();
        this.enabled = false;
    }

}
