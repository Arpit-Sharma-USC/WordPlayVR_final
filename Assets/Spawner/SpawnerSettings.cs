using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

using UnityEditor;


//public static class spawnerInfo
//{
//    public static int noOfRows, noOfColumns;
//    public static float spacing;
//}


[ExecuteInEditMode]
public class SpawnerSettings : MonoBehaviour
{
    /////Spawned Location Indicator Object/////
    public GameObject spawnerPlane;

    /////Type of Spawner/////
    public bool spawnAtMultiplePlaces = false;

    /////Spawn Area Setup/////
    public int noOfRows = 1, noOfColumns = 1;
    public float spacing = 1;
    public float minHeight = 0;
    public float maxHeight = 0;

    /////User Based Setup/////
    //User Area - space left for the user;
    //if 0, no space; if 1, 1 to 4 squares in the centre; 
    //every next number adds a layer of squares.
    public int userArea = 1;
    public bool alphabetsFaceUsersEye = true;
    public bool useCustomUserEyeLocation = false;
    public Vector3 userEyePoint;
    public bool limitMaximumPitchChange = false;
    public float maximumPitchChange = 30.0f;

    /////Spawn Dynamics/////
    public float spawnIntensity = 100;
    public float spawnInterval = 3;
    public bool gravity = false;

    /////Floating Effect/////
    public bool floatingEffect = true;
    bool tempFloatingEffect = true;
    public float height = 0.01f;
    public float frequency = 1;
    public bool spin = false;
    public float rotationSpeed = 15.0f;

    /////Miscellaneous Factors/////
    public bool repetiton = true;
    public int repetitionCount = 0;
    public string solutionString;
    public bool spawnAllOtherAlphabets = true;
    public string otherAlphabetsToSpawn = "ajhgklscbzx";

