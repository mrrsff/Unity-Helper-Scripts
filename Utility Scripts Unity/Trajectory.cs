using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Trajectory
{
    public static List<Vector3> CalculateTrajectory(Rigidbody rb, Vector3 velocity, float time, int positionCount = 20)
    {
        if(rb == null)
        {
            Debug.LogWarning("Rigidbody is null, returning empty trajectory");
            return new List<Vector3>();
        }

        if(rb.mass == 0f)
        {
            Debug.LogWarning("Rigidbody has mass of 0, returning empty trajectory");
            return new List<Vector3>();
        }

        if(positionCount < 2)
        {
            Debug.LogWarning("Not enough positionCount to calculate trajectory, returning empty trajectory");
            return new List<Vector3>();
        }

        /*
            This function calculates the trajectory for a Rigidbody.

            The function returns a list of Vector3s that represent the trajectory.
        */
        List<Vector3> trajectory = new List<Vector3>();
        Vector3 gravity = Physics.gravity;
        Vector3 position = rb.position;
        float timeStep = time / positionCount;
        for(int i = 0; i < positionCount; i++)
        {
            trajectory.Add(position);
            position += velocity * timeStep + 0.5f * gravity * timeStep * timeStep;
            if(position.y < 0) position.y = 0;
            velocity += gravity * timeStep;

            // If it collides with something, stop calculating the trajectory
            RaycastHit hit;
            if(Physics.Linecast(position, position + velocity * timeStep, out hit))
            {
                trajectory.Add(hit.point);
                break;
            }
        }
        
        return trajectory;
    }
    public static Vector3 CalculateVelocity(Vector3 start, Vector3 end, float time = 1f)
    {
        /*
            This function calculates the velocity needed to throw a Rigidbody from a start position to an end position in a given time.

            The function returns a Vector3 that represents the velocity.
        */
        Vector3 gravity = Physics.gravity;
        Vector3 displacement = end - start;
        Vector3 displacementXZ = displacement;
        float distanceY = displacement.y;
        displacementXZ.y = 0f;
        float distanceXZ = displacementXZ.magnitude;
        float xzVelocity = distanceXZ / time;
        float yVelocity = distanceY / time + 0.5f * Mathf.Abs(gravity.y) * time;

        Vector3 velocity = displacementXZ.normalized * xzVelocity + Vector3.up * yVelocity;
        return velocity;
    }



}