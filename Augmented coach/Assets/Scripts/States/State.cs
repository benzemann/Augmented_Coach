using UnityEngine;
using System.Collections;

public enum StateID
{
    NullStateID = 0, // Non existing state
    IdleID = 1,
    RouteRunningID = 2,
    RunForEndZoneID = 3,
    Celebration = 4,
    RunToBallCarrierID = 5,
    TackledID = 6,
    TacklingID = 7,
    RunBlockingID = 8,
    RunBlockingRouteID = 9,
    BlockID = 10,
    DefensiveRouteRunning = 11
}

public abstract class State
{

    #region protected
    protected StateID id;
    #endregion
    #region public
    public StateID Id { get { return id; } }
    public float timeInState { get; set; }
    #endregion

    /// <summary>
    /// Is called when entering the state.
    /// </summary>
    public virtual void Enter() { }

    /// <summary>
    /// Is called when changing from this state to another.
    /// </summary>
    public virtual void Exit() { }

    /// <summary>
    /// Is called every frame when this state is the current state.
    /// </summary>
    public virtual void Execute() { }

    /// <summary>
    /// Checks whether this state should be changed to another.
    /// </summary>
    public virtual StateID Reason() { return Id; }

}