    /////Spawned Objects/////
    public List<GameObject> gameObjects;
    public int spawnerNumber = 0;
    List<Vector2> existingObjs;
    string solution;
    Vector3 centerPoint;
    float spacingScaleFactor = 10.0f;
    float time = 0;
    //int flyingCount = 0;
    List<GameObject> toFly;
    List<Vector3> flyingTo;
    public float flyingSpeed = 2;
    public GameObject camera;
    // Use this for initialization
    void Start()
    {


        //GameObject spawnedPlanes = Instantiate(spawnerPlane, centerPoint, Quaternion.identity);
        if (Application.isEditor && !Application.isPlaying)
        {
            //Debug.Log("Hello");
        }

        //if (Application.isPlaying)
        if (Application.isPlaying)
        {
            tempFloatingEffect = floatingEffect;
            toFly = new List<GameObject>();
            flyingTo = new List<Vector3>();
            existingObjs = new List<Vector2>();
            preFill();
            GameObject[] alphabets = Resources.LoadAll<GameObject>("Spawner Prefabs");
            gameObjects = new List<GameObject>();
            GameObject Parent = GameObject.Find("Parent");
            //Parent.transform.position = (new Vector3(500, 500, 500));
            foreach (Transform go in Parent.transform)
            {
                if (go.gameObject.name != "SendInput")
                    go.gameObject.SetActive(false);
            }
            //SendInputString ParentInputObject = Parent.GetComponent<SendInputString>();
            //Debug.Log("The solution string is " + solutionString);

            //If receiving from start scene.
            //if (ParentInputObject)
            //{
            //ContainerGenerator CG =  GameObject.Find("ContainerBoxParent").GetComponent<ContainerGenerator>();
            int[] bms;
            if (spawnerNumber == 0)
            {
                bms = gameParametersContainer.gameParam.bitMapSubString1;
            }else if (spawnerNumber == 1)
            {
                bms = gameParametersContainer.gameParam.bitMapSubString2;
            }else
            {
                bms = gameParametersContainer.gameParam.bitMapSubString3;
            }
            
            solution = gameParametersContainer.gameParam.input[spawnerNumber];
            spawnInterval = gameParametersContainer.gameParam.SpawnInterval[spawnerNumber];
            spin = gameParametersContainer.gameParam.spin[spawnerNumber];
            alphabetsFaceUsersEye = gameParametersContainer.gameParam.AlphabetsFaceUser[spawnerNumber];
            if (gameParametersContainer.gameParam.repeatSolution[spawnerNumber])
            {
                repetitionCount = 1;
            }
            flyingSpeed = gameParametersContainer.gameParam.FlyingSpeed / 25.0f;
            rotationSpeed = (gameParametersContainer.gameParam.RotationSpeed * 15.0f) / 50.0f;
            //update other scalability features from the start scene - disabled for prototype. 
            //spawnAllOtherAlphabets = !(ParentInputObject.valueOfMissingLettersOnlyToggle);
            //spin = ParentInputObject.valueOfSpinToggle;
            //spawnInterval = ParentInputObject.spawnIntervalFromUI;


            for (int i = 0; i <= repetitionCount; i++)
                for (int k = 0; k < solution.Length; k++)
                {
                    if (bms[k] == 0)
                        foreach (GameObject go in alphabets)
                        {
                            if (char.ToLower(go.name[0]) == char.ToLower(solution[k]))
                                if (repetiton)
                                    gameObjects.Add(go);
                                else if (!gameObjects.Contains(go))
                                    gameObjects.Add(go);
                        }
                }
            // }
            //else
            //{
            //    solution = solutionString;
            //    for (int i = 0; i <= repetitionCount; i++)
            //        foreach (char letter in solution)
            //        {
            //            foreach (GameObject go in alphabets)
            //            {
            //                if (char.ToLower(go.name[0]) == char.ToLower(letter))
            //                    if (repetiton)
            //                        gameObjects.Add(go);
            //                    else if (!gameObjects.Contains(go))
            //                        gameObjects.Add(go);
            //            }
            //        }

            //}

            List<GameObject> nonSolutionAlphabets = new List<GameObject>();
            foreach (GameObject go in alphabets)
                if (!gameObjects.Contains(go))
                    nonSolutionAlphabets.Add(go);


            if (spawnAllOtherAlphabets)
            {
                nonSolutionAlphabets = nonSolutionAlphabets.OrderBy(x => Random.value).ToList();
                foreach (GameObject go in nonSolutionAlphabets)
                    gameObjects.Add(go);
            }
            else
            {
                foreach (GameObject go in nonSolutionAlphabets)
                    if (otherAlphabetsToSpawn.ToLower().Contains(char.ToLower(go.name[0])))
                        gameObjects.Add(go);
            }


            foreach (GameObject go in gameObjects) Debug.Log(go.name);

            //centerPoint.Set(0.6f,1.0869f,0.6f);
            //centerPoint.Set(-3f, 4f, -5f);
            if (useCustomUserEyeLocation)
                centerPoint = userEyePoint;
            else
                centerPoint.Set(0f, 7.2f, 0f);
            //gameObjects = new GameObject[noOfSpawnedObjects];
            foreach (Transform child in transform)
            {
                GameObject.Destroy(child.gameObject);
            }

            if (gameObjects.Count > 0)
                //If no restriction on spawning duplicates.
                if (spawnAtMultiplePlaces)
                {
                    for (int i = 0; i < noOfRows; i++)
                        for (int j = 0; j < noOfColumns; j++)
                        {
                            //Debug.Log("in spawner!");
                            Vector3 spawnPos = transform.position;
                            spawnPos.x += i * (spawnerPlane.GetComponent<Renderer>().bounds.size.x + spacing / spacingScaleFactor);
                            spawnPos.z += j * (spawnerPlane.GetComponent<Renderer>().bounds.size.z + spacing / spacingScaleFactor);
                            if (Random.Range(0.0f, 100.0f) < spawnIntensity)
                            {
                                int randGOindex = Random.Range(0, gameObjects.Count);
                                float y_variation = Random.Range(minHeight, maxHeight);
                                spawnPos.y += y_variation;
                                spawnPos.y += gameObjects[randGOindex].GetComponent<Renderer>().bounds.size.y / 2;
                                GameObject spawnedGO = Instantiate(gameObjects[randGOindex], spawnPos, Quaternion.identity);
                                spawnedGO.transform.parent = transform;
                                Rigidbody rb = spawnedGO.GetComponent<Rigidbody>();
                                if (rb)
                                    rb.useGravity = gravity;
                                // Debug.Log("Spawning!");
                            }
                        }
                }
                //If each object is to be spawned only once. The Currently worked on one.
                else
                {

                    for (int g = 0; g < gameObjects.Count; g++)
                    {
                        Debug.Log("Obj: " + g);
                        float randIntensity = Random.Range(0.0f, 100.0f);
                        Debug.Log("SpawnIntensity: " + randIntensity);
                        if (randIntensity < spawnIntensity)
                        {
                            //Debug.Log("Entering.");
                            int randx = Random.Range(0, noOfRows);
                            int randz = Random.Range(0, noOfColumns);
                            Debug.Log("Location generated: " + randx + "," + randz);

                            //Check if spot is already occupied.
                            if (existingObjs.Contains(new Vector2(randx, randz)))
                            {
                                Debug.Log("Location not free!");

                                //If grid still has spots, check next available spot from current spot.
                                if (existingObjs.Count <= (noOfRows * noOfColumns))
                                {
                                    bool foundSpot = false;
                                    //Check for next available spot from the current spot.
                                    Debug.Log("Checking next available spot from current spot.");
                                    for (int i = randx + 1; i < noOfRows; i++)
                                        for (int j = 0; j < noOfColumns; j++)
                                        {
                                            if (i == randx && j <= randz)
                                                continue;
                                            if (!existingObjs.Contains(new Vector2(i, j)) && !foundSpot)
                                            {
                                                spawn(i, j, g);
                                                foundSpot = true;
                                                Debug.Log("Found a spot");
                                            }
                                        }

                                    //Check for a spot from start to current spot if spot not already found.
                                    if (!foundSpot)
                                    {
                                        Debug.Log("Didnt find a spot, trying from start.");
                                        for (int i = 0; i <= randx; i++)
                                            for (int j = 0; j < noOfColumns; j++)
                                                if (!existingObjs.Contains(new Vector2(i, j)) && !foundSpot)
                                                {
                                                    spawn(i, j, g);
                                                    foundSpot = true;
                                                    Debug.Log("Found a spot");
                                                }
                                    }

                                }
                            }

                            //Current spot was free.
                            else
                            {
                                Debug.Log("Location free!");
                                spawn(randx, randz, g);

                            }
                        }


                    }

                }
            else
                Debug.Log("No gameobjects specified for spawner!");
        }
    }

