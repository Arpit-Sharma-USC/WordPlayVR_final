using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnerObjectController : MonoBehaviour
{
    // User Inputs
    public float degreesPerSecond = 15.0f;
    public float amplitude = 0.01f;
    public float frequency = 1f;
    public bool spin = false;
    public bool floatingEffect = false;
    public bool interacted = false;
    float startingPoint;
    public Vector2 pos;

    // Position Storage Variables
    Vector3 posOffset = new Vector3();
    Vector3 tempPos = new Vector3();

    // Use this for initialization
    void Start()
    {
        // Store the starting position & rotation of the object
        posOffset = transform.position;
        startingPoint = Random.Range(0.0f, 6.283f);
    }

    // Update is called once per frame
    void Update()
    {
        if (spin)
            transform.Rotate(new Vector3(0f, Time.deltaTime * degreesPerSecond, 0f), Space.World);

        // Float up/down with a Sin()
        if (floatingEffect)
        {
            tempPos = posOffset;

            tempPos.y += Mathf.Sin(Time.fixedTime * Mathf.PI * frequency + startingPoint) * amplitude;

            transform.position = tempPos;
        }

    }
    public void startFloating()
    {
        posOffset = transform.position;
        floatingEffect = true;
        startingPoint = Random.Range(0.0f, 6.283f);
    }
}
