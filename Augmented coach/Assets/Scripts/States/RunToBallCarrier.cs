using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RunToBallCarrier : State {

    GameObject player;
    Rigidbody rb;
    PlayerStats stats;

    public RunToBallCarrier(GameObject p)
    {
        player = p;
        stats = player.GetComponent<PlayerStats>();
        if (stats == null)
        {
            Debug.LogError("There is no PlayerStats attached to the object you try to set in RunToBallCarrier state. GameObject: " + p.name);
        }
        rb = player.GetComponent<Rigidbody>();
        if (rb == null)
        {
            Debug.LogError("There is no rigidbody attached to the object you try to set in RunToBallCarrier state. GameObject: " + p.name);
        }
        id = StateID.RunToBallCarrierID;
    }

    public override void Enter()
    {

    }

    public override void Execute()
    {
        if(ObjectManager.Instance.ballCarrier != null)
        {
            var vecToBallCarrier = ObjectManager.Instance.ballCarrier.transform.position - player.transform.position;
            var distance = vecToBallCarrier.magnitude;
            // Calcualte position to run towards. If far away run forward ball carrier to catch up
            var targetPos = ObjectManager.Instance.ballCarrier.transform.position +
                    ObjectManager.Instance.ballCarrier.transform.forward * 14f;
            if (distance > 30f)
            {
                targetPos = ObjectManager.Instance.ballCarrier.transform.position +
                    ((ObjectManager.Instance.ballCarrier.transform.forward * 0.5f) +
                    (Vector3.forward * 0.5f)).normalized * 30f;
            }
            else if (distance < 15f)
            {
                targetPos = ObjectManager.Instance.ballCarrier.transform.position;
            }

            // Rotate towards end zone
            var rot = Helper.RotateTowardsPoint(player.transform,
                targetPos,
                player.transform.forward,
                player.transform.rotation,
                stats.rotationSpeed);
            rb.MoveRotation(rot);
            // Calculate velocity
            rb.velocity += player.transform.forward * stats.acceleration * Time.deltaTime;
        }
    }

    public override StateID Reason()
    {
        var distance = (ObjectManager.Instance.ballCarrier.transform.position -
            player.transform.position).magnitude;
        // Tackle player if close enough
        if(distance < stats.tackleRadius)
        {
            player.GetComponent<Player>().Tackle(ObjectManager.Instance.ballCarrier.GetComponent<Player>());
            return StateID.TacklingID;
        }

        return base.Reason();
    }

    public override void Exit()
    {

    }

}
