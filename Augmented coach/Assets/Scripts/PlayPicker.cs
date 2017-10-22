using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(Route))]
public class PlayPicker : MonoBehaviour {

    [SerializeField]
    int playNumber;

    [SerializeField]
    GameObject[] plays;
    GameObject currentplay;

	// Use this for initialization
	void Start () {
        // Start with a random play
        ChangePlay(Random.Range(0, plays.Length));
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void ChangePlay(int number)
    {
        if(number >= plays.Length)
        {
            return;
        }
        if(currentplay != null)
        {
            Destroy(currentplay);
        }
        currentplay = Instantiate(plays[number], new Vector3(this.transform.position.x, 0.02f, this.transform.position.z), Quaternion.identity) as GameObject;
        playNumber = number;
        AssignRoute(currentplay);
    }

    public void AssignRoute(GameObject play)
    {
        var waypointsParent = play.transform.GetChild(0);
        if(waypointsParent == null)
        {
            return;
        }
        var route = GetComponent<Route>();
        var newRoute = new List<Transform>();
        for(int i = 0; i < waypointsParent.childCount; i++)
        {
            newRoute.Add(waypointsParent.GetChild(i));
        }
        route.route = newRoute.ToArray();
    }

    public void HidePlayIndicator()
    {
        for(int i = 1; i < currentplay.transform.childCount; i++)
        {
            currentplay.transform.GetChild(i).GetComponent<Renderer>().enabled = false;
        }
    }

    private void OnMouseDown()
    {
        if(GameMng.Instance.PlayStarted == false)
        {
            playNumber++;
            if (playNumber >= plays.Length)
            {
                playNumber = 0;
            }
            ChangePlay(playNumber);
        }
    }
}
