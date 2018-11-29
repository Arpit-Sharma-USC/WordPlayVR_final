using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class obstacleSound : MonoBehaviour {
   // public GameObject temp;
	// Use this for initialization
	void Start () {
        Debug.Log("Inside start");

    }

    // Update is called once per frame
    void Update () {
		
	}

    void OnCollisionEnter(Collision col)
    {
        Debug.Log("Inside");
      //  if(temp.)
        if (col.gameObject.tag=="HandController")
        {
            Debug.Log("In collision compare");
            AudioClip audioClip = Resources.Load("Audio/bad-beep-incorrect") as AudioClip;
            AudioSource audioSource = GameObject.Find("Furniture_foliageplant_01_LOD0").GetComponent<AudioSource>();
            audioSource.clip = audioClip;
            audioSource.Play();
        }
    }

}
