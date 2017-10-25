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
    public Transform reference;
    public Transform field;
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
        currentplay = Instantiate(plays[number], Vector3.zero, Quaternion.identity) as GameObject;
        if(this.transform.parent != null)
        {
            currentplay.transform.SetParent(this.transform.parent);
        }
        currentplay.transform.localPosition = new Vector3(this.transform.localPosition.x, -0.0365f, this.transform.localPosition.z);
        currentplay.transform.localRotation = this.transform.localRotation;
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
        currentplay.transform.SetParent(field);
        for (int i = 1; i < currentplay.transform.childCount; i++)
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
