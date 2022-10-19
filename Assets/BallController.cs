using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody), typeof(Renderer))]
public class BallController : MonoBehaviour
{

    public float movementForce = 10.0f;

    [HideInInspector]
    public Rigidbody rb;

    [SerializeField]
    ParticleSystem m_ps;

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
        m_renderer.sharedMaterial.color = c;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        //Apply force in direction
        rb.AddForce(m_movementDirection*movementForce*Time.deltaTime, ForceMode.Acceleration);
    }

    void OnCollisionEnter(Collision other) {
        BallController otherBall = other.gameObject.GetComponent<BallController>();
        if (otherBall != null) {
            Debug.Log("Collision");

            m_ps.transform.position = other.GetContact(0).point;
            m_ps.Play();

            otherBall.rb.velocity += rb.velocity;
        }
        
    }
}
