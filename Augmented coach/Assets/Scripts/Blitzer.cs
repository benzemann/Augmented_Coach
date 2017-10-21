using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(PlayerStats)), RequireComponent(typeof(Rigidbody)), RequireComponent(typeof(Route))]
public class Blitzer : Player {

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
        //fsm.AddState(new Idle());
        fsm.AddState(new DefensiveRouteRunning(this.gameObject));
        fsm.AddState(new RunToBallCarrier(this.gameObject));
        fsm.AddState(new Tackling(this.gameObject));
    }

    private void OnDrawGizmos()
    {
        //Gizmos.color = Color.red; 
        //Gizmos.DrawWireSphere(transform.position, stats.tackleRadius);
    }

}
