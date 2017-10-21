using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(PlayerStats)), RequireComponent(typeof(Rigidbody))]
public class Rusher : MonoBehaviour {

    public Transform[] route;
    FSM fsm;
    PlayerStats stats;
    Rigidbody rb;
    // For debugging
    [SerializeField]
    StateID currentState;

	// Use this for initialization
	void Start () {
        InitFsm();
        stats = GetComponent<PlayerStats>();
        rb = GetComponent<Rigidbody>();
	}
	
	// Update is called once per frame
	void Update () {
        UpdateFsm();
        RestrictSpeed();
        
	}

    void RestrictSpeed()
    {
        if (rb.velocity.magnitude > (stats.speed * Time.deltaTime))
        {
            rb.velocity = rb.velocity.normalized * stats.speed * Time.deltaTime;
        }
        Debug.Log(rb.velocity.magnitude);
    }

    void InitFsm()
    {
        fsm = new FSM();
        // Adding states to the fsm
        fsm.AddState(new Idle());
        fsm.AddState(new RouteRunning(this));
    }

    void UpdateFsm()
    {
        fsm.UpdateState();
        currentState = fsm.CurrentStateID;
    }
}
