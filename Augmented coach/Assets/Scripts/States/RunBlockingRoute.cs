using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RunBlockingRoute : State {

    GameObject player;
    Route route;
    Rigidbody rb;
    PlayerStats stats;
    int currentWaypoint = 0;

    public RunBlockingRoute(GameObject p)
    {
        player = p;
        rb = player.GetComponent<Rigidbody>();
        if (rb == null)
        {
            Debug.LogError("There is no rigidbody attached to the object you try to set in RouteRunnging state. GameObject: " + p.name);
        }
        route = player.GetComponent<Route>();
        if (route == null)
        {
            Debug.LogError("There is no Route component attached the gameobject in RouteRunning state. GameObject: " + p.name);
        }
        stats = player.GetComponent<PlayerStats>();
        if (stats == null)
        {
            Debug.LogError("There is no PlayerStats component attached to the gameobject in RunBlockingRoute state. GameObject: " + p.name);
        }
        id = StateID.RunBlockingRouteID;
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
        // Rotate towards next waypoint
        player.GetComponent<Player>().RotateTowardsNextWaypointInRoute(route, ref currentWaypoint);
        // Move forward
        var dir = player.transform.forward;
        // Avoid sideline
        dir += Helper.CalculateSidelineAvoidance(player.transform.position, 3f, 5f);
        // Calculate velocity
        rb.velocity += dir.normalized * stats.acceleration * Time.deltaTime;
    }

    public override StateID Reason()
    {
        // End of route
        if(currentWaypoint >= route.route.Length)
        {
            return StateID.RunBlockingID;
        }
        GameObject closestPlayer = Helper.GetClosestPlayer(player.transform.position, Player.Side.Defence, 15f, true);
        // Close unblocked player, block him
        if (closestPlayer != null)
        {
            player.GetComponent<Player>().target = closestPlayer;
            closestPlayer.GetComponent<Player>().isBlocked = true;
            return StateID.BlockID;
        }

        return base.Reason();
    }

    public override void Exit()
    {

    }
}
