using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Ball))]
public class Player : MonoBehaviour
{
    [SerializeReference]
    private Ball m_ball = null;

    Camera m_camera;

    string[] axisNames;

    void Start() {
        axisNames = new string[] {
            "Horizontal" + (int)m_ball.ID,
            "Vertical" + (int)m_ball.ID,
        };
        m_camera = Camera.main;
        m_ball = GetComponent<Ball>();
    }

    void Update() {
        Vector3 forward = Vector3.ProjectOnPlane(m_camera.transform.forward, Vector3.up);
        Vector3 right = Vector3.ProjectOnPlane(m_camera.transform.right, Vector3.up);

        Vector3 movementDirection = Input.GetAxis(axisNames[0])*right + Input.GetAxis(axisNames[1])*forward;
        m_ball.SetMovementDirection(movementDirection);
    }
}
