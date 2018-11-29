using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParameterController : MonoBehaviour {

    GameObject targetObj; // drag the object with the Clips variable here
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        // get a reference to the target script (ScriptName is the name of your script):

        SpawnerSettings targetScript = targetObj.GetComponent<SpawnerSettings>();

        // use the targetScript reference to access the variables:

        //targetScript.alphabetsFaceUsersEye = gPc.gamePara.alphabetsFaceUsersEye;

        //targetScript.spawnInterval = gPc.gamePara.spawnInterval;

        //targetScript.gravity = gPc.gamePara.gravity;

        //targetScript.floatingEffect = gPc.gamePara.floatingEffect;

        //targetScript.spin = gPc.gamePara.spin;

        //targetScript.rotationSpeed = gPc.gamePara.rotationSpeed;

        //targetScript.flyingSpeed = gPc.gamePara.flyingSpeed;


    }
}
