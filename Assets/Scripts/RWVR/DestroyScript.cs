using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyScript : MonoBehaviour {

    private float destroyTime = 20.0f;
    public bool flag=false;

    //gameParametersContainer.gameParam.respawnTime;Debug.Log ;//+gameParametersContainer.gameParam.respawnTime);

   // private float rotateSpeed = 80.0f;

	// Use this for initialization
	void Start () {
        if(!(gameParametersContainer.gameParam==null))
            destroyTime = float.Parse(gameParametersContainer.gameParam.respawnTime);
       
    }
	
	// Update is called once per frame
	void Update () {
        //transform.Rotate(Vector3.right * Time.deltaTime * rotateSpeed);
        //  Destroy(gameObject, destroyTime);
        if (flag == false)
        {
         //   Debug.Log(destroyTime + "Destroy time");
        }


       
    }
    void OnCollisionEnter(Collision collision)
    {
       if((collision.gameObject.tag== "HandController") && flag==false)
        {
            //Destroy(gameObject, destroyTime);
            flag = true;
            Debug.Log("Destroy not called");
        }
        //if (!(collision.gameObject.tag == "HandController"))
        //{
        //    //Destroy(gameObject, destroyTime);
        //    flag = false;
        //    Destroy(gameObject, destroyTime);
        //    //   Debug.Log("Destroy called");
        //}
        Destroy(gameObject, destroyTime);

    }
}
