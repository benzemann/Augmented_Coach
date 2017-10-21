using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefensiveRouteRunning : State {

    GameObject player;
    Route route;
    Rigidbody rb;
    float acc;
    float rotationSpeed;
    int currentWaypoint = 0;

    public DefensiveRouteRunning(GameObject p)
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
            Debug.LogError("There is no rigidbody attached to the object you try to set in DefensiveRouteRunning state. GameObject: " + p.name);
        }
        route = player.GetComponent<Route>();
        if (route == null)
        {
            Debug.LogError("There is no Route component attached the gameobject in DefensiveRouteRunning state. GameObject: " + p.name);
        }
        id = StateID.DefensiveRouteRunning;
    }

    public override void Enter()
    {

    }

    public override void Execute()
    {
        if (timeInState <= 5f)
        {
            return;
        }
        if (currentWaypoint < route.route.Length)
        {
            var nextWaypoint = route.route[currentWaypoint];
            // Check if already infront of next waypoint
            var ezToWp = nextWaypoint.position - ObjectManager.Instance.endZone.transform.position;
            var ezToPlayer = player.transform.position - ObjectManager.Instance.endZone.transform.position;
            if ((new Vector3(0f, 0f, ezToWp.z).magnitude > new Vector3(0f, 0f, ezToPlayer.z).magnitude))
            {
                currentWaypoint++;
            }
        }
        // Run route
        if (currentWaypoint < route.route.Length)
        {
            var nextWaypoint = route.route[currentWaypoint];
            nextWaypoint.transform.position = new Vector3(
                nextWaypoint.transform.position.x,
                player.transform.position.y,
                nextWaypoint.transform.position.z
            );
            var vecToWaypoint = nextWaypoint.position - player.transform.position;
            var distanceToWaypoint = vecToWaypoint.magnitude;
            if (distanceToWaypoint < 4f)
            {
                currentWaypoint++;
            }
            var rot = Helper.RotateTowardsPoint(player.transform,
                nextWaypoint.position,
                player.transform.forward,
                player.transform.rotation,
                rotationSpeed);
            rb.MoveRotation(rot);
        }
        var dir = player.transform.forward;

        dir += Helper.CalculateSidelineAvoidance(player.transform.position, 3f, 5f);
        rb.velocity += dir.normalized * acc * Time.deltaTime;
    }

    public override StateID Reason()
    {
        if (currentWaypoint >= route.route.Length)
        {
            return StateID.RunToBallCarrierID;
        }
        return base.Reason();
    }

    public override void Exit()
    {

    }
}
