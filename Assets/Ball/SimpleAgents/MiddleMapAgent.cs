using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiddleMapAgent : SimpleReflexAgent
{
    protected override void PerformAction(Ball ball, List<Vector3> otherBallPositions, List<Vector3> otherBallVelocities)
    {
        ball.SetMovementDirection(-ball.transform.localPosition.normalized); //Make ball always move towards center of map
    }
}
