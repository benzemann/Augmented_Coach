using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RunForEndZone : State {

    GameObject player;
    Rigidbody rb;
    float acc;
    float rotationSpeed;

    public RunForEndZone(GameObject p)
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
            rotationSpeed);
        rb.MoveRotation(rot);

        var dir = player.transform.forward;
        dir += Helper.CalculateAvoidanceVector(player.transform, Player.Side.Defence, 40f, 0.5f);
        dir += Helper.CalculateAvoidanceVector(player.transform, Player.Side.Offense, 10f, 0.25f);
        dir += Helper.CalculateSidelineAvoidance(player.transform.position, 3f, 5f);
        rb.velocity += dir.normalized * acc * Time.deltaTime;
    }

    public override StateID Reason()
    {
        return base.Reason();
    }

    public override void Exit()
    {

    }
}
