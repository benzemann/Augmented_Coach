using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tackled : State {

    Rigidbody rb;

    public Tackled(GameObject p)
    {
        rb = p.GetComponent<Rigidbody>();
        if (rb == null)
        {
            Debug.LogError("There is no rigidbody attached to the object you try to set in Tackled state. GameObject: " + p.name);
        }
        id = StateID.TackledID;
    }

    public override void Enter()
    {
        rb.velocity = Vector3.zero;
        rb.isKinematic = true;
        rb.freezeRotation = true;
        
    }

    public override void Execute()
    {
        
    }

    public override StateID Reason()
    {
        return base.Reason();
    }

    public override void Exit()
    {

    }
}
