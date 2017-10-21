using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(PlayerStats)), RequireComponent(typeof(Rigidbody)), RequireComponent(typeof(Route))]
public class Rusher : Player {

    // Use this for initialization
    protected override void Init () {
        InitFsm();
    }
	
	// Update is called once per frame
	void Update () {
        UpdateFsm();
        RestrictSpeed();
        
	}


    void InitFsm()
    {
        fsm = new FSM();
        // Adding states to the fsm
        fsm.AddState(new Idle());
        fsm.AddState(new RouteRunning(this.gameObject));
        fsm.AddState(new RunForEndZone(this.gameObject));
        fsm.AddState(new Celebration(this.gameObject));
        fsm.AddState(new Tackled(this.gameObject));
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == ObjectManager.Instance.endZone)
        {
            fsm.ChangeState(StateID.Celebration);
        }
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject == ObjectManager.Instance.endZone)
        {
            fsm.ChangeState(StateID.Celebration);
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.white;
        //Gizmos.DrawWireSphere(transform.position, 40f);
       // var avoidanceVec = Helper.CalculateSidelineAvoidance(this.transform.position, 7f, 1f);
        //Gizmos.DrawLine(this.transform.position, this.transform.position + rb.velocity.normalized * 5f);
        //Gizmos.color = Color.blue;
        //Gizmos.DrawLine(this.transform.position, this.transform.position + avoidanceVec.normalized * 5f);
    }
}
