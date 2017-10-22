using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PlayerStats)), RequireComponent(typeof(Rigidbody))]
public class Player : MonoBehaviour {
    
    public enum Side { Offense, Defence }

    protected FSM fsm;
    protected PlayerStats stats;
    protected Rigidbody rb;
    
    [HideInInspector]
    public GameObject target;
    //[HideInInspector]
    public bool isBlocked;

    [Tooltip("The side of the player. Defence or offense.")]
    public Side side;

    // For debugging
    [SerializeField, Tooltip("Should bot be changed, can see current state for debugging purposes.")]
    protected StateID currentState;

    private void Start()
    {
        // Get components add to object manager and set variables
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
        // Call init function. 
        Init();

    }

    public virtual void Snap()
    {
        // Use this to define what will happen when the ball is snapped
        // base.Snap() should always be called
        var playPicker = GetComponent<PlayPicker>();
        if(playPicker != null)
        {
            playPicker.HidePlayIndicator();
        }
    }

    protected virtual void Init()
    {
        // Use this function in subclasses call code at start
    }

    private void LateUpdate()
    {
        // After end of frame set is blocked to false
        isBlocked = false;
    }

    /// <summary>
    /// Updates the FSM system
    /// </summary>
    protected void UpdateFsm()
    {
        // Update the fsm system
        fsm.UpdateState();
        currentState = fsm.CurrentStateID;
    }

    /// <summary>
    /// Restricts the velocity of the player to the speed stat.
    /// </summary>
    protected void RestrictSpeed()
    {
        if (rb.velocity.magnitude > (stats.speed * Time.deltaTime))
        {
            rb.velocity = rb.velocity.normalized * stats.speed * Time.deltaTime;
        }
    }

    /// <summary>
    /// Changes the state of the player to tackled
    /// </summary>
    public void GetTackled()
    {
        fsm.ChangeState(StateID.TackledID);
    }

    /// <summary>
    /// Tackle another player
    /// </summary>
    /// <param name="player">The player that should be tackled</param>
    public void Tackle(Player player)
    {
        player.GetTackled();
        // TODO: There should be some functionality for 
    }

    /// <summary>
    /// Call this function to try to block a player. The strength determines who has the biggest chance of winning.
    /// </summary>
    /// <param name="player">The player that tries to block</param>
    /// <param name="dir">The direction the blocking player wants to push this player.</param>
    /// <param name="strength">The strength attribute of the blocking player</param>
    public void GetBlocked(Transform player, Vector3 dir, int strength)
    {
        // Calculate strenght score, positive defender wins, 0 or negative attacker wins
        var strenghtScore = (stats.strength - strength) + Random.Range(-3, 4);
        if(strenghtScore > 0)
        {
            // Defender wins
            var vecToAttacker = player.position - this.transform.position;
            if(player.GetComponent<Rigidbody>() != null)
            {
                // Push attacker away from defender
                player.GetComponent<Rigidbody>().velocity += vecToAttacker * (25f * stats.strength) * Time.deltaTime;
            }
        } else
        {
            // Attacker wins, push the defender en direction of dir
            rb.velocity *= 0.2f;
            rb.velocity += dir.normalized * (25f * strength) * Time.deltaTime;
        }

    }

    /// <summary>
    /// Rotate towards next waypoint in route.
    /// </summary>
    /// <param name="route">The route</param>
    /// <param name="currentWaypoint">The current waypoint</param>
    public void RotateTowardsNextWaypointInRoute(Route route, ref int currentWaypoint)
    {
        if (currentWaypoint < route.route.Length)
        {
            // Calculate the position of next waypoint
            var nextWaypoint = route.route[currentWaypoint];
            nextWaypoint.transform.position = new Vector3(
                nextWaypoint.transform.position.x,
                this.transform.position.y,
                nextWaypoint.transform.position.z
            );
            // vector and distance to next waypoint
            var vecToWaypoint = nextWaypoint.position - this.transform.position;
            var distanceToWaypoint = vecToWaypoint.magnitude;
            if (distanceToWaypoint < 4f)
            {
                // Close to next wapypoint go to next
                currentWaypoint++;
            }
            // Calculate rotation and rotate the player
            var rotation = Helper.RotateTowardsPoint(this.transform,
                nextWaypoint.position,
                this.transform.forward,
                this.transform.rotation,
                stats.rotationSpeed);
            rb.MoveRotation(rotation);
        }
    }

}
