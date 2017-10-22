using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RunForEndZone : State {

    GameObject player;
    Rigidbody rb;
    PlayerStats stats;

    public RunForEndZone(GameObject p)
    {
        player = p;
        stats = player.GetComponent<PlayerStats>();
        if(stats == null)
        {
            Debug.LogError("There is no PlayerStats attached to the object you try to set in RunForEndZone state. GameObject: " + p.name);
        }
        rb = player.GetComponent<Rigidbody>();
        if (rb == null)
        {
            Debug.LogError("There is no rigidbody attached to the object you try to set in RunForEndZone state. GameObject: " + p.name);
        }
        id = StateID.RunForEndZoneID;
    }

    public override void Enter()
    {

    }

    public override void Execute()
    {
        // Rotate towards end zone
        var rot = Helper.RotateTowardsPoint(player.transform,
            player.transform.position + (Vector3.forward * 10f), // TODO: Should be better than just out of z axis!
            player.transform.forward,
            player.transform.rotation,
            stats.rotationSpeed);
        rb.MoveRotation(rot);
        // Move forward
        var dir = player.transform.forward;
        // Avoid defence players
        dir += Helper.CalculateAvoidanceVector(player.transform, Player.Side.Defence, 40f, 0.5f);
        // Avoid players from same team
        dir += Helper.CalculateAvoidanceVector(player.transform, Player.Side.Offense, 10f, 0.25f);
        // Avoid sideline
        dir += Helper.CalculateSidelineAvoidance(player.transform.position, 3f, 5f);
        // Calculate velocity
        rb.velocity += dir.normalized * stats.acceleration * Time.deltaTime;
    }

    public override StateID Reason()
    {
        return base.Reason();
    }

    public override void Exit()
    {

    }
}
