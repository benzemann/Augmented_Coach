using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RouteRunning : State {

    GameObject player;
    Route route;
    Rigidbody rb;
    PlayerStats stats;
    int currentWaypoint = 0;

    public RouteRunning(GameObject p)
    {
        // Get components
        player = p;
        rb = player.GetComponent<Rigidbody>();
        if (rb == null)
        {
            Debug.LogError("There is no rigidbody attached to the object you try to set in RouteRunnging state. GameObject: " + p.name);
        }
        stats = player.GetComponent<PlayerStats>();
        if(stats == null)
        {
            Debug.LogError("There is no PlayerStats component attached to the object you try to set in RouteRunning state. GameObject: " + p.name);
        }
        route = player.GetComponent<Route>();
        if(route == null)
        {
            Debug.LogError("There is no Route component attached the gameobject in RouteRunning state. GameObject: " + p.name);
        }
        id = StateID.RouteRunningID;
    }

    public override void Enter()
    {

    }

    public override void Execute()
    {
        // Check if already infront of next waypoint
        if (currentWaypoint < route.route.Length)
        {
            var nextWaypoint = route.route[currentWaypoint];
            if (Helper.DistanceToEndZone(nextWaypoint.position, Player.Side.Offense) > 
                Helper.DistanceToEndZone(player.transform.position, Player.Side.Offense))
            {
                currentWaypoint++;
            }
        }
        // Rotate towards next waypoint in route
        player.GetComponent<Player>().RotateTowardsNextWaypointInRoute(route, ref currentWaypoint);
        var dir = player.transform.forward;
        // Avoid defence players
        dir += Helper.CalculateAvoidanceVector(player.transform, Player.Side.Defence, 20f, 0.5f);
        // Avoid offense players
        dir += Helper.CalculateAvoidanceVector(player.transform, Player.Side.Offense, 10f, 0.25f);
        // Avoid sideline
        dir += Helper.CalculateSidelineAvoidance(player.transform.position, 3f, 5f);
        // Calculate and add to velocity
        rb.velocity += dir.normalized * stats.acceleration * Time.deltaTime;
    }

    public override StateID Reason()
    {
        if(currentWaypoint >= route.route.Length)
        {
            return StateID.RunForEndZoneID;
        }
        return base.Reason();
    }

    public override void Exit()
    {

    }
}