    // Update is called once per frame
    void Update()
    {
        //For displaying in editor, no gameplay stuff in here.       
        if (Application.isEditor && !Application.isPlaying)
        {
            while (transform.childCount != 0)
            {
                DestroyImmediate(transform.GetChild(0).gameObject);
            }
            existingObjs = new List<Vector2>();
            preFill();
            for (int i = 0; i < noOfRows; i++)
                for (int j = 0; j < noOfColumns; j++)
                {
                    if (!existingObjs.Contains(new Vector2(i, j)))
                    {
                        Vector3 spawnPos = transform.position;
                        spawnPos.x += i * (spawnerPlane.GetComponent<Renderer>().bounds.size.x + spacing / spacingScaleFactor);
                        spawnPos.z += j * (spawnerPlane.GetComponent<Renderer>().bounds.size.z + spacing / spacingScaleFactor);
                        spawnPos.y += minHeight;
                        GameObject spawnedPlanes = Instantiate(spawnerPlane, spawnPos, Quaternion.identity);
                        spawnedPlanes.transform.parent = transform;
                    }
                }
            if (maxHeight != 0)
                for (int i = 0; i < noOfRows; i++)
                    for (int j = 0; j < noOfColumns; j++)
                    {
                        if (!existingObjs.Contains(new Vector2(i, j)))
                        {
                            Vector3 spawnPos = transform.position;
                            spawnPos.x += i * (spawnerPlane.GetComponent<Renderer>().bounds.size.x + spacing / spacingScaleFactor);
                            spawnPos.z += j * (spawnerPlane.GetComponent<Renderer>().bounds.size.z + spacing / spacingScaleFactor);
                            spawnPos.y += maxHeight;
                            GameObject spawnedPlanes = Instantiate(spawnerPlane, spawnPos, Quaternion.identity);
                            spawnedPlanes.transform.parent = transform;
                        }
                    }
        }

        //Actual game code.
        if (Application.isPlaying)
        {
            foreach (Transform child in transform)
            {
                SpawnerObjectController soc = child.gameObject.GetComponent<SpawnerObjectController>();
                soc.spin = spin;
                //soc.floatingEffect = floatingEffect;
                if (floatingEffect && !soc.floatingEffect)
                {
                    soc.startFloating();
                }
                else if (!floatingEffect && soc.floatingEffect)
                    soc.floatingEffect = floatingEffect;
                //child.parent = transform.parent;
                //existingObjs.Remove(soc.pos);
                //Debug.Log("Found and set");
                if (alphabetsFaceUsersEye)
                {
                    child.gameObject.transform.LookAt(camera.transform.position);
                    child.gameObject.transform.Rotate(0, 180, 0, Space.Self);
                }
                //foreach (GameObject go in GameObject.FindGameObjectsWithTag("InteractionObject"))
                //    if (go.GetComponent<SpawnerObjectController>())
                //    {
                //        Debug.Log("gameObjectttttttttttttttttttttttttttttt: "+go.name);


                //    }
            }
            foreach (GameObject go in GameObject.FindGameObjectsWithTag("InteractionObject"))
                if (go.GetComponent<SpawnerObjectController>() && go.transform.position.y - 0.1 <= (transform.position.y + go.GetComponent<Renderer>().bounds.size.y / 2)
                    || go.transform.position.y - 0.1 <= (transform.position.y + go.GetComponent<Renderer>().bounds.size.x / 2)
                    || go.transform.position.y - 0.1 <= (transform.position.y + go.GetComponent<Renderer>().bounds.size.z / 2))
                {
                    Debug.Log("fallen");
                    string name = go.name;
                    SpawnerObjectController socloc = go.GetComponent<SpawnerObjectController>();
                    //existingObjs.Remove(socloc.pos);
                    GameObject.Destroy(go);
                    for (int g = 0; g < gameObjects.Count; g++)
                        if (char.ToLower(gameObjects[g].name[0]) == char.ToLower(name[0]))
                        {
                            Debug.Log("Obj: " + g);
                            //float randIntensity = Random.Range(0.0f, 100.0f);
                            //Debug.Log("SpawnIntensity: " + randIntensity);
                            //if (randIntensity < spawnIntensity)
                            if (true)
                            {
                                //Debug.Log("Entering.");
                                int randx = Random.Range(0, noOfRows);
                                int randz = Random.Range(0, noOfColumns);
                                Debug.Log("Location generated: " + randx + "," + randz);

                                //Check if spot is already occupied.
                                if (existingObjs.Contains(new Vector2(randx, randz)))
                                {
                                    Debug.Log("Location not free!");

                                    //If grid still has spots, check next available spot from current spot.
                                    if (existingObjs.Count <= (noOfRows * noOfColumns))
                                    {
                                        bool foundSpot = false;
                                        //Check for next available spot from the current spot.
                                        Debug.Log("Checking next available spot from current spot.");
                                        for (int i = randx + 1; i < noOfRows; i++)
                                            for (int j = 0; j < noOfColumns; j++)
                                            {
                                                if (i == randx && j <= randz)
                                                    continue;
                                                if (!existingObjs.Contains(new Vector2(i, j)) && !foundSpot)
                                                {
                                                    spawn(i, j, g);
                                                    foundSpot = true;
                                                    Debug.Log("Found a spot");
                                                }
                                            }

                                        //Check for a spot from start to current spot if spot not already found.
                                        if (!foundSpot)
                                        {
                                            Debug.Log("Didnt find a spot, trying from start.");
                                            for (int i = 0; i <= randx; i++)
                                                for (int j = 0; j < noOfColumns; j++)
                                                    if (!existingObjs.Contains(new Vector2(i, j)) && !foundSpot)
                                                    {
                                                        spawn(i, j, g);
                                                        foundSpot = true;
                                                        Debug.Log("Found a spot");
                                                    }
                                        }

                                    }
                                }

                                //Current spot was free.
                                else
                                {
                                    Debug.Log("Location free!");
                                    spawn(randx, randz, g);

                                }
                            }
                        }

                }


            


            if (toFly.Count == 0)
            {
                time += Time.deltaTime;
                floatingEffect = tempFloatingEffect;
            }


            else
            {
                for (int i = 0; i < toFly.Count; i++)
                {
                    toFly[i].transform.position = Vector3.MoveTowards(toFly[i].transform.position, flyingTo[i], flyingSpeed * Time.deltaTime);
                    if (toFly[i].transform.position == flyingTo[i])
                    {
                        toFly.Remove(toFly[i]);
                        flyingTo.Remove(flyingTo[i]);
                        i--;
                    }

                }
            }
            if (time >= spawnInterval)
            {
                tempFloatingEffect = floatingEffect;
                floatingEffect = false;
                foreach (Transform child in transform)
                {
                    toFly.Add(child.gameObject);
                    //child.gameObject.GetComponent<SpawnerObjectController>().floatingEffect = floatingEffect;
                }

                existingObjs.Clear();
                for (int p = 0; p < toFly.Count; p++)
                {
                    //Debug.Log("Entering.");
                    int randx = Random.Range(0, noOfRows);
                    int randz = Random.Range(0, noOfColumns);
                    Debug.Log("Location generated: " + randx + "," + randz);

                    //Check if spot is already occupied.
                    if (existingObjs.Contains(new Vector2(randx, randz)))
                    {
                        Debug.Log("Location not free!");

                        //If grid still has spots, check next available spot from current spot.
                        if (existingObjs.Count <= (noOfRows * noOfColumns))
                        {
                            bool foundSpot = false;
                            //Check for next available spot from the current spot.
                            Debug.Log("Checking next available spot from current spot.");
                            for (int i = randx + 1; i < noOfRows; i++)
                                for (int j = 0; j < noOfColumns; j++)
                                {
                                    if (i == randx && j <= randz)
                                        continue;
                                    if (!existingObjs.Contains(new Vector3(i, j)) && !foundSpot)
                                    {
                                        flyingTo.Add(new Vector3(i, 0, j));
                                        foundSpot = true;
                                        Debug.Log("Found a spot");
                                    }
                                }

                            //Check for a spot from start to current spot if spot not already found.
                            if (!foundSpot)
                            {
                                Debug.Log("Didnt find a spot, trying from start.");
                                for (int i = 0; i <= randx; i++)
                                    for (int j = 0; j < noOfColumns; j++)
                                        if (!existingObjs.Contains(new Vector2(i, j)) && !foundSpot)
                                        {
                                            flyingTo.Add(new Vector3(i, 0, j));
                                            foundSpot = true;
                                            Debug.Log("Found a spot");
                                        }
                            }
                        }
                    }

                    //Current spot was free.
                    else
                    {
                        Debug.Log("Location free!");
                        flyingTo.Add(new Vector3(randx, 0, randz));

                    }
                }

                //convert flying pos from row-col to locations.
                for (int i = 0; i < flyingTo.Count; i++)
                {
                    Vector3 spawnPos = transform.position;
                    spawnPos.x += flyingTo[i].x * (spawnerPlane.GetComponent<Renderer>().bounds.size.x + spacing / spacingScaleFactor);
                    spawnPos.z += flyingTo[i].z * (spawnerPlane.GetComponent<Renderer>().bounds.size.z + spacing / spacingScaleFactor);
                    spawnPos.y += Random.Range(minHeight, maxHeight);

                    flyingTo[i] = spawnPos;
                }



                




                time = 0;
                

            }
        }
    }

