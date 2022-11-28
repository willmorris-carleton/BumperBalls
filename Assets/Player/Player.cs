using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField]
    PlayerID playerID;

    [SerializeReference]
    private Ball m_ball = null;

    Camera m_camera;

    string[] axisNames;

    void Start() {
        axisNames = new string[] {
            "Horizontal" + (int)playerID,
            "Vertical" + (int)playerID,
        };
        m_camera = Camera.main;
        m_ball.SetBallColor(PlayerSettingsManager.GetPlayerColor(playerID));
    }

    void OnValidate() {
        if (transform.GetChild(0).TryGetComponent(out m_ball)) {
            if (PlayerSettingsManager.Instance != null) {
                m_ball.SetBallColor(PlayerSettingsManager.GetPlayerColor(playerID));
            }
        }
    }

    void Update() {
        Vector3 forward = Vector3.ProjectOnPlane(m_camera.transform.forward, Vector3.up);
        Vector3 right = Vector3.ProjectOnPlane(m_camera.transform.right, Vector3.up);

        Vector3 movementDirection = Input.GetAxis(axisNames[0])*right + Input.GetAxis(axisNames[1])*forward;
        m_ball.SetMovementDirection(movementDirection);
    }
}
