using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameMng : Singleton<GameMng> {

    public bool PlayStarted { get; set; }

    // Use this for initialization
    void Start () {
        PlayStarted = false;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void Snap()
    {
        PlayStarted = true;
        // Trigger snap event for all players
        var allPlayers = ObjectManager.Instance.AllPlayers;
        for(int i = 0; i < allPlayers.Length; i++)
        {
            allPlayers[i].GetComponent<Player>().Snap();
        }
    }
}
