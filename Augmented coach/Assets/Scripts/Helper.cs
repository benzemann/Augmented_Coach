using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Helper {

	public static Quaternion RotateTowardsPoint(Transform from, Vector3 to, Vector3 forward, Quaternion currentRotation, float speed)
    {
        Vector3 targetDir = to - from.position;
        Vector3 localTarget = from.transform.InverseTransformPoint(to);

        float angle = Mathf.Atan2(localTarget.x, localTarget.z) * Mathf.Rad2Deg;

        Vector3 eulerAngleVelocity = new Vector3(0, angle, 0);
        Quaternion deltaRotation = Quaternion.Euler(eulerAngleVelocity * Time.deltaTime * speed);
        return currentRotation * deltaRotation;
    }

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
}
