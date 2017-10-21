using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Celebration : State {

    GameObject player;
    Rigidbody rb;
    Transform endZone;

    public Celebration(GameObject p)
    {
        player = p;
        rb = player.GetComponent<Rigidbody>();
        if (rb == null)
        {
            Debug.LogError("There is no rigidbody attached to the object you try to set in Celecbration state. GameObject: " + p.name);
        }
        id = StateID.Celebration;
    }

    public override void Enter()
    {
        if(ObjectManager.Instance.endZone == null)
        {
            Debug.LogError("There are no endzone assigned in the objectmanager!");
        }
        endZone = ObjectManager.Instance.endZone.transform;
    }

    public override void Execute()
    {
        // Slow down and slowly walk towards endzone center
        if(rb.velocity.magnitude > 10f)
        {
            rb.velocity *= 0.97f;
        } else
        {
            // Rotate towards end zone
            var rot = Helper.RotateTowardsPoint(player.transform,
                endZone.position,
                player.transform.forward,
                player.transform.rotation,
                10f);
            rb.MoveRotation(rot);

            rb.velocity += player.transform.forward * 50f * Time.deltaTime;
        }
        
    }

    public override StateID Reason()
    {
        return base.Reason();
    }

    public override void Exit()
    {

    }
}
