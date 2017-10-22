using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(PlayerStats)), RequireComponent(typeof(Rigidbody)), RequireComponent(typeof(Route))]
public class Guard : Player {
    


    // Use this for initialization
    protected override void Init()
    {
        InitFsm();
    }

    // Update is called once per frame
    void Update()
    {
        UpdateFsm();
        RestrictSpeed();
    }

    void InitFsm()
    {
        fsm = new FSM();
        // Adding states to the fsm
        fsm.AddState(new Idle());
        fsm.AddState(new RunBlockingRoute(this.gameObject));
        fsm.AddState(new RunBlocking(this.gameObject));
        fsm.AddState(new Block(this.gameObject));
        
    }

    public override void Snap()
    {
        fsm.ChangeState(StateID.RunBlockingRouteID);
        base.Snap();
    }

}
