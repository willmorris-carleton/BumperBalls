using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody), typeof(Renderer))]
public class Ball : MonoBehaviour
{
    [HideInInspector]
    public BallID ID; //Set by game

    public float movementForce = 10.0f;
    public float minKnockbackImpulseForce = 0.1f;
    public float maxKnockbackImpulseForce = 1f;

    [HideInInspector]
    public Rigidbody rb;

    Vector3 m_movementDirection = Vector3.zero;

    Renderer m_renderer;

    bool m_currentlyAlive = true;

    public int gamesWon = 0;
    public int gamesDied = 0;

    void OnValidate() {
        rb = GetComponent<Rigidbody>();
        m_renderer = GetComponent<Renderer>();
    }

    public void SetMovementDirection(Vector3 direction) {
        m_movementDirection = Vector3.ProjectOnPlane(direction, Vector3.up);
        m_movementDirection.Normalize();
    }

    public void SetBallColor(Color c) {
        if (TryGetComponent(out m_renderer)) {
            m_renderer.material.SetColor("_Color", c);
        }
    }

    public void Respawn(Vector3 startingPos) {
        gameObject.SetActive(true);
        transform.localPosition = startingPos;
        transform.rotation = Quaternion.identity; 
        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
        m_currentlyAlive = true;
        if (TryGetComponent<BallAgent>(out BallAgent ballAgent)) {
            ballAgent.enabled = true;
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        //Apply force in direction
        rb.AddForce(m_movementDirection*movementForce*Time.deltaTime, ForceMode.Acceleration);
    }
    
    public bool isDead() {
        return !m_currentlyAlive;
    }

    public void die() {
        gameObject.SetActive(false);
        m_currentlyAlive = false;
        gamesDied++;

        if (TryGetComponent<BallAgent>(out BallAgent ballAgent)) {
            ballAgent.SetReward(-1f);
            ballAgent.EndEpisode();
            ballAgent.enabled = false;
        }
    }

    void OnCollisionEnter(Collision other) {
        Ball otherBall = other.gameObject.GetComponent<Ball>();
        if (otherBall != null) {
            //Debug.Log("Collision");

            ParticleEffectsManager.CreateExplosion(other.GetContact(0).point);

            Vector3 relativeVelocity = other.relativeVelocity;
            if (other.relativeVelocity.sqrMagnitude < 0.1f) {
                relativeVelocity = -rb.velocity;
            }
            float knockbackMagnitude = Mathf.Clamp(relativeVelocity.magnitude, minKnockbackImpulseForce, maxKnockbackImpulseForce);
            rb.AddForce(relativeVelocity.normalized*knockbackMagnitude, ForceMode.Impulse);
        }
    }

    public void wonGame() {
        if (TryGetComponent<BallAgent>(out BallAgent ballAgent)) {
            //ballAgent.SetReward(1);
            ballAgent.EndEpisode();
            ballAgent.enabled = false;
        }
        gamesWon++;
    }
}
