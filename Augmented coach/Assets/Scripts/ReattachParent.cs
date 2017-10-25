using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReattachParent : MonoBehaviour {

    [SerializeField]
    Transform field;

    [SerializeField]
    Transform referenceTransform;
	// Use this for initialization
	void Start () {
    }
	
	// Update is called once per frame
	void Update () {
        //referenceTransform.GetComponent<Renderer>().enabled = false;

    }

    public void Snap()
    {
        this.transform.SetParent(field);
        this.transform.position = new Vector3(this.transform.position.x, referenceTransform.position.y, this.transform.position.z);
        this.transform.rotation = referenceTransform.rotation;
        
    }
}
