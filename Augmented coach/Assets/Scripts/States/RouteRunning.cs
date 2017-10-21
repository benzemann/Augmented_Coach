using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RouteRunning : State {

    Rusher player;
    Rigidbody rb;
    float acc;
    float rotationSpeed;
    int currentWaypoint = 0;

    public RouteRunning(Rusher p)
    {
        player = p;
        acc = player.GetComponent<PlayerStats>() != null 
            ? player.GetComponent<PlayerStats>().acceleration 
            : 0;
        rotationSpeed = player.GetComponent<PlayerStats>() != null
            ? player.GetComponent<PlayerStats>().rotationSpeed
            : 0;
        rb = player.GetComponent<Rigidbody>();
        if (rb == null)
        {
            Debug.LogError("There is no rigidbody attached to the object you try to set in RouteRunnging state. GameObject: " + p.name);
        }
        id = StateID.RouteRunningID;
    }

    public override void Enter()
    {

    }

    public override void Execute()
    {
        if(currentWaypoint < player.route.Length)
        {
            var nextWaypoint = player.route[currentWaypoint];
            nextWaypoint.transform.position = new Vector3(
                nextWaypoint.transform.position.x,
                player.transform.position.y,
                nextWaypoint.transform.position.z
                );
            var vecToWaypoint = nextWaypoint.position - player.transform.position;
            var distanceToWaypoint = vecToWaypoint.magnitude;
            if(distanceToWaypoint < 4f)
            {
                currentWaypoint++;
            }
            var rot = RotationCalculator.RotateTowardsPoint(player.transform, 
                nextWaypoint.position, 
                player.transform.forward, 
                player.transform.rotation, 
                rotationSpeed);
            rb.MoveRotation(rot);
        } else
        {
            // Rotate towards end zone
            var rot = RotationCalculator.RotateTowardsPoint(player.transform,
                player.transform.position + (Vector3.forward * 10f),
                player.transform.forward,
                player.transform.rotation,
                rotationSpeed);
            rb.MoveRotation(rot);
        }
        rb.velocity += player.transform.forward * acc * Time.deltaTime;
    }

    public override StateID Reason()
    {
        return base.Reason();
    }

    public override void Exit()
    {

    }
}
