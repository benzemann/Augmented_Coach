using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RunToBallCarrier : State {

    GameObject player;
    Rigidbody rb;
    float acc;
    float rotationSpeed;
    float tackleRadius;

    public RunToBallCarrier(GameObject p)
    {
        player = p;
        acc = player.GetComponent<PlayerStats>() != null
            ? player.GetComponent<PlayerStats>().acceleration
            : 0;
        rotationSpeed = player.GetComponent<PlayerStats>() != null
            ? player.GetComponent<PlayerStats>().rotationSpeed
            : 0;
        tackleRadius = player.GetComponent<PlayerStats>() != null
            ? player.GetComponent<PlayerStats>().tackleRadius
            : 0;
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
                rotationSpeed);
            rb.MoveRotation(rot);

            rb.velocity += player.transform.forward * acc * Time.deltaTime;
        }
    }

    public override StateID Reason()
    {
        var distance = (ObjectManager.Instance.ballCarrier.transform.position -
            player.transform.position).magnitude;

        if(distance < tackleRadius)
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
