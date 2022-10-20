using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody), typeof(Renderer))]
public class BallController : MonoBehaviour
{

    public float movementForce = 10.0f;
    //[Range(0.01f, 0.99f)]
    public float minKnockbackImpulseForce = 0.1f;
    //[Range(1f, 3f)]
    public float maxKnockbackImpulseForce = 1f;

    [HideInInspector]
    public Rigidbody rb;

    Vector3 m_movementDirection = Vector3.zero;

    Renderer m_renderer;

    void OnValidate() {
        rb = GetComponent<Rigidbody>();
        m_renderer = GetComponent<Renderer>();
    }

    public void SetMovementDirection(Vector3 direction) {
        m_movementDirection = direction;
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
        if (rb.transform.position.y < -5) {
            rb.transform.position = Vector3.up;
            rb.velocity = Vector3.zero;
        }

        //rb.velocity *= (1f - Time.deltaTime);
    }

    void OnCollisionEnter(Collision other) {
        BallController otherBall = other.gameObject.GetComponent<BallController>();
        if (otherBall != null) {
            Debug.Log("Collision");

            ParticleEffectsManager.CreateExplosion(other.GetContact(0).point);

            //if (other.relativeVelocity.magnitude < 0 || Vector3.Dot(other.relativeVelocity, rb.velocity) > 0) {

            //}
            Vector3 relativeVelocity = other.relativeVelocity;
            if (other.relativeVelocity.sqrMagnitude < 0.1f) {
                relativeVelocity = -rb.velocity;
            }
            float knockbackMagnitude = Mathf.Clamp(relativeVelocity.magnitude, minKnockbackImpulseForce, maxKnockbackImpulseForce);
            rb.AddForce(relativeVelocity.normalized*knockbackMagnitude, ForceMode.Impulse);
        }
        
    }
}
