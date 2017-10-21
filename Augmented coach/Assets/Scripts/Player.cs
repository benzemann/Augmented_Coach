using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PlayerStats)), RequireComponent(typeof(Rigidbody))]
public class Player : MonoBehaviour {

    public enum Side { Offense, Defence }

    protected FSM fsm;
    protected PlayerStats stats;
    protected Rigidbody rb;

    public GameObject target;
    public bool isBlocked;

    [SerializeField]
    public Side side;

    // For debugging
    [SerializeField]
    protected StateID currentState;

    private void Start()
    {
        stats = GetComponent<PlayerStats>();
        rb = GetComponent<Rigidbody>();
        switch (side)
        {
            case Side.Offense:
                ObjectManager.Instance.AddOffensePlayer(this.gameObject);
                break;
            case Side.Defence:
                ObjectManager.Instance.AddDefencePlayer(this.gameObject);
                break;
            default:
                break;
        }
        isBlocked = false;
        Init();
    }

    protected virtual void Init()
    {
        
    }

    private void LateUpdate()
    {
        isBlocked = false;
    }

    protected void UpdateFsm()
    {
        fsm.UpdateState();
        currentState = fsm.CurrentStateID;
    }

    protected void RestrictSpeed()
    {
        if (rb.velocity.magnitude > (stats.speed * Time.deltaTime))
        {
            rb.velocity = rb.velocity.normalized * stats.speed * Time.deltaTime;
        }
        //Debug.Log(rb.velocity.magnitude);
    }

    public void GetTackled()
    {
        fsm.ChangeState(StateID.TackledID);
    }

    public void Tackle(Player player)
    {
        player.GetTackled();
    }

    public void GetBlocked(Transform player, Vector3 dir, int strength)
    {
        var strenghtScore = (stats.strength - strength) + Random.Range(-3, 3);
        if(strenghtScore > 0)
        {
            // Defender wins
            var vecToAttacker = player.position - this.transform.position;
            if(player.GetComponent<Rigidbody>() != null)
            {
                player.GetComponent<Rigidbody>().velocity += vecToAttacker * (25f * stats.strength) * Time.deltaTime;
            }
        } else
        {
            // Attacker wins
            rb.velocity *= 0.2f;
            rb.velocity += dir.normalized * (25f * strength) * Time.deltaTime;
        }

    }

}
