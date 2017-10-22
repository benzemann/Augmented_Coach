using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RunBlocking : State {

    GameObject player;
    Rigidbody rb;
    PlayerStats stats;

    public RunBlocking(GameObject p)
    {
        player = p;
        rb = player.GetComponent<Rigidbody>();
        if (rb == null)
        {
            Debug.LogError("There is no rigidbody attached to the object you try to set in RouteRunnging state. GameObject: " + p.name);
        }
        stats = player.GetComponent<PlayerStats>();
        if (stats == null)
        {
            Debug.LogError("There is no PlayerStats component attached to the gameobject in RunBlockingRoute state. GameObject: " + p.name);
        }
        id = StateID.RunBlockingID;
    }

    public override void Enter()
    {

    }

    public override void Execute()
    {
        var vecToBallCarrier = player.transform.position - ObjectManager.Instance.ballCarrier.transform.position;
        // Calculate the spot which the player wants to go and protect the ball carrier
        var targetSpot = ObjectManager.Instance.ballCarrier.transform.position + (ObjectManager.Instance.ballCarrier.transform.forward * 20f);
        // Rotate towards target pos
        var rot = Helper.RotateTowardsPoint(player.transform,
                    targetSpot,
                    player.transform.forward,
                    player.transform.rotation,
                    stats.rotationSpeed);
        rb.MoveRotation(rot);
        // Avoid sideline
        var dir = Helper.CalculateSidelineAvoidance(player.transform.position, 3f, 5f);
        // Move forward
        dir += player.transform.forward;
        // Keep distance between player and ball carrier
        if (Vector3.Distance(player.transform.position, ObjectManager.Instance.ballCarrier.transform.position) < 5f)
        {
            dir += (player.transform.position - ObjectManager.Instance.ballCarrier.transform.position).normalized * 5f;
        }
        // Calculate velocity
        rb.velocity += dir.normalized * stats.acceleration * Time.deltaTime;
    }

    public override StateID Reason()
    {

        var defencePlayers = ObjectManager.Instance.DefencePlayers;

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
