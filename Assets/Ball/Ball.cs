using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody), typeof(Renderer))]
public class Ball : MonoBehaviour
{
    [SerializeField]
    public BallID ID;

    public float movementForce = 10.0f;
    public float minKnockbackImpulseForce = 0.1f;
    public float maxKnockbackImpulseForce = 1f;

    [HideInInspector]
    public Rigidbody rb;

    Vector3 m_movementDirection = Vector3.zero;

    Renderer m_renderer;

    bool m_currentlyAlive = false;

    void OnValidate() {
        rb = GetComponent<Rigidbody>();
        m_renderer = GetComponent<Renderer>();
        if (BallColorSettingsManager.Instance) SetBallColor(BallColorSettingsManager.GetColor(ID));
    }

    private void Start() {
        if (BallColorSettingsManager.Instance) SetBallColor(BallColorSettingsManager.GetColor(ID));
    }

    public void SetMovementDirection(Vector3 direction) {
        m_movementDirection = Vector3.ProjectOnPlane(direction, Vector3.up);
        m_movementDirection.Normalize();
    }

    public void SetBallColor(Color c) {
        if (TryGetComponent(out m_renderer)) {
            m_renderer.sharedMaterial.color = c;
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        //Apply force in direction
        rb.AddForce(m_movementDirection*movementForce*Time.deltaTime, ForceMode.Acceleration);

        //TEMP
        if (m_currentlyAlive && transform.position.y < -5) {
            die();
        }
    }
    
    public bool isDead() {
        return m_currentlyAlive;
    }

    void die() {
        gameObject.SetActive(false);
        m_currentlyAlive = false;
    }

    void OnCollisionEnter(Collision other) {
        Ball otherBall = other.gameObject.GetComponent<Ball>();
        if (otherBall != null) {
            Debug.Log("Collision");

            ParticleEffectsManager.CreateExplosion(other.GetContact(0).point);

            Vector3 relativeVelocity = other.relativeVelocity;
            if (other.relativeVelocity.sqrMagnitude < 0.1f) {
                relativeVelocity = -rb.velocity;
            }
            float knockbackMagnitude = Mathf.Clamp(relativeVelocity.magnitude, minKnockbackImpulseForce, maxKnockbackImpulseForce);
            rb.AddForce(relativeVelocity.normalized*knockbackMagnitude, ForceMode.Impulse);
        }
        
    }
}
