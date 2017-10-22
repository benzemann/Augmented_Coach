﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Block : State {

    GameObject player;
    GameObject target;
    Rigidbody rb;
    PlayerStats stats;

    public Block(GameObject p)
    {
        player = p;
        rb = player.GetComponent<Rigidbody>();
        if (rb == null)
        {
            Debug.LogError("There is no rigidbody attached to the object you try to set in Block state. GameObject: " + p.name);
        }
        stats = player.GetComponent<PlayerStats>();
        if (stats == null)
        {
            Debug.LogError("There is no PlayerStats component attached to the gameobject in Block state. GameObject: " + p.name);
        }
        
        id = StateID.BlockID;
    }

    public override void Enter()
    {
        target = player.GetComponent<Player>().target;
    }

    public override void Execute()
    {
        
        var vecToTarget = target.transform.position - player.transform.position;
        // If far away run forward player, else run towards
        if(vecToTarget.magnitude > 10f)
        {
            var rot = Helper.RotateTowardsPoint(player.transform,
                target.transform.position + target.transform.forward * 9f,
                player.transform.forward,
                player.transform.rotation,
                stats.rotationSpeed);
            rb.MoveRotation(rot);
        } else
        {
            var rot = Helper.RotateTowardsPoint(player.transform,
                target.transform.position,
                player.transform.forward,
                player.transform.rotation,
                stats.rotationSpeed);
            rb.MoveRotation(rot);
        }
        // Try to block if close enough
        if (vecToTarget.magnitude < stats.blockRadius)
        {
            // Try to shove away from ball carrier
            var vecFromBallToTarget = target.transform.position - ObjectManager.Instance.ballCarrier.transform.position;
            rb.velocity *= 0.95f;
            target.GetComponent<Player>().GetBlocked(player.transform, vecFromBallToTarget.normalized, stats.strength);
        }
        // Move forward
        var dir = player.transform.forward;
        // Avoid sideline
        dir += Helper.CalculateSidelineAvoidance(player.transform.position, 3f, 5f);
        // Calculate velocity
        rb.velocity += dir.normalized * stats.acceleration * Time.deltaTime;
    }

    public override StateID Reason()
    {
        // If target too far away forget about him
        if(target == null || Vector3.Distance(target.transform.position, player.transform.position) > 25f)
        {
            return StateID.RunBlockingID;
        }
        // If target is blocked and a bit far away, forget about him
        if(target.GetComponent<Player>().isBlocked == true && 
            Vector3.Distance(target.transform.position, ObjectManager.Instance.ballCarrier.transform.position) > 10f)
        {
            return StateID.RunBlockingID;
        }
        // Flag the target as blocked
        target.GetComponent<Player>().isBlocked = true;

        return base.Reason();
    }

    public override void Exit()
    {

    }
}