    void spawn(int rowSpawnedAt, int colSpawnedAt, int gameObjectIndex)
    {
        Vector3 spawnPos = transform.position;
        spawnPos.x += rowSpawnedAt * (spawnerPlane.GetComponent<Renderer>().bounds.size.x + spacing / spacingScaleFactor);
        spawnPos.z += colSpawnedAt * (spawnerPlane.GetComponent<Renderer>().bounds.size.z + spacing / spacingScaleFactor);
        spawnPos.y += Random.Range(minHeight, maxHeight);
        spawnPos.y += gameObjects[gameObjectIndex].GetComponent<Renderer>().bounds.size.y / 2;
        GameObject spawnedGO = Instantiate(gameObjects[gameObjectIndex], spawnPos, Quaternion.identity);
        spawnedGO.transform.parent = transform;

        if (alphabetsFaceUsersEye)
        {
            spawnedGO.transform.LookAt(camera.transform.position);
            spawnedGO.transform.Rotate(0, 180, 0, Space.Self);
            //spawnedGO.transform.Rotate(Vector3.up, -90);                                                
            //spawnedGO.transform.rotation.eulerAngles.Set(spawnedGO.transform.rotation.eulerAngles.x, spawnedGO.transform.rotation.eulerAngles.y+90,0);
            //spawnedGO.transform.localEulerAngles = (new Vector3(spawnedGO.transform.localEulerAngles.x, spawnedGO.transform.localEulerAngles.y + 180, spawnedGO.transform.localEulerAngles.z));
        }

        if (limitMaximumPitchChange)
        {
            if (spawnedGO.transform.eulerAngles.x > maximumPitchChange && spawnedGO.transform.eulerAngles.x < 180)
                spawnedGO.transform.eulerAngles = new Vector3(maximumPitchChange, spawnedGO.transform.eulerAngles.y, spawnedGO.transform.eulerAngles.z);
            else if ((360 - spawnedGO.transform.eulerAngles.x) > maximumPitchChange && (360 - spawnedGO.transform.eulerAngles.x) < 180)
                spawnedGO.transform.eulerAngles = new Vector3(-maximumPitchChange, spawnedGO.transform.eulerAngles.y, spawnedGO.transform.eulerAngles.z);
        }

        Rigidbody rb = spawnedGO.GetComponent<Rigidbody>();
        if (rb)
        {
            rb.useGravity = gravity;
            rb.isKinematic = !gravity;
        }

        SpawnerObjectController fe = spawnedGO.AddComponent<SpawnerObjectController>();
        fe.frequency = frequency;
        fe.amplitude = height;
        fe.spin = spin;
        fe.degreesPerSecond = rotationSpeed;
        fe.floatingEffect = floatingEffect;
        fe.pos = new Vector2(rowSpawnedAt, colSpawnedAt);

        existingObjs.Add(new Vector2(rowSpawnedAt, colSpawnedAt));

    }

