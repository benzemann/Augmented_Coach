using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefensiveRouteRunning : State {

    GameObject player;
    Route route;
    Rigidbody rb;
    PlayerStats stats;
    int currentWaypoint = 0;

    public DefensiveRouteRunning(GameObject p)
    {
        player = p;
        rb = player.GetComponent<Rigidbody>();
        if (rb == null)
        {
            Debug.LogError("There is no rigidbody attached to the object you try to set in DefensiveRouteRunning state. GameObject: " + p.name);
        }
        stats = player.GetComponent<PlayerStats>();
        if(stats == null)
        {
            Debug.LogError("There is no PlayerStats attached to the object you try to set in DefensiveRouteRinnging state. GameObejct: " + p.name);
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
        // Run route
        player.GetComponent<Player>().RotateTowardsNextWaypointInRoute(route, ref currentWaypoint);
        // Run forward
        var dir = player.transform.forward;
        // Avoid sideline
        dir += Helper.CalculateSidelineAvoidance(player.transform.position, 3f, 5f);
        rb.velocity += dir.normalized * stats.acceleration * Time.deltaTime;
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
