using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Helper functions for various calculations
/// </summary>
public static class Helper {
    /// <summary>
    /// Get a rotation towards a point in the xz plane
    /// </summary>
    /// <param name="from">The transform that should be rotated</param>
    /// <param name="to">The point it should rotate towards</param>
    /// <param name="forward">The forward vector</param>
    /// <param name="currentRotation">Current rotation</param>
    /// <param name="speed">Rotation speed</param>
    /// <returns></returns>
	public static Quaternion RotateTowardsPoint(Transform from, Vector3 to, Vector3 forward, Quaternion currentRotation, float speed)
    {
        Vector3 targetDir = to - from.position;
        Vector3 localTarget = from.InverseTransformPoint(to);

        float angle = Mathf.Atan2(localTarget.x, localTarget.z) * Mathf.Rad2Deg;

        Vector3 eulerAngleVelocity = new Vector3(0, angle, 0);
        Quaternion deltaRotation = Quaternion.Euler(eulerAngleVelocity * Time.deltaTime * speed);
        return currentRotation * deltaRotation;
    }

    /// <summary>
    /// Calculate an avoidance vector from other players
    /// </summary>
    /// <param name="player">The player that needs to avoid others</param>
    /// <param name="side">The side the player needs to avoid</param>
    /// <param name="radius">The radius of influence</param>
    /// <param name="weight">The weight of the calculation. Should be between 0 and 1.</param>
    /// <returns></returns>
    public static Vector3 CalculateAvoidanceVector(Transform player, Player.Side side, float radius, float weight)
    {
        GameObject[] players = new GameObject[0];
        switch (side)
        {
            case Player.Side.Offense:
                players = ObjectManager.Instance.OffensePlayers;
                break;
            case Player.Side.Defence:
                players = ObjectManager.Instance.DefencePlayers;
                break;
            default:
                break;
        }

        var avoidanceVec = Vector3.zero;
        var numberOfClosePlayers = 0;
        for(int i = 0; i < players.Length; i++)
        {
            if (players[i] == player.gameObject)
                continue;
            var vec = (player.position + player.forward * 15f) - players[i].transform.position;
            var dis = vec.magnitude;
            if(dis <= radius)
            {
                avoidanceVec += (1f - (dis/radius)) * vec;
                numberOfClosePlayers++;
            }
        }
        if(numberOfClosePlayers > 0)
        {
            avoidanceVec /= numberOfClosePlayers;
        }
        return avoidanceVec.normalized * weight;
    }
    /// <summary>
    /// Calculate an avoidance vector from the sidelines
    /// </summary>
    /// <param name="from">From position</param>
    /// <param name="radius">Radius of influence</param>
    /// <param name="weight">Weight of calculation. Should be between 0 and 1.</param>
    /// <returns></returns>
    public static Vector3 CalculateSidelineAvoidance(Vector3 from, float radius, float weight)
    {
        var distanceToSideline1 = Mathf.Abs(ObjectManager.Instance.sidelineMarker1.position.x - from.x);
        if(distanceToSideline1 <= radius)
        {
            return - new Vector3(ObjectManager.Instance.sidelineMarker1.position.x - from.x, 0f, 0f).normalized * (1f - (distanceToSideline1 / radius)) * weight;
        }
        var distanceToSideline2 = Mathf.Abs(ObjectManager.Instance.sidelineMarker2.position.x - from.x);
        if (distanceToSideline2 <= radius)
        {
            return - new Vector3(ObjectManager.Instance.sidelineMarker1.position.x - from.x, 0f, 0f).normalized * (1f - (distanceToSideline1 / radius)) * weight;
        }
        return Vector3.zero;
    }
    /// <summary>
    /// Calculate the distance to the endzone from a given position and side.
    /// </summary>
    /// <param name="pos">From this position</param>
    /// <param name="side">Whether it is for offense or defence endzone</param>
    /// <returns></returns>
    public static float DistanceToEndZone(Vector3 pos, Player.Side side)
    {
        // Get the endzone position
        Vector3 endzonePos = Vector3.zero;
        switch (side)
        {
            case Player.Side.Offense:
                endzonePos = ObjectManager.Instance.endZoneOffense.transform.position;
                break;
            case Player.Side.Defence:
                endzonePos = ObjectManager.Instance.endZoneDefence.transform.position;
                break;
            default:
                break;
        }
        var vecToEndzone = endzonePos - pos;
        // The field is always placed down the z axis
        return Mathf.Abs(vecToEndzone.z);
    }

    /// <summary>
    /// Get the closest player from either defence or offense.
    /// </summary>
    /// <param name="pos">The from position</param>
    /// <param name="side">The side of players to search for</param>
    /// <param name="radius">The radius to search in.</param>
    /// <param name="checkIsBlocked">whether it should filter out players that is blocked by others.</param>
    /// <returns></returns>
    public static GameObject GetClosestPlayer(Vector3 pos, Player.Side side, float radius, bool checkIsBlocked = false)
    {
        var players = new GameObject[0];
        switch (side)
        {
            case Player.Side.Offense:
                players = ObjectManager.Instance.OffensePlayers;
                break;
            case Player.Side.Defence:
                players = ObjectManager.Instance.DefencePlayers;
                break;
            default:
                break;
        }

        var closestDistance = float.MaxValue;
        GameObject closePlayer = null;
        for (int i = 0; i < players.Length; i++)
        {
            var dis = Vector3.Distance(pos, players[i].transform.position);
            if (dis < closestDistance && dis < radius && 
                (checkIsBlocked == false || (players[i].GetComponent<Player>().isBlocked == true || players.Length == 1)))
            {
                closestDistance = dis;
                closePlayer = players[i];
            }
        }
        if(closePlayer != null)
        {
            return closePlayer;
        }
        return null;
    }
}
