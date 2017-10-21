using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tackling : State {

    Rigidbody rb;

    public Tackling(GameObject p)
    {
        rb = p.GetComponent<Rigidbody>();
        if (rb == null)
        {
            Debug.LogError("There is no rigidbody attached to the object you try to set in Tackling state. GameObject: " + p.name);
        }
        id = StateID.TacklingID;
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
