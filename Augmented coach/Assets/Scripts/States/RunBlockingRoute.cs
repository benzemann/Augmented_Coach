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
                stats.rotationSpeed);
            rb.MoveRotation(rot);
        }
        var dir = player.transform.forward;
        dir += Helper.CalculateSidelineAvoidance(player.transform.position, 3f, 5f);
        rb.velocity += dir.normalized * stats.acceleration * Time.deltaTime;
    }

    public override StateID Reason()
    {
        if(currentWaypoint >= route.route.Length)
        {
            return StateID.RunBlockingID;
        }
        var defencePlayers = ObjectManager.Instance.DefencePlayers;

        GameObject closestPlayer = null;
        var closestDistance = float.MaxValue;

        for (int i = 0; i < defencePlayers.Length; i++)
        {
            var dis = Vector3.Distance(player.transform.position, defencePlayers[i].transform.position);
            if (dis < closestDistance && dis < 20f && (defencePlayers[i].GetComponent<Player>().isBlocked == false || defencePlayers.Length == 1))
            {
                closestDistance = dis;
                closestPlayer = defencePlayers[i];
            }
        }

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