    void preFill()
    {
        if (userArea > 0)
        {
            //Debug.Log("Entered Prefill");
            //Use hashSet instead of lists to avoid duplicate entries.
            HashSet<int> rowNos = new HashSet<int>(), colNos = new HashSet<int>();
            if (noOfRows % 2 != 0)
                for (int i = 0; i < userArea; i++)
                {
                    rowNos.Add((noOfRows - 1) / 2 + i);
                    rowNos.Add((noOfRows - 1) / 2 - i);
                }
            else
                for (int i = 0; i < userArea; i++)
                {
                    rowNos.Add((noOfRows - 1) / 2 + 1 + i);
                    rowNos.Add((noOfRows - 1) / 2 - i);
                }
            //Debug.Log("rowNos count is: " + rowNos.Count);
            //foreach (int item in rowNos) { Debug.Log("Row positions generated in preFill: " + item); }

            if (noOfColumns % 2 != 0)
                for (int i = 0; i < userArea; i++)
                {
                    colNos.Add((noOfColumns - 1) / 2 + i);
                    colNos.Add((noOfColumns - 1) / 2 - i);
                }
            else
                for (int i = 0; i < userArea; i++)
                {
                    colNos.Add((noOfColumns - 1) / 2 + 1 + i);
                    colNos.Add((noOfColumns - 1) / 2 - i);
                }

            //Debug.Log("colNos count is: " + colNos.Count);
            //foreach (int item in colNos) { Debug.Log("Col positions generated in preFill: " + item); }

            foreach (int rowNo in rowNos)
                foreach (int colNo in colNos)
                    if (!existingObjs.Contains(new Vector2(rowNo, colNo)))
                        existingObjs.Add(new Vector2(rowNo, colNo));



            //foreach (Vector2 item in existingObjList) { Debug.Log("Positions generated in preFill: " + item.x + "," + item.y); }
        }

    }

    public void remove(Vector2 pos)
    {
        existingObjs.Remove(pos);
    }

    //IEnumerator waitAndFly(float waitTime, int rowNo, int colNo, GameObject go)
    //{
    //    yield return new WaitForSeconds(waitTime);
    //    go.transform.position = Vector3.MoveTowards(transform.position, target.position, step);
    //}
}
//[CustomEditor(typeof(SpawnerSettings))]
//public class SpawnerSettingsEditor : Editor
//{
//    SpawnerSettings script;
//    void OnEnable()
//    {
//        script = (SpawnerSettings)target;
//    }
//    public override void OnInspectorGUI()
//    {
//        EditorGUI.indentLevel++;
//        if (script.spawnAtMultiplePlaces)
//        {
//            script.spawnIntensity = EditorGUILayout.Slider("Percentage chance of spawning at each location", script.spawnIntensity, 1, 100);
//        }

//    }

//}



