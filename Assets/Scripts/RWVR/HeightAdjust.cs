using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeightAdjust : MonoBehaviour {

    public int floorHeight = 100;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void FixedUpdate () {
        if(floorHeight<=-20)
        {
            floorHeight = -20;
            transform.position = new Vector3(0.0f, floorHeight, 0.0f);
        }
        else if (floorHeight >= 100)
        {
            floorHeight = 100;
            transform.position = new Vector3(0.0f, floorHeight, 0.0f);
        }
        else
        {
            transform.position = new Vector3(0.0f, floorHeight, 0.0f);
        }
    }
}
