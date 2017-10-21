using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RunBlocking : State {
    GameObject player;
    Rigidbody rb;
    PlayerStats stats;

    public RunBlocking(GameObject p)
    {
        player = p;
        rb = player.GetComponent<Rigidbody>();
        if (rb == null)
        {
            Debug.LogError("There is no rigidbody attached to the object you try to set in RouteRunnging state. GameObject: " + p.name);
        }
        stats = player.GetComponent<PlayerStats>();
        if (stats == null)
        {
            Debug.LogError("There is no PlayerStats component attached to the gameobject in RunBlockingRoute state. GameObject: " + p.name);
        }
        id = StateID.RunBlockingID;
    }

    public override void Enter()
    {

    }

    public override void Execute()
    {
        var vecToBallCarrier = player.transform.position - ObjectManager.Instance.ballCarrier.transform.position;
        var targetSpot = ObjectManager.Instance.ballCarrier.transform.position + (vecToBallCarrier.normalized * 7f);

        var ezToTarget = targetSpot - ObjectManager.Instance.endZone.transform.position;
        var ezToPlayer = player.transform.position - ObjectManager.Instance.endZone.transform.position;
        var dir = Helper.CalculateSidelineAvoidance(player.transform.position, 3f, 5f);
        dir += player.transform.forward;
        if ( (new Vector3(0f, 0f, ezToTarget.z).magnitude < new Vector3(0f, 0f, ezToPlayer.z).magnitude))
        {
            var vecToTargetSpot = targetSpot - player.transform.position;
            if(vecToTargetSpot.magnitude < 5f)
            {
                rb.velocity *= 0.5f;
                dir *= 0.5f;
            } else
            {

                var rot = Helper.RotateTowardsPoint(player.transform,
                    targetSpot,
                    player.transform.forward,
                    player.transform.rotation,
                    stats.rotationSpeed);
                rb.MoveRotation(rot);
            }

        } else
        {
            var rot = Helper.RotateTowardsPoint(player.transform,
                    player.transform.position + Vector3.forward,// TODO: Should be implemented better
                    player.transform.forward,
                    player.transform.rotation,
                    stats.rotationSpeed);
            rb.MoveRotation(rot);
           // rb.velocity *= 0.90f;
        }
        
        if(Vector3.Distance(player.transform.position, ObjectManager.Instance.ballCarrier.transform.position) < 5f)
        {
            dir += (player.transform.position - ObjectManager.Instance.ballCarrier.transform.position).normalized * 5f;
        }

        rb.velocity += dir.normalized * stats.acceleration * Time.deltaTime;
    }

    public override StateID Reason()
    {

        var defencePlayers = ObjectManager.Instance.DefencePlayers;

        GameObject closestPlayer = null;
        var closestDistance = float.MaxValue;

        for(int i = 0; i < defencePlayers.Length; i++)
        {
            var dis = Vector3.Distance(player.transform.position, defencePlayers[i].transform.position);
            if (dis < closestDistance && dis < 20f && (defencePlayers[i].GetComponent<Player>().isBlocked == false || defencePlayers.Length == 1))
            {
                closestDistance = dis;
                closestPlayer = defencePlayers[i];
            }
        }
            
        if (closestPlayer != null)
        {
            player.GetComponent<Player>().target = closestPlayer;
            closestPlayer.GetComponent<Player>().isBlocked = true;
            return StateID.BlockID;
        }
        

        return base.Reason();
    }

    public override void Exit()
    {

    }
}
