using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitNearestAgent : SimpleReflexAgent
{
    protected override void PerformAction(Ball ball, List<Vector3> otherBallPositions, List<Vector3> otherBallVelocities)
    {
        if (otherBallPositions.Count == 0) {
            ball.SetMovementDirection(-ball.transform.localPosition.normalized);
            return;
        }

        //Find the nearest ball position
        Vector3 ballPosition = ball.transform.localPosition;
        Vector3 closestPos = otherBallPositions[0];
        float currentSmallestDistance = Vector3.Distance(ballPosition, otherBallPositions[0]);

        if (otherBallPositions.Count > 1) {
            for (int i = 1; i < otherBallPositions.Count; i++) {
                float d = Vector3.Distance(ballPosition, otherBallPositions[i]);
                if (d < currentSmallestDistance) {
                    currentSmallestDistance = d;
                    closestPos = otherBallPositions[i];
                }
            }
        }


        ball.SetMovementDirection((closestPos - ballPosition).normalized); //Make ball move towards closest ball
    }
    
}
