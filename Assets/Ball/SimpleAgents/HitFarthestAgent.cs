using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitFarthestAgent : SimpleReflexAgent
{
    protected override void PerformAction(Ball ball, List<Vector3> otherBallPositions, List<Vector3> otherBallVelocities)
    {
        if (otherBallPositions.Count == 0) {
            ball.SetMovementDirection(-ball.transform.localPosition.normalized);
            return;
        }
        
        //Find the nearest ball position
        Vector3 ballPosition = ball.transform.localPosition;
        Vector3 farthestPos = otherBallPositions[0];
        float currentBiggestDistance = Vector3.Distance(ballPosition, otherBallPositions[0]);

        if (otherBallPositions.Count > 1) {
            for (int i = 1; i < otherBallPositions.Count; i++) {
                float d = Vector3.Distance(ballPosition, otherBallPositions[i]);
                if (d > currentBiggestDistance) {
                    currentBiggestDistance = d;
                    farthestPos = otherBallPositions[i];
                }
            }
        }


        ball.SetMovementDirection((farthestPos - ballPosition).normalized); //Make ball move towards farthest ball
    }
    
}