using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectManager : Singleton<ObjectManager> {

    public GameObject endZoneOffense;
    public GameObject endZoneDefence;
    public GameObject ballCarrier;
    public Transform sidelineMarker1;
    public Transform sidelineMarker2;
    List<GameObject> defencePlayers;
    public GameObject[] DefencePlayers { get { return defencePlayers.ToArray(); } }
    List<GameObject> offensePlayers;
    public GameObject[] OffensePlayers { get { return offensePlayers.ToArray(); } }
    public GameObject[] AllPlayers {
        get
        {
            var allPlayers = new List<GameObject>();
            allPlayers.AddRange(defencePlayers);
            allPlayers.AddRange(offensePlayers);
            return allPlayers.ToArray();
        }
    }
    
    // Use this for initialization
    void Awake () {
        defencePlayers = new List<GameObject>();
        offensePlayers = new List<GameObject>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}
    
    public void AddOffensePlayer(GameObject player)
    {
        if (offensePlayers.Contains(player))
        {
            Debug.LogWarning("Trying to add already added offense player!");
            return;
        }
        offensePlayers.Add(player);
    }

    public void RemoveOffensePlayer(GameObject player)
    {
        if (offensePlayers.Contains(player))
        {
            offensePlayers.Remove(player);
            return;
        }
        Debug.LogWarning("Trying to remove offense player that has not been added!");
    }

    public void AddDefencePlayer(GameObject player)
    {
        if (defencePlayers.Contains(player))
        {
            Debug.LogWarning("Trying to add already added defence player!");
            return;
        }
        defencePlayers.Add(player);
    }

    public void RemoveDefencePlayer(GameObject player)
    {
        if (defencePlayers.Contains(player))
        {
            defencePlayers.Remove(player);
            return;
        }
        Debug.LogWarning("Trying to remove defence player that has not been added!");
    }
}
